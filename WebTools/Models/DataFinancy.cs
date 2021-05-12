using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTools.Models
{
    public class DataFinancy
    {
        [Required(ErrorMessage = "Required field!")]
        public decimal grossValue { get; set; }
        [Required(ErrorMessage = "Required field!")]
        public decimal percentual { get; set; }
        [Required(ErrorMessage = "Required field!")]
        public int portion { get; set; }
        public decimal totalValue { get; set; }
        public decimal totalFees { get; set; }
        public decimal valuePortion { get; set; }
        [Required(ErrorMessage = "Required field!")]
        public decimal salary { get; set; }
        public string message { get; set; }

        public int[] SelectedPercIds { get; set; }
        public IEnumerable<SelectListItem> PercList { get; set; }
        public int[] SelectedPortionIds { get; set; }
        public IEnumerable<SelectListItem> PortionList { get; set; }
        public DateTime concessao { get; set; }
        public List<object> Result { get; set; }
    }
}