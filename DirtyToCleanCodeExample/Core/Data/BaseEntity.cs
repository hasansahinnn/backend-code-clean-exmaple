namespace Core.Data;

/// <summary>
/// Base entity class
/// <para>Contains the Id property.</para>
/// <para>Every entity class should inherit from this class.</para>
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Id property
    /// <para>Primary key of the entity</para>
    /// </summary>
    public int Id { get; set; }

}