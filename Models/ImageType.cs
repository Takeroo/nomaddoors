using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nomaddoors.Models
{
    public class ImageType
    {

            [Required(ErrorMessage = "Please select an image file")]
            public HttpPostedFileBase File { get; set; }
    }
}