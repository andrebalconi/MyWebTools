using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTools.Controllers {
    public class ClassSimulator {
        public DateTime dateToday { get; set; }
        public decimal monthlyValue { get; set; }
        public decimal annualValue { get; set; }
        public int yearsOld { get; set; }
        public decimal total { get; set; }


        public int CalculateAge(DateTime birthDate, DateTime Retired) {
            int age = 0;

            age = Retired.Year - birthDate.Year;
            if (Retired.DayOfYear < birthDate.DayOfYear)
                age = age - 1;

            return age;
        }

        public decimal Monetize(decimal valor, DateTime bigDate, decimal perc) {
            decimal money = decimal.Zero;
            int monthRight = 12;

            money = bigDate.Month == monthRight ? valor + valor * Convert.ToDecimal(perc / 100) : valor;

            return money;
        }
    }
}