using DataBases.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repositories.Texts;
using System.ComponentModel;

namespace Repositories.Api;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepositoryGame Games { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);

    [Obsolete("Заменить на Begin, Commit, Rollback Transaction. Причина, не возвращает ID", false)]
    Task<T> TransactionAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken cancellationToken = default);
    [Obsolete("Заменить на Begin, Commit, Rollback Transaction. Причина, не возвращает ID", false)]
    Task<T> TransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
    [Obsolete("Заменить на Begin, Commit, Rollback Transaction. Причина, не возвращает ID", false)]
    Task TransactionAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default);
    [Obsolete("Заменить на Begin, Commit, Rollback Transaction. Причина, не возвращает ID", false)]
    Task TransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);


    /// <summary>
    /// ВАЖНО: не вызывать вручную в бизнес-коде.
    /// <para/>
    /// Экземпляр <see cref="IUnitOfWork"/> создаётся и уничтожается контейнером DI (Scoped lifetime) —
    /// ручной вызов <c>Dispose()</c> может преждевременно освободить scope/DbContext и привести
    /// к ошибкам при дальнейшем использовании зависимостей в рамках одного запроса.
    /// </summary>
    [Obsolete("Не вызывать вручную: жизненный цикл управляется DI (Scoped).", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    new void Dispose();

    /// <summary>
    /// ВАЖНО: не вызывать вручную в бизнес-коде.
    /// См. комментарий к <see cref=\"Dispose()\"/>.
    /// </summary>
    [Obsolete("Не вызывать вручную: жизненный цикл управляется DI (Scoped).", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    new ValueTask DisposeAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AsyncServiceScope _scope;
    private readonly DbContextGames _context;
    private readonly ILogger<UnitOfWork> _logger;
    private bool _disposed;

    private IRepositoryGame? _games;


    public UnitOfWork(IServiceScopeFactory serviceScopeFactory, ILogger<UnitOfWork> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceScopeFactory);

        _logger = logger;
        _scope = serviceScopeFactory.CreateAsyncScope();
        _context = _scope.ServiceProvider.GetRequiredService<DbContextGames>();
    }

    public IRepositoryGame Games => _games ??= _scope.ServiceProvider.GetRequiredService<IRepositoryGame>();


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(nameof(transaction));

        await transaction.CommitAsync(cancellationToken);
        await transaction.DisposeAsync();
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(nameof(transaction));

        await transaction.RollbackAsync(cancellationToken);
        await transaction.DisposeAsync();
    }

    public async Task<T> TransactionAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken cancellationToken = default)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        var transaction = await BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await operation(cancellationToken);
            await SaveChangesAsync(cancellationToken);
            await CommitTransactionAsync(transaction, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, UnitOfWorkTexts.Messages.Error.TRANSACTION_ROLLBACK);
            await RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }

    public async Task<T> TransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        var transaction = await BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await operation();
            await SaveChangesAsync(cancellationToken);
            await CommitTransactionAsync(transaction, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, UnitOfWorkTexts.Messages.Error.TRANSACTION_ROLLBACK);
            await RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }

    public async Task TransactionAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        var transaction = await BeginTransactionAsync(cancellationToken);
        try
        {
            await operation(cancellationToken);
            await SaveChangesAsync(cancellationToken);
            await CommitTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, UnitOfWorkTexts.Messages.Error.TRANSACTION_ROLLBACK);
            await RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }

    public async Task TransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        var transaction = await BeginTransactionAsync(cancellationToken);
        try
        {
            await operation();
            await SaveChangesAsync(cancellationToken);
            await CommitTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, UnitOfWorkTexts.Messages.Error.TRANSACTION_ROLLBACK);
            await RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _scope.Dispose();
            }
            _disposed = true;
        }
    }

    /// <summary>
    /// ВАЖНО: не вызывать вручную в бизнес-коде.
    /// Жизненный цикл <see cref="UnitOfWork"/> управляется контейнером DI (Scoped).
    /// </summary>
    [Obsolete("Не вызывать вручную: жизненный цикл управляется DI (Scoped).", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// ВАЖНО: не вызывать вручную в бизнес-коде.
    /// Жизненный цикл <see cref="UnitOfWork"/> управляется контейнером DI (Scoped).
    /// </summary>
    [Obsolete("Не вызывать вручную: жизненный цикл управляется DI (Scoped).", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            await _scope.DisposeAsync();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
