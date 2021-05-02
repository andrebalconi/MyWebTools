using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTools.Models
{
    public class DataSimulator
    {
        [Required(ErrorMessage = "Required field!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required field!")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:HH:mm dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime birthDate { get; set; }
        [Required(ErrorMessage = "Required field!")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:HH:mm dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime retiredDate { get; set; }
        [Required(ErrorMessage = "Required field!")]
        
        public decimal monthlyValue { get; set; }
        [Required(ErrorMessage = "Required field!")]
                
        public decimal annualValue { get; set; }
        [Required(ErrorMessage = "Required field!")]
        public int annualProfitability { get; set; }
        public decimal benefitAmount { get; set; }
        public class annualPayments
        { 
            public string descricao { get; set; }
            public string monthYear { get; set; }
            public decimal monthValue { get; set; }
            public DateTime dateValue { get; set; }
            
        }

        public List<DropDownList> listPerc()
        {
            return new List<DropDownList>
            {
                new DropDownList {percentual = 1},
                new DropDownList {percentual = 2},
                new DropDownList {percentual = 3}
            };

        }

        public int[] SelectedPercIds { get; set; }
        public IEnumerable<SelectListItem> PercList { get; set; }

    }

}