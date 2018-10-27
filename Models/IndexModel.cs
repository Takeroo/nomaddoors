using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nomaddoors.Models
{
    public class IndexModel
    {
        public List<VMFestival> upcoming { get; set; }
        public List<VMFestival> past { get; set; }
    }
}