using RedisDemo.Cache.Contracts;

namespace RedisDemo.Cache
{
    public class RedisCacheConfiguration : IRedisCacheConfiguration
    {
        public readonly IConfiguration _configuration;

        public RedisCacheConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Endpoint => _configuration["RedisCacheConfiguration:Endpoint"];


        public int? Database => Convert.ToInt32(_configuration["RedisCacheConfiguration:Database"]);

    }
}
