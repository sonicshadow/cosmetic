using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class Home
    {
        public List<DisplayViewModel> Display { get; set; }

        public List<Notice> Notice { get; set; }
    }
}