using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.Models
{
	public class Appoitments
	{

            [Key]
            public int AppoitmentID { get; set; }

        

            [Required]
            public DateTime date { get; set; }

           

            public Type tipche { get; set; }

            public int TypeID { get; set; }


            [Required]
            public string ClientName { get; set; }

            public string Status { get; set; } = "Pending";

    }
}