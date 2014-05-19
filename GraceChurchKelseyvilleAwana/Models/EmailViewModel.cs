using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GraceChurchKelseyvilleAwana.Models
{
    public class EmailViewModel
    {
        [Display (Name = "Message")]
        public string EmailBody { get; set; }

        [Display (Name = "Recipients")]
        public string Recipients { get; set; }

        public bool Confirm { get; set; }
    }
}