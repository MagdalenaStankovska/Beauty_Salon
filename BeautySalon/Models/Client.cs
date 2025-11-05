using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.Models
{
	public class Client
	{

            [Key]
            public int ClientId { get; set; }

            [Required]
            [Display(Name = "Client Name")]
            public string ClientName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }  // Keep email here too

            [Required]
            [Phone]
            [Display(Name = "Mobile Phone")]
            public string MobilePhone { get; set; }

            public virtual List<Appoitments> appoitments { get; set; }



    }
}