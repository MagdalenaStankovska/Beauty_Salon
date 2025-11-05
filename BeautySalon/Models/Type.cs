using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySalon.Models
{
	public class Type
	{
       
            [Key]
            public int TypeID { get; set; }

            [Required]
            public string tipche { get; set; }

            public List<Appoitments> appoitments { get; set; }
        
    }
}