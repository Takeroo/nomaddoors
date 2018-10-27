using Nomaddoors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nomaddoors.Controllers
{
    public class FestivalsController : ApiController
    {
        DB_A095E6_nomaddoorsEntities nomadDB = new DB_A095E6_nomaddoorsEntities();
        public IEnumerable<Festival> Get()
        {
                
            return nomadDB.Festivals.ToList();
        }

        // GET api/festivals/5
        public Festival Get(int id)
        {
            
            return nomadDB.Festivals.Find(id);
        }

        public void Post([FromBody]Festival fest)
        {

            nomadDB.Festivals.Add(fest);
            nomadDB.SaveChanges();
        }

        public void Delete(int id)
        {

            nomadDB.Festivals.Remove(nomadDB.Festivals.Find(id));
            nomadDB.SaveChanges();
        }

        public void Put(int id, [FromBody]Festival fest)
        {

            var f = nomadDB.Festivals.Find(id);
            if (fest.Name != null) f.Name = fest.Name;
            if (fest.Short != null) f.Short = fest.Short;
            if (fest.About != null) f.About = fest.About;
            if (fest.Organizator != null) f.Organizator = fest.Organizator;
            if (fest.Guide != null) f.Guide = fest.Guide;

            if (fest.Price != null) f.Price = fest.Price;
            if (fest.Region != null) f.Region = fest.Region;
            if (fest.Province != null) f.Province = fest.Province;
            if (fest.City != null) f.City = fest.City;
            if (fest.Address != null) f.Address = fest.Address;

            if (fest.Date != null) f.Date = fest.Date;
            if (fest.Date1 != null) f.Date1 = fest.Date1;
            if (fest.Date2 != null) f.Date2 = fest.Date2;
            if (fest.Image != null) f.Image = fest.Image;

            nomadDB.SaveChanges();
        }
    }
}
