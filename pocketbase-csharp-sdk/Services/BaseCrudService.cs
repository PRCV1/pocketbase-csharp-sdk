using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseCrudService<T> : BaseService
    {
        private readonly PocketBase client;
        

        public BaseCrudService(PocketBase client)
        {
            this.client = client;
        }

        //public async Task<PagedCollectionModel<T>> ListAsync(
        //    int? page = null,
        //    int? perPage = null,
        //    string? sort = null,
        //    string? filter = null,
        //    string? expand = null,
        //    IDictionary<string, string>? headers = null)
        //{
        //    var query = new Dictionary<string, object?>()
        //    {
        //        { "filter", filter },
        //        { "page", page },
        //        { "perPage", perPage },
        //        { "sort", sort },
        //        { "expand", expand },
        //    };
        //    var pagedCollection = await client.SendAsync<PagedCollectionModel<T>>(
        //        BasePath(),
        //        HttpMethod.Get,
        //        headers: headers,
        //        query: query);
        //    if (pagedCollection is null) throw new ClientException(BasePath());

        //    return pagedCollection;
        //}

        //public async Task<T> CreateAsync(
        //    IDictionary<string, object>? body)
        //{
        //    var ret = await client.SendAsync<T>(
        //        BasePath(),
        //        HttpMethod.Post,
        //        body: body);
        //    if (ret is null) throw new ClientException(BasePath());

        //    return ret;
        //}

        public async Task<T> UpdateAsync(
            string id,
            T item,
            string? expand = null,
            IDictionary<string, string>? headers = null)
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            //var url = this.BasePath(sub) + "/" + HttpUtility.UrlEncode(id);
            var pagedCollection = await client.SendAsync<T>(
                BasePath(),
                HttpMethod.Patch,
                body: body,
                headers: headers,
                query: query);
            if (pagedCollection is null) throw new ClientException(BasePath());

            return pagedCollection;
        }

    }
}
