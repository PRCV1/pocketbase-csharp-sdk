namespace pocketbase_csharp_sdk.Services.Base
{
    public abstract class BaseCrudService : BaseService
    {
        private readonly PocketBase _pocketBase;

        protected BaseCrudService(PocketBase pocketBase)
        {
            _pocketBase = pocketBase;
        }

        protected abstract string GetBasePath();

    }
}