using FluentResults;
using pocketbase_csharp_sdk.Models;

namespace pocketbase_csharp_sdk.Extensions
{
    public static class PocketBaseExtensions
    {
        
        public static Result<UserModel> GetCurrentUser(this PocketBase pocketBase)
        {
            if (pocketBase.AuthStore.Model is null || string.IsNullOrWhiteSpace(pocketBase.AuthStore.Model.Id))
            {
                return Result.Fail("AuthStore.Model is null or AuthStore.Model.Id is null");
            }

            return pocketBase.User.GetOne(pocketBase.AuthStore.Model.Id);
        }
        
        public static Task<Result<UserModel>> GetCurrentUserAsync(this PocketBase pocketBase)
        {
            if (pocketBase.AuthStore.Model is null || string.IsNullOrWhiteSpace(pocketBase.AuthStore.Model.Id))
            {
                var fail = Result.Fail($"AuthStore.Model is null or AuthStore.Model.Id is null");
                return Task.FromResult<Result<UserModel>>(fail);
            }

            return pocketBase.User.GetOneAsync(pocketBase.AuthStore.Model.Id);
        }
    }
}