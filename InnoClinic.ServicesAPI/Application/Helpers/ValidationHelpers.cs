using Domain.Enums;

namespace Application.Helpers;

public static class ValidationHelpers
{
    public static bool BeValidStatus(string status) =>
        Enum.TryParse(typeof(Statuses), status, out _);
}