using WebTools.Models;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTools.Views
{
    public class SimulatorController : Controller
    {
        public DateTime dateToday;
        public decimal monthlyValue;
        public decimal annualValue;
        public int yearsOld;
        public decimal total;
        
        // GET: Simulator
        public ActionResult Simulator(DataSimulator simulator)
        {
            dateToday = DateTime.Today;
            annualValue = simulator.annualValue;
            //Evolution of income
            while (dateToday <= simulator.retiredDate)
            {
                monthlyValue += simulator.monthlyValue;
                monthlyValue = Monetize(monthlyValue, dateToday, simulator.annualProfitability);
                annualValue = Monetize(annualValue, dateToday, simulator.annualProfitability);
                dateToday = dateToday.AddMonths(1);

                //Result
                if (annualValue > decimal.Zero)
                {
                    DataSimulator.annualPayments lista = new DataSimulator.annualPayments();
                    lista.descricao = "Month Pay";
                    lista.monthValue = monthlyValue;
                    lista.dateValue = dateToday;
                }
            }
            total = Math.Round(monthlyValue + annualValue, 2);
            simulator.benefitAmount = Math.Round(total, 2);
                        
            return View(simulator);
        }

        private static int CalculateAge(DateTime birthDate) 
        {
            int age = 0;

            age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                age = age - 1;
        
            return age;
        }

        private static decimal Monetize(decimal valor, DateTime bigDate , decimal perc) 
        {
            decimal money = decimal.Zero;
            int monthRight = 12;

            money = bigDate.Month == monthRight ? valor + valor * Convert.ToDecimal(perc/100) : valor;
               
            return money;
        }
    }
}