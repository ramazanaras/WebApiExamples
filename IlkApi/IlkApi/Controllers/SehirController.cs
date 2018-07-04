using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IlkApi.Controllers
{

    [EnableCors("*","*","*")]
    public class SehirController : ApiController
    {
        private string[] sehirler = new string[] { "Antalya", "Eskişehir", "İstanbul" };

        //domainadresi/api/sehir
        //http://localhost:33697/api/sehir
        public string[] Get()
        {
            return sehirler;
        }


        //http://localhost:33697/api/sehir/2
        public string Get(int id)
        {

            return sehirler[id];
        }


    }
}
