using FluentAssertions;
using UnitTests.TestCases;
using Infrastructure.Helpers;

namespace UnitTests.Helpers;

public class DapperMappingHelperTests
{
    private class TestParent
    {
        public Guid Id { get; set; }
        public List<TestChild>? Children { get; set; }
    }

    private class TestChild
    {
        public string? Value { get; set; }
    }

    [Fact]
    public void MapWithChildrenShouldAddParentToDictionaryWhenNotExists()
    {
        var dict = new Dictionary<Guid, TestParent>();
        var parent = new TestParent { Id = Guid.NewGuid() };
        var child = new TestChild { Value = DapperMappingHelperTestCases.ChildValue1 };

        var mapper = DapperMappingHelper.MapWithChildren<TestParent, TestChild>(dict, p => p.Id, (p, children) => p.Children = children, (p, c) => p.Children?.Add(c));

        var result = mapper(parent, child);

        dict.Should().ContainKey(parent.Id);
        dict[parent.Id].Should().Be(parent);
        result.Should().Be(parent);
        parent.Children.Should().NotBeNull();
        parent.Children.Should().Contain(child);
    }

    [Fact]
    public void MapWithChildrenShouldAddChildToExistingParentWhenParentExists()
    {
        var parentId = Guid.NewGuid();
        var existingParent = new TestParent { Id = parentId, Children = new List<TestChild>() };
        var dict = new Dictionary<Guid, TestParent> { { parentId, existingParent } };

        var newParentInstance = new TestParent { Id = parentId };
        var child = new TestChild { Value = DapperMappingHelperTestCases.ChildValue2 };

        var mapper = DapperMappingHelper.MapWithChildren<TestParent, TestChild>(dict, p => p.Id, (p, children) => p.Children = children, (p, c) => p.Children?.Add(c));

        var result = mapper(newParentInstance, child);

        result.Should().Be(existingParent);
        existingParent.Children.Should().Contain(child);
        existingParent.Children.Count.Should().Be(1);
    }

    [Fact]
    public void MapWithChildrenShouldHandleNullChild()
    {
        var dict = new Dictionary<Guid, TestParent>();
        var parent = new TestParent { Id = Guid.NewGuid() };
        TestChild? child = null;

        var mapper = DapperMappingHelper.MapWithChildren<TestParent, TestChild>(dict, p => p.Id, (p, children) => p.Children = children, (p, c) => p.Children?.Add(c));

        var result = mapper(parent, child!);

        dict.Should().ContainKey(parent.Id);
        result.Children.Should().BeEmpty();
    }
}