namespace underware.Data.EntityFrameworkCore;

/// <summary>Sestavuje query tagy.</summary>
public static class QueryTagBuilder
{
    /// <summary>Sestaví Query Tag pro daného membera z daného typu.</summary>
    public static string CreateTag(Type type, string memberName)
    {
        return memberName == null ? type.Name : type.Name + "." + memberName;
    }
}