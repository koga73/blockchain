using Microsoft.AspNetCore.Mvc;

using Q.API.Models;

namespace Q.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet("[action]")]
        public ApiResponse Heartbeat()
        {
            return new ApiResponse()
            {
                Success = true,
                Data = "OK"
            };
        }

        [HttpPost("[action]")]
        public ApiResponse RegisterUser()
        {
            return new ApiResponse()
            {
                Success = true,
                Data = "OK"
            };
        }
    }
}