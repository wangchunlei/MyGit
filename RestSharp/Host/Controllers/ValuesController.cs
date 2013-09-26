﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domas.DAP.ADF.LogManager;

namespace Host.Controllers
{
    public class ValuesController : ApiController
    {
        private ILogger logger = LogManager.GetLogger("test");
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {

            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            logger.Debug("begin:" + value);
            System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
            logger.Debug("end:" + value);
            //logger.Debug(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}