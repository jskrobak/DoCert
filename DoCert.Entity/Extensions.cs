using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace DoCert.Entity;

public static class Extensions
{
    public static void RegisterModelFromAssembly(
        this 
#nullable disable
            ModelBuilder modelBuilder,
        Assembly assembly,
        string namespaceName = null)
    {
        Contract.Requires(modelBuilder != null, "modelBuilder != null");
        Contract.Requires(assembly != (Assembly) null, "assembly != null");
        foreach (Type type in ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type =>
                 {
                     if (!type.IsPublic || !type.IsClass)
                         return false;
                     return !type.IsAbstract || !type.IsSealed;
                 })).Where<Type>((Func<Type, bool>) (type => string.IsNullOrEmpty(namespaceName) || type.Namespace.StartsWith(namespaceName))).ToList<Type>())
        {
            if (!type.GetCustomAttributes<NotMappedAttribute>().Any<NotMappedAttribute>() && !type.GetCustomAttributes<ComplexTypeAttribute>().Any<ComplexTypeAttribute>() && !type.GetCustomAttributes<OwnedAttribute>().Any<OwnedAttribute>())
                modelBuilder.Entity(type);
        }
    }
}