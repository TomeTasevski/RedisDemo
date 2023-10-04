using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisDemo.Cache.Contracts;

namespace RedisDemo.Controllers
{
    [ApiController]
    [Route("Users")]
    public class RedisController : ControllerBase
    {
        private static readonly string _prefix = "Users:";

        private readonly ILogger<RedisController> _logger;
        private readonly ICacheService _cacheService;


        public RedisController(ILogger<RedisController> logger, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            var user = _cacheService.Get<User>($"{_prefix}{id}");
            return Ok(user);
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_cacheService.GetAll<User>(_prefix));
        }

        [HttpPost]
        public ActionResult Create([FromQuery] string name, [FromQuery] string email)
        {
            var id = Guid.NewGuid();

            var user = new User()
            {
                Id = id,
                Email = email,
                Name = name
            };

            _cacheService.Add($"{_prefix}{id}", user);

            return Ok(_cacheService.Get<User>($"{_prefix}{id}"));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            _cacheService.Remove($"{_prefix}{id}");
            return Ok();
        }
    }
}