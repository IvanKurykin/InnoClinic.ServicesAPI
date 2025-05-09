using Domain.Enums;

namespace Application.Helpers;

public static class MappingProfileHelper
{
    public static Statuses ParseStatus(string statusStr) => 
        Enum.TryParse<Statuses>(statusStr, true, out var status) ? status: Statuses.Inactive;
}