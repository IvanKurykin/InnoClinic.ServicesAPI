using Application.Helpers;
using UnitTests.TestCases;
using FluentAssertions;

namespace UnitTests.Helpers;

public class ValidationHelpersTests
{
    [Theory]
    [InlineData(ValidationHelperTestCases.ValidStatus, true)]
    [InlineData(ValidationHelperTestCases.InvalidStatus, false)]
    [InlineData(ValidationHelperTestCases.MixedCaseStatus, false)]
    [InlineData(ValidationHelperTestCases.EmptyStatus, false)]
    [InlineData(ValidationHelperTestCases.NullStatus, false)]
    public void BeValidStatusShouldReturnExpectedResult(string? status, bool expected)
    {
        var result = ValidationHelpers.BeValidStatus(status!);

        result.Should().Be(expected);
    }
}