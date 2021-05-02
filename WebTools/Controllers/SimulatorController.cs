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
            Index();
            simulator.age = CalculateAge(simulator.birthDate, simulator.retiredDate);
            dateToday = DateTime.Today;
            annualValue = simulator.annualValue;
            //Evolution of income
            while (dateToday <= simulator.retiredDate)
            {
                monthlyValue += simulator.monthlyValue;
                monthlyValue = Monetize(monthlyValue, dateToday, Convert.ToInt32(simulator.SelectedPercIds[0]));
                annualValue = Monetize(annualValue, dateToday, Convert.ToInt32(simulator.SelectedPercIds[0]));
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


            simulator.PercList = GetAllPercTypes();
            if (simulator.SelectedPercIds != null)
            {
                List<SelectListItem> selectedItems = simulator.PercList.Where(p => simulator.SelectedPercIds.Contains(int.Parse(p.Value))).ToList();
                foreach (var Perc in selectedItems)
                {
                    Perc.Selected = true;
                    ViewBag.Message += Perc.Text + " | ";
                }
            }

            return View(simulator);
              
        }

        private static int CalculateAge(DateTime birthDate, DateTime Retired) 
        {
            int age = 0;

            age = Retired.Year - birthDate.Year;
            if (Retired.DayOfYear < birthDate.DayOfYear)
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

        public ActionResult Index()
        {
            var model = new DataSimulator
            {
                SelectedPercIds = new[] { 2 },
                PercList = GetAllPercTypes()
            };
            return View(model);
        }

        public List<SelectListItem> GetAllPercTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "1%", Value = "1" });
            items.Add(new SelectListItem { Text = "2%", Value = "2" });
            items.Add(new SelectListItem { Text = "3%", Value = "3" });
            items.Add(new SelectListItem { Text = "4%", Value = "4" });
            return items;
        }
    }
}