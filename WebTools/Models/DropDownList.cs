using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTools.Models
{
    public class DropDownList
    {
        public int percentual { get; set; }

        public List<DropDownList> listPerc() 
        {
            return new List<DropDownList>
            {
                new DropDownList {percentual = 1},
                new DropDownList {percentual = 2},
                new DropDownList {percentual = 3}
            };
            
        }
    }
}