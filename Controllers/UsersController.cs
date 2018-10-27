using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nomaddoors.Controllers
{
    public class UsersController : ApiController
    {
        
        DB_A095E6_nomaddoorsEntities nomadDB = new DB_A095E6_nomaddoorsEntities();
        public IEnumerable<User> Get()
        {
                
            return nomadDB.Users.ToList();
        }
        public User Get(int id)
        {


            return nomadDB.Users.Find(id);
        }

        // GET api/festivals/5
        
    }
}
