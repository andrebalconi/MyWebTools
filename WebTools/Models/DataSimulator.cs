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
        [Range(10, 1000000, ErrorMessage = "Value between 10 and 1.000.000")]
        public decimal monthlyValue { get; set; }
        [Required(ErrorMessage = "Required field!")]
        [Range(10, 1000000, ErrorMessage = "Value between 10 and 1.000.000")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal annualValue { get; set; }
        [Required(ErrorMessage = "Required field!")]
        public int annualProfitability { get; set; }

    }
}