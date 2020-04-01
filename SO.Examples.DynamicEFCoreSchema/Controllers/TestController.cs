using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SO.Examples.DynamicEFCoreSchema.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private ILogger<TestController> Logger { get; }
        private TestContext TestContext { get; }

        public TestController(ILogger<TestController> logger, TestContext context)
        {
            this.Logger = logger;
            this.TestContext = context;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            this.Logger.LogDebug($"Default schema is {this.TestContext.Model.GetDefaultSchema()}");

            return this.TestContext.Customer.ToList();
        }
    }
}
