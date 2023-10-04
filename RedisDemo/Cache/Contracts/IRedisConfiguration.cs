namespace RedisDemo.Cache.Contracts
{
    public interface IRedisCacheConfiguration
    {
        public string Endpoint { get; }
        public int? Database { get; }
    }
}
