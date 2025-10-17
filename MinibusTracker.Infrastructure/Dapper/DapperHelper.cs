using Dapper;

namespace MinibusTracker.Infrastructure.Dapper;

public static class DapperHelper
{
     /// <summary>
     /// Ensures Dapper correctly maps database column names to C# class properties using ColumnAttribute mappings.
     /// This method sets up custom property mapping that looks for System.ComponentModel.DataAnnotations.Schema.ColumnAttribute
     /// decorations on properties to match database column names, falling back to case-insensitive property name matching.
     /// </summary>
     /// <typeparam name="T">The entity class type that needs custom column mapping</typeparam>
     /// <remarks>
     /// Call this method once per entity type, typically during application startup or before first database access.
     /// This is particularly useful when database column names use different naming conventions than C# properties
     /// (e.g., snake_case columns vs PascalCase properties).
     /// </remarks>
     /// <example>
     /// <code>
     /// // Entity class with column mappings
     /// public class User
     /// {
     ///     public int Id { get; set; }
     ///     
     ///     [Column("first_name")]
     ///     public string FirstName { get; set; }
     ///     
     ///     [Column("last_name")]
     ///     public string LastName { get; set; }
     ///     
     ///     public string Email { get; set; } // Maps directly by name
     /// }
     /// 
     /// // Setup mapping (call once during startup)
     /// DapperHelper.UsePropertyAttributeMapping&lt;User&gt;();
     /// 
     /// // Now Dapper queries will work correctly
     /// var users = connection.Query&lt;User&gt;("SELECT id, first_name, last_name, email FROM users");
     /// </code>
     /// </example>
    public static void UsePropertyAttributeMapping<T>() where T : class
    {
        if (SqlMapper.GetTypeMap(typeof(T)) is DefaultTypeMap)
        {
            SqlMapper.SetTypeMap(typeof(T), new CustomPropertyTypeMap(
                typeof(T),
                (type, columnName) => Array.Find(
                    type.GetProperties(),
                    prop => prop.GetCustomAttributes(false)
                        .OfType<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>()
                        .Any(attr => attr.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                        || prop.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)
                )));
        }
    }
}
