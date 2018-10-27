using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nomaddoors.Controllers
{
    public class ImagesController : ApiController
    {
        
        DB_A095E6_nomaddoorsEntities nomadDB = new DB_A095E6_nomaddoorsEntities();
        public IEnumerable<Image> Get()
        {
                
            return nomadDB.Images.ToList();
        }
        public Image Get(int id)
        {


            return nomadDB.Images.Find(id);
        }

        // GET api/festivals/5
        
    }
}
