namespace RedisDemo.Cache.Contracts
{
    public interface ICacheService
    {
        bool Add<T>(string key, T value);

        bool Update<T>(string key, T value);

        T Get<T>(string key);

        IEnumerable<T> Get<T>(IEnumerable<string> keys);

        bool Remove(string key);

        bool HasKey(string key);

        public IEnumerable<T> GetAll<T>(string seacrhPhrase);
    }
}
