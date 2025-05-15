using Dapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Moq;
using Moq.Dapper;
using System.Data;
using UnitTests.TestCases;
using Xunit;

namespace UnitTests.Repositories
{
    public class SpecializationRepositoryTests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly SpecializationRepository _repository;

        public SpecializationRepositoryTests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _repository = new SpecializationRepository(_mockConnection.Object);
        }

        [Fact]
        public async Task CreateAsyncShouldExecuteInsert()
        {
            var spec = new Specialization
            {
                Id = Guid.NewGuid(),
                Name = "Test Spec",
                Status = Statuses.Active
            };

            _mockConnection.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<Specialization>(), null, null, null)).ReturnsAsync(1);

            var result = await _repository.CreateAsync(spec);

            Assert.Equal(spec.Id, result.Id);
        }

        [Fact]
        public async Task UpdateAsyncShouldExecuteUpdate()
        {
            var spec = new Specialization
            {
                Id = Guid.NewGuid(),
                Name = "Updated Spec",
                Status = Statuses.Inactive
            };

            _mockConnection.SetupDapperAsync(c => c.QuerySingleAsync<Specialization>(It.IsAny<string>(), It.IsAny<Specialization>(), null, null, null)).ReturnsAsync(spec);

            var result = await _repository.UpdateAsync(spec);

            Assert.Equal("Updated Spec", result.Name);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAllSpecializations()
        {
            var specializations = new List<Specialization>
            {
                new Specialization { Id = Guid.NewGuid(), Name = "Spec 1" },
                new Specialization { Id = Guid.NewGuid(), Name = "Spec 2" }
            };

            _mockConnection.SetupDapperAsync(c => c.QueryAsync<Specialization>(It.IsAny<string>(), null, null, null, null)).ReturnsAsync(specializations);

            var result = await _repository.GetAllAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnSpecialization()
        {
            var specId = Guid.NewGuid();
            var expected = new Specialization { Id = specId, Name = "Test Spec" };

            _mockConnection.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<Specialization>(It.IsAny<string>(), It.Is<object>(o => CheckIdParameter(o, specId)), null, null, null)).ReturnsAsync(expected);

            var result = await _repository.GetByIdAsync(specId);

            Assert.Equal(specId, result?.Id);
        }

        [Fact]
        public async Task DeleteAsyncShouldExecuteDelete()
        {
            var specId = Guid.NewGuid();
            _mockConnection.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.Is<object>(o => CheckIdParameter(o, specId)), null, null, null)).ReturnsAsync(1);

            await _repository.DeleteAsync(specId);
        }

        [Fact]
        public async Task GetWithDependenciesAsyncShouldReturnSpecializationWithRelatedServices()
        {
            using var connection = new SqlConnection(TestConfiguration.ConnectionString);

            await connection.OpenAsync();

            await connection.ExecuteAsync(SpecializationRepositoryTestConstants.CheckDbExistsSql);
            await connection.ExecuteAsync(SpecializationRepositoryTestConstants.CreateDbSql);

            var useDbSql = SpecializationRepositoryTestConstants.UseDbSql;
            await connection.ExecuteAsync(useDbSql);

            var createTablesSql = SpecializationRepositoryTestConstants.CreateTablesSql;
            await connection.ExecuteAsync(createTablesSql);

            var categoryId = Guid.NewGuid();
            var specializationId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();

            await connection.ExecuteAsync(
            SpecializationRepositoryTestConstants.InsertCategorySql,
            new { Id = categoryId, Name = "Therapy", Duration = 30 });

            await connection.ExecuteAsync(
                SpecializationRepositoryTestConstants.InsertSpecializationSql,
                new { Id = specializationId, Name = "General", Status = 1 });

            await connection.ExecuteAsync(
                SpecializationRepositoryTestConstants.InsertServiceSql,
                new
                {
                    Id = serviceId,
                    Name = "Massage",
                    Price = 50m,
                    Status = 1,
                    CategoryId = categoryId,
                    SpecializationId = specializationId
                });

            var repository = new SpecializationRepository(connection);

            var result = await repository.GetWithDependenciesAsync(specializationId);

            result.Should().NotBeNull();
            result!.Id.Should().Be(specializationId);
            result.Services.Should().Contain(s => s.Id == serviceId);

            await connection.ExecuteAsync(SpecializationRepositoryTestConstants.CloseConnectionsSql);
            await connection.CloseAsync();

            await connection.ExecuteAsync(SpecializationRepositoryTestConstants.DropDbSql);
        }

        [Fact]
        public async Task GetAllWithDependenciesAsyncShouldReturnSpecializationsWithRelatedServices()
        {
            using var connection = new SqlConnection(TestConfiguration.ConnectionString);

            await connection.OpenAsync();

            await connection.ExecuteAsync(SpecializationRepositoryTestConstants.CheckDbExistsSql);
            await connection.ExecuteAsync(SpecializationRepositoryTestConstants.CreateDbSql);

            var useDbSql = SpecializationRepositoryTestConstants.UseDbSql;
            await connection.ExecuteAsync(useDbSql);

            var createTablesSql = SpecializationRepositoryTestConstants.CreateTablesSql;
            await connection.ExecuteAsync(createTablesSql);

            var categoryId1 = Guid.NewGuid();
            var categoryId2 = Guid.NewGuid();
            var specializationId1 = Guid.NewGuid();
            var specializationId2 = Guid.NewGuid();
            var serviceId1 = Guid.NewGuid();
            var serviceId2 = Guid.NewGuid();

            await connection.ExecuteAsync(
                SpecializationRepositoryTestConstants.InsertCategorySql,
                new { Id = categoryId1, Name = "Therapy", Duration = 30 });

            await connection.ExecuteAsync(
                SpecializationRepositoryTestConstants.InsertCategorySql,
                new { Id = categoryId2, Name = "Massage", Duration = 45 });

            await connection.ExecuteAsync(
                SpecializationRepositoryTestConstants.InsertSpecializationSql,
                new { Id = specializationId1, Name = "General", Status = 1 });

            await connection.ExecuteAsync(
                SpecializationRepositoryTestConstants.InsertSpecializationSql,
                new { Id = specializationId2, Name = "Advanced", Status = 1 });

            await connection.ExecuteAsync(
                SpecializationRepositoryTestConstants.InsertServiceSql,
                new
                {
                    Id = serviceId1,
                    Name = "Service1",
                    Price = 100m,
                    Status = 1,
                    CategoryId = categoryId1,
                    SpecializationId = specializationId1
                });

            await connection.ExecuteAsync(
                SpecializationRepositoryTestConstants.InsertServiceSql,
                new
                {
                    Id = serviceId2,
                    Name = "Service2",
                    Price = 150m,
                    Status = 1,
                    CategoryId = categoryId2,
                    SpecializationId = specializationId2
                });

            var repository = new SpecializationRepository(connection);
            var result = await repository.GetAllWithDependenciesAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(s => s.Id == specializationId1 && s.Services.Any(service => service.Id == serviceId1));
            result.Should().Contain(s => s.Id == specializationId2 && s.Services.Any(service => service.Id == serviceId2));

            await connection.ExecuteAsync(SpecializationRepositoryTestConstants.CloseConnectionsSql);
            await connection.CloseAsync();

            await connection.ExecuteAsync(SpecializationRepositoryTestConstants.DropDbSql);
        }

        private static bool CheckIdParameter(object param, Guid expectedId)
        {
            var prop = param.GetType().GetProperty("id") ?? param.GetType().GetProperty("Id");
            if (prop is null) return false;

            var value = prop.GetValue(param);
            return value != null && value.Equals(expectedId);
        }
    }
}