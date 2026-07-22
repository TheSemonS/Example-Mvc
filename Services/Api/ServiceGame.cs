using Microsoft.Extensions.Logging;
using Repositories.Api;
using Services.Models;
using Services.Texts;

namespace Services.Api
{
    public interface IServiceGame
    {
        Task<GameDisplayCreateResponse> DisplayCreatePageAsync(CancellationToken token = default);
        Task<GameDisplayUpdateResponse> DisplayUpdatePageAsync(GameDisplayUpdateRequest request, CancellationToken token = default);
        Task<GameDisplayDeleteResponse> DisplayDeletePageAsync(GameDisplayDeleteRequest request, CancellationToken token = default);

        Task CreateAsync(GameCreateRequest request, CancellationToken token = default);
        Task UpdateAsync(GameUpdateRequest request, CancellationToken token = default);
        Task DeleteAsync(GameDeleteRequest request, CancellationToken token = default);

        Task<GameGetByResponse> GetByAsync(GameGetByRequest request, CancellationToken token = default);
        Task<GameGetAllResponse> GetAllAsync(CancellationToken token = default);
    }

    public class ServiceGame : IServiceGame
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ServiceGame> logger;

        public ServiceGame(IUnitOfWork unitOfWork, ILogger<ServiceGame> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task CreateAsync(GameCreateRequest request, CancellationToken token = default)
        {
            logger.LogInformation(GameTexts.Messages.Start.CREATING);

            if (request == null)
            {
                logger.LogWarning(GameTexts.Messages.Validation.REQUAST_NULL);
                throw new Exception(GameTexts.Messages.Validation.REQUAST_NULL);
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                logger.LogWarning(GameTexts.Messages.Validation.NAME_EMPTY);
                throw new Exception(GameTexts.Messages.Validation.NAME_EMPTY);
            }
            if (request.Name.Length > 50)
            {
                logger.LogWarning(GameTexts.Messages.Validation.NAME_EXCEEDS_LIMIT);
                throw new Exception(GameTexts.Messages.Validation.NAME_EXCEEDS_LIMIT);
            }
            
            var transaction = await unitOfWork.BeginTransactionAsync(token);

            try
            {
                var model = unitOfWork.Games.GetNew();
                model.Name = request.Name;
                var result = await unitOfWork.Games.CreateAsync(model, token);
                await unitOfWork.SaveChangesAsync(token);
                await unitOfWork.CommitTransactionAsync(transaction, token);
                logger.LogInformation(GameTexts.Messages.Succses.CREATING_COMPLETED);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, GameTexts.Messages.Error.CREATING_ERROR);
                await unitOfWork.RollbackTransactionAsync(transaction, token);
                throw;            
            }
        }

        public async Task UpdateAsync(GameUpdateRequest request, CancellationToken token = default)
        {
            logger.LogInformation(GameTexts.Messages.Start.UPDATING);

            if (request == null)
            {
                logger.LogWarning(GameTexts.Messages.Validation.REQUAST_NULL);
                throw new Exception(GameTexts.Messages.Validation.REQUAST_NULL);
            }
            if (request.Id <= 0)
            {
                logger.LogWarning(GameTexts.Messages.Validation.ID_NOT_VALID);
                throw new Exception(GameTexts.Messages.Validation.ID_NOT_VALID);
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                logger.LogWarning(GameTexts.Messages.Validation.NAME_EMPTY);
                throw new Exception(GameTexts.Messages.Validation.NAME_EMPTY);
            }
            if (request.Name.Length > 50)
            {
                logger.LogWarning(GameTexts.Messages.Validation.NAME_EXCEEDS_LIMIT);
                throw new Exception(GameTexts.Messages.Validation.NAME_EXCEEDS_LIMIT);
            }

            var transaction = await unitOfWork.BeginTransactionAsync(token);

            try
            {
                var original = await unitOfWork.Games.GetSignalOrDefaultAsync(request.Id, token);

                if (original == default)
                {
                    logger.LogWarning(string.Format(GameTexts.Messages.Validation.NOT_FOUND_BY_ID, request.Id));
                    throw new Exception(string.Format(GameTexts.Messages.Validation.NOT_FOUND_BY_ID, request.Id));
                }

                original.Name = request.Name;

                var resault = await unitOfWork.Games.UpdateAsync(original, token);
                await unitOfWork.SaveChangesAsync(token);
                await unitOfWork.CommitTransactionAsync(transaction, token);
                logger.LogInformation(GameTexts.Messages.Succses.UPDATING_COMPLETED);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, GameTexts.Messages.Error.UPDATING_ERROR);
                await unitOfWork.RollbackTransactionAsync(transaction, token);
                throw;
            }
        }

        public async Task DeleteAsync(GameDeleteRequest request, CancellationToken token = default)
        {
            logger.LogInformation(GameTexts.Messages.Start.DELETING);

            if (request == null)
            {
                logger.LogWarning(GameTexts.Messages.Validation.REQUAST_NULL);
                throw new Exception(GameTexts.Messages.Validation.REQUAST_NULL);
            }
            if (request.Id <= 0)
            {
                logger.LogWarning(GameTexts.Messages.Validation.ID_NOT_VALID);
                throw new Exception(GameTexts.Messages.Validation.ID_NOT_VALID);
            }

            var transaction = await unitOfWork.BeginTransactionAsync(token);

            try
            {
                var original = await unitOfWork.Games.GetSignalOrDefaultAsync(request.Id, token);

                if (original == default)
                {
                    logger.LogWarning(string.Format(GameTexts.Messages.Validation.NOT_FOUND_BY_ID, request.Id));
                    throw new Exception(string.Format(GameTexts.Messages.Validation.NOT_FOUND_BY_ID, request.Id));
                }

                await unitOfWork.Games.DeleteAsync(original, token);
                await unitOfWork.SaveChangesAsync(token);
                await unitOfWork.CommitTransactionAsync(transaction, token);
                logger.LogInformation(GameTexts.Messages.Succses.DELETING_COMPLETED);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, GameTexts.Messages.Error.DELETING_ERROR);
                await unitOfWork.RollbackTransactionAsync(transaction, token);
                throw;
            }
        }

        public async Task<GameGetByResponse> GetByAsync(GameGetByRequest request, CancellationToken token = default)
        {
            logger.LogInformation(GameTexts.Messages.Start.GETBY);

            if (request == null)
            {
                logger.LogWarning(GameTexts.Messages.Validation.REQUAST_NULL);
                throw new Exception(GameTexts.Messages.Validation.REQUAST_NULL);
            }
            if (request.Id <= 0)
            {
                logger.LogWarning(GameTexts.Messages.Validation.ID_NOT_VALID);
                throw new Exception(GameTexts.Messages.Validation.ID_NOT_VALID);
            }

            try
            {
                var original = await unitOfWork.Games.GetSignalOrDefaultAsync(request.Id, token);

                if (original == default)
                {
                    logger.LogWarning(string.Format(GameTexts.Messages.Validation.NOT_FOUND_BY_ID, request.Id));
                    throw new Exception(string.Format(GameTexts.Messages.Validation.NOT_FOUND_BY_ID, request.Id));
                }

                var resault = new GameGetByResponse
                {
                    Id = original.Id,
                    Name = original.Name
                };

                logger.LogInformation(GameTexts.Messages.Succses.GETBY_COMPLETED);
                return resault;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, GameTexts.Messages.Error.GETBY_ERROR);
                throw;
            }
        }

        public async Task<GameGetAllResponse> GetAllAsync(CancellationToken token = default)
        {
            logger.LogInformation(GameTexts.Messages.Start.GETALL);

            try
            {
                var originals = await unitOfWork.Games.GetListAsync(token);

                var games = originals.Select(p => new GameGetAllModel
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList();

                var result = new GameGetAllResponse
                {
                    Games = games
                };

                logger.LogInformation(GameTexts.Messages.Succses.GETALL_COMPLETED);
                return result;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, GameTexts.Messages.Error.GETALL_ERROR);
                throw;
            }
        }

        public async Task<GameDisplayCreateResponse> DisplayCreatePageAsync(CancellationToken token = default)
        {
            logger.LogInformation(GameTexts.Messages.Start.DISPLAY_CREATING);

            try
            {
                var original = unitOfWork.Games.GetNew();
                var response = new GameDisplayCreateResponse { Name = original.Name };
                logger.LogInformation(GameTexts.Messages.Succses.DISPLAY_CREATING_COMPLETED);
                return await Task.FromResult(response);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, GameTexts.Messages.Error.DISPLAY_CREATING_ERROR);

                throw;
            }


        }

        public async Task<GameDisplayUpdateResponse> DisplayUpdatePageAsync(GameDisplayUpdateRequest request, CancellationToken token = default)
        {
            logger.LogInformation(GameTexts.Messages.Start.DISPLAY_UPDATING);


            if (request == null)
            {
                logger.LogWarning(GameTexts.Messages.Validation.REQUAST_NULL);
                throw new Exception(GameTexts.Messages.Validation.REQUAST_NULL);
            }
            if (request.Id <= 0)
            {
                logger.LogWarning(GameTexts.Messages.Validation.ID_NOT_VALID);
                throw new Exception(GameTexts.Messages.Validation.ID_NOT_VALID);
            }
            try
            {
                var original = await unitOfWork.Games.GetSignalOrDefaultAsync(request.Id, token);
                if (original == default)
                {
                    logger.LogWarning(string.Format(GameTexts.Messages.Validation.NOT_FOUND_BY_ID, request.Id));
                    throw new Exception(string.Format(GameTexts.Messages.Validation.NOT_FOUND_BY_ID, request.Id));
                }
                var response = new GameDisplayUpdateResponse 
                { 
                    Id = request.Id,
                    Name = original.Name 
                };
                logger.LogInformation(GameTexts.Messages.Succses.DISPLAY_UPDATING_COMPLETED);
                return response;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, GameTexts.Messages.Error.DISPLAY_UPDATING_ERROR);
                throw;
            }
        }

        public async Task<GameDisplayDeleteResponse> DisplayDeletePageAsync(GameDisplayDeleteRequest request, CancellationToken token = default)
        {
            logger.LogInformation(GameTexts.Messages.Start.DISPLAY_DELETING);

            if (request == null)
            {
                logger.LogWarning(GameTexts.Messages.Validation.REQUAST_NULL);
                throw new Exception(GameTexts.Messages.Validation.REQUAST_NULL);
            }
            if (request.Id <= 0)
            {
                logger.LogWarning(GameTexts.Messages.Validation.ID_NOT_VALID);
                throw new Exception(GameTexts.Messages.Validation.ID_NOT_VALID);
            }
            try
            {
                var original = await unitOfWork.Games.GetSignalOrDefaultAsync(request.Id, token);
                if (original == default)
                {

                    throw new Exception(string.Format(GameTexts.Messages.Validation.NOT_FOUND_BY_ID, request.Id));
                }
                var response = new GameDisplayDeleteResponse 
                { 
                    Id = original.Id,
                    Name = original.Name
                };
                logger.LogInformation(GameTexts.Messages.Succses.DISPLAY_DELETING_COMPLETED);
                return response;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, GameTexts.Messages.Error.DISPLAY_DELETING_ERROR);
                throw;
            }
        }
    }
}
