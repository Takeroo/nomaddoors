using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nomaddoors.Models
{
    
    public class VMUser
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please provide your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please provide your surname")]
        public string Surname { get; set; }
        [UniqueEmail]
        [Required(ErrorMessage="Please Provide your Email", AllowEmptyStrings=false)]
        public string Email { get; set; }
        public DateTime Birth { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public int Age { get; set; }
        public string Language { get; set; }
        public string Info { get; set; }

        [Required(ErrorMessage = "Please Provide your Password", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
        public string Whatsapp { get; set; }
        public string Telephone { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }

        public List<VMFestival> going {get; set;}

        public List<VMFestival> guiding { get; set; }

        public HttpPostedFileBase File { get; set; }
    }

}