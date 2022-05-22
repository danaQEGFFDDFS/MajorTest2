using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProgressCheck.Models
{
    public class Fibbo
    {
        [Required(ErrorMessage = "Укажите число")]
        public int f { get; set; }
            }
}
