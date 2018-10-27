using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nomaddoors.Models
{
    public class VMFestival
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Short { get; set; }
        public string About { get; set; }
        public int? Price { get; set; }

        public string Date { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public string Region { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Address { get; set; }
        public string Organizator { get; set; }
        public int? Guide { get; set; }

        public VMUser gid { get; set; }

        public string Img { get; set; }

        public Image Program { get; set; }

        public List<Subtitle> subtitles { get; set; }
        public List<Program> programs { get; set; }
        public List<Image> Images { get; set; }
        public List<VMUser> joins { get; set; }

        [Required(ErrorMessage = "Please select an image files")]

        public HttpPostedFileBase File { get; set; }
    }
}