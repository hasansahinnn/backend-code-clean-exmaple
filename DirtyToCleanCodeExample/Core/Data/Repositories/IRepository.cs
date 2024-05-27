using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Data.Repositories;
/// <summary>
/// Generic repository interface for basic CRUD operations and querying.
/// </summary>
/// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
/// <typeparam name="TContext">The type of DbContext associated with the repository.</typeparam>
public interface IRepository<TEntity, TContext>
    where TEntity : BaseEntity
    where TContext : DbContext
{
    /// <summary>
    /// Saves changes to the repository asynchronously.
    /// </summary>
    Task SaveChangesAsync();
    /// <summary>
    /// Gets the queryable for the entity.
    /// </summary>
    IQueryable<TEntity> Query();
    /// <summary>
    /// Adds an entity to the repository asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The entity that was added.</returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Adds a collection of entities to the repository asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to add.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The collection of entities that were added.</returns>
    Task<ICollection<TEntity>> AddRangeAsync
    (
        ICollection<TEntity> entities,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Deletes an entity from the repository asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>The entity that was deleted.</returns>
    Task<TEntity> DeleteAsync(TEntity entity);

    /// <summary>
    /// Deletes a collection of entities from the repository asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to delete.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The number of entities that were deleted.</returns>
    Task<int> DeleteRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an entity in the repository asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The entity that was updated.</returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Updates a collection of entities in the repository asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to update.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The collection of entities that were updated.</returns>
    Task<ICollection<TEntity>> UpdateRangeAsync
    (
        ICollection<TEntity> entities,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finds an entity in the repository by its key values asynchronously.
    /// </summary>
    /// <param name="keyValues">The key values of the entity to find.</param>
    /// <returns>The entity that was found, or null if no entity was found.</returns>
    Task<TEntity?> FindAsync(params object[] keyValues);

    /// <summary>
    /// Gets the first entity that matches the specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="include">The navigation properties to include.</param>
    /// <param name="withDeleted">Whether to include deleted entities.</param>
    /// <param name="enableTracking">Whether to enable entity tracking.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The first entity that matches the predicate, or null if no entity was found.</returns>
    Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a single entity that matches the specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="include">The navigation properties to include.</param>
    /// <param name="enableTracking">Whether to enable entity tracking.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The single entity that matches the predicate, or null if no entity was found.</returns>
    Task<TEntity?> GetSingleOrDefaultAsync
    (
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>,
            IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a single entity that matches the specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="include">The navigation properties to include.</param>
    /// <param name="enableTracking">Whether to enable entity tracking.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The single entity that matches the predicate, or null if no entity was found.</returns>
    Task<List<TEntity>> GetListAsync
    (
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a single entity that matches the specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="include">The navigation properties to include.</param>
    /// <param name="enableTracking">Whether to enable entity tracking.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The single entity that matches the predicate, or null if no entity was found.</returns>
    Task<List<IGrouping<TKey, TEntity>>> GetGroupedListAsync<TKey>(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Expression<Func<TEntity, TKey>>? groupBy = null,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
}

