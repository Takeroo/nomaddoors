using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nomaddoors.Controllers
{
    public class JoinsController : ApiController
    {
        
        DB_A095E6_nomaddoorsEntities nomadDB = new DB_A095E6_nomaddoorsEntities();
        public IEnumerable<Join> Get()
        {
                
            return nomadDB.Joins.ToList();
        }

        // GET api/festivals/5
        
    }
}
