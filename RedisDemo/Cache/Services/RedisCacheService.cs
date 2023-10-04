using Newtonsoft.Json;
using RedisDemo.Cache.Contracts;
using StackExchange.Redis;
using System.Net;

namespace RedisDemo.Cache.Services
{

    public class RedisCache : ICacheService
    {
        private static int _db;
        private static Lazy<ConnectionMultiplexer> _lazyConnection;
        private static ConnectionMultiplexer Connection => _lazyConnection.Value;
        private static IDatabase Db => Connection.GetDatabase(_db);

        public RedisCache(IRedisCacheConfiguration _redisCacheConfiguration)
        {
            _db = _redisCacheConfiguration.Database ?? -1;
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_redisCacheConfiguration.Endpoint));
        }
        public bool Add<T>(string key, T value)
        {
            return Db.StringSet(key, Serialize(value));
        }

        public T Get<T>(string key)
        {
            string val = Db.StringGet(key);
            return !string.IsNullOrEmpty(val) ? Deserialize<T>(val) : default(T);
        }

        public IEnumerable<T> Get<T>(IEnumerable<string> keys)
        {
            var result = new List<T>();
            foreach (var key in keys)
            {
                string val = Db.StringGet(key);
                if (!string.IsNullOrEmpty(val))
                {
                    result.Add(Deserialize<T>(val));
                }
            }

            return result;
        }

        public IEnumerable<T> GetAll<T>(string seacrhPhrase)
        {
            var result = new List<T>();

            EndPoint endPoint = Connection.GetEndPoints().First();
            RedisKey[] keys = Connection.GetServer(endPoint).Keys(pattern: seacrhPhrase + "*").ToArray();
            var val = Db.StringGet(keys);

            if (val.Count() > 0)
            {
                result = val.Select(d => Deserialize<T>(d)).ToList();
            }

            return result;
        }

        public bool HasKey(string key)
        {
            return Db.KeyExists(key);
        }

        public bool Remove(string key)
        {
            return Db.KeyDelete(key);
        }

        public bool Update<T>(string key, T value)
        {
            var isDeleted = Remove(key);
            return isDeleted && Add(key, value);
        }

        private static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        private static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}

