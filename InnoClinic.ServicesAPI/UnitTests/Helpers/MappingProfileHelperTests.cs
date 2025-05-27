using Application.Helpers;
using Domain.Enums;
using FluentAssertions;
using UnitTests.TestCases;

namespace UnitTests.Helpers;

public class MappingProfileHelperTests
{
    [Theory]
    [InlineData(MappingProfileHelperTestCases.ValidStatus, Statuses.Active)]
    [InlineData(MappingProfileHelperTestCases.InvalidStatus, Statuses.Inactive)]
    [InlineData(MappingProfileHelperTestCases.MixedCaseStatus, Statuses.Active)]
    [InlineData(MappingProfileHelperTestCases.EmptyStatus, Statuses.Inactive)]
    [InlineData(MappingProfileHelperTestCases.NullStatus, Statuses.Inactive)]
    public void ParseStatusShouldReturnExpectedStatus(string? statusStr, Statuses expected)
    {
        var result = MappingProfileHelper.ParseStatus(statusStr!);

        result.Should().Be(expected);
    }
}