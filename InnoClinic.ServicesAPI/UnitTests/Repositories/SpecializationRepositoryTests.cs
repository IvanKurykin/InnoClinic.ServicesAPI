using Dapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using Moq;
using Moq.Dapper;
using System.Data;
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

        private bool CheckIdParameter(object param, Guid expectedId)
        {
            var prop = param.GetType().GetProperty("id") ?? param.GetType().GetProperty("Id");
            if (prop is null) return false;

            var value = prop.GetValue(param);
            return value != null && value.Equals(expectedId);
        }
    }
}