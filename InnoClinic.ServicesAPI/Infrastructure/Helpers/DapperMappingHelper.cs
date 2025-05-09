namespace Infrastructure.Helpers;

public static class DapperMappingHelper
{
    public static Func<TParent, TChild, TParent> MapWithChildren<TParent, TChild>(
        IDictionary<Guid, TParent> dict,
        Func<TParent, Guid> parentIdSelector,
        Action<TParent, List<TChild>> initializeChildren,
        Action<TParent, TChild> addChild)
    {
        return (parent, child) =>
        {
            if (!dict.TryGetValue(parentIdSelector(parent), out var existing))
            {
                existing = parent;
                initializeChildren(existing, new List<TChild>());
                dict.Add(parentIdSelector(existing), existing);
            }

            if (child is not null) addChild(existing, child);

            return existing;
        };
    }
}