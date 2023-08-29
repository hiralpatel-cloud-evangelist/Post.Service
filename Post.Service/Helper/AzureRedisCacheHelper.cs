using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Post.Service.Helper
{
    public class AzureRedisCacheHelper
    {

        private readonly StackExchange.Redis.IDatabase _connection;

        public AzureRedisCacheHelper(StackExchange.Redis.IDatabase connection)
        {
            _connection = connection;
        }

        public async virtual Task<T> Get<T>(string cacheKey)
        {
            return Deserialize<T>(await _connection.StringGetAsync(cacheKey));
        }
        public async Task<object> Get(string cacheKey)
        {
            return Deserialize<object>(await _connection.StringGetAsync(cacheKey));
        }

        public async Task<List<T>> GetList<T>(string cacheKey)
        {
            var genericList = await Get(cacheKey);
            if (genericList != null)
            {
                var jArray = JArray.FromObject(genericList);
                return jArray.ToObject<List<T>>();
            }
            return null;
        }
        public async Task<string> SetStringList<T>(string cacheKey, List<T> list)
        {
            return await Set(cacheKey, list);
        }
        public async Task<string> Set(string cacheKey, object cacheValue)
        {
            await _connection.StringSetAsync(cacheKey, Serialize(cacheValue));
            return null;
        }

        private static string Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(obj);
        }

        private T Deserialize<T>(string jsonstring)
        {
            if (String.IsNullOrEmpty(jsonstring))
                return default(T);

            var x = JsonConvert.DeserializeObject<T>(jsonstring);
            return x;
        }

        public async virtual Task<bool> IsInCache(string key)
        {
            return await _connection.KeyExistsAsync(key);
        }

        public async Task<bool> RemoveCache(string key)
        {
            return await _connection.KeyDeleteAsync(key);
        }

    }
}
