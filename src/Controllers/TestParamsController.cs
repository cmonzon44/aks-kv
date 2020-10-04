using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KubernetKV.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KubernetKV.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestParamsController : ControllerBase
    {
        IConfiguration configuration;

        public TestParamsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // GET: api/TestParams
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TestParams/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // GET: api/TestParams/GetKeyvault
        [HttpGet("GetKeyvault")]
        public JsonResult GetKeyvault()
        {
            var url = this.configuration.GetSection("KeyVault");

            return new JsonResult(url);
        }

        // GET: api/TestParams/GetConfigurations
        [HttpGet("GetConfigurations")]
        public JsonResult GetConfigurations()
        {
            var r = this.configuration.AsEnumerable();

            return new JsonResult(r);
        }

        [HttpGet("GetEnv")]
        public JsonResult GetEnv()
        {
            var strings = new List<string>();
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                strings.Add(string.Format("Key: {0} , valor: {1} ", de.Key, de.Value));

            return new JsonResult(strings);
        }


        // GET: api/TestParams/GetKeyvault
        [HttpGet("Keyvault/values")]
        public async Task<JsonResult> GetKeyvaultKeys()
        {
            var url = this.configuration["KeyVault:Url"];
            var keys = this.configuration.GetSection("KeyVault:Keys").Get<List<string>>();

            KVUtils KVutils = new KVUtils();

            var ret = new Dictionary<string, string>();
            foreach (var ky in keys)
            {
                var value = await KVutils.GetValuesFromKV(url, ky);
                ret.Add(ky, value);

            }
            

            return new JsonResult(ret);
        }
    }
}
