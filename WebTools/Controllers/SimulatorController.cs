using WebTools.Models;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTools.Controllers;

namespace WebTools.Views {
    public class SimulatorController : Controller {

        public ActionResult Simulator(DataSimulator simulator) {
            ClassSimulator cs = new ClassSimulator();

            Index();
            simulator.age = cs.CalculateAge(simulator.birthDate, simulator.retiredDate);
            cs.dateToday = DateTime.Today;
            cs.annualValue = simulator.annualValue;
            //Evolution of income
            while (cs.dateToday <= simulator.retiredDate) {
                cs.monthlyValue += simulator.monthlyValue;
                cs.monthlyValue = cs.Monetize(cs.monthlyValue, cs.dateToday, Convert.ToInt32(simulator.SelectedPercIds[0]));
                cs.annualValue += cs.dateToday.Month == 12 ? simulator.annualValue : 0;
                cs.annualValue = cs.Monetize(cs.annualValue, cs.dateToday, Convert.ToInt32(simulator.SelectedPercIds[0]));
                cs.dateToday = cs.dateToday.AddMonths(1);

                //Result
                if (cs.annualValue > decimal.Zero) {
                    DataSimulator.annualPayments lista = new DataSimulator.annualPayments();
                    lista.descricao = "Month Pay";
                    lista.monthValue = cs.monthlyValue;
                    lista.dateValue = cs.dateToday;
                }
            }
            cs.total = Math.Round(cs.monthlyValue + cs.annualValue, 2);
            simulator.benefitAmount = Math.Round(cs.total, 2);


            simulator.PercList = GetAllPercTypes();
            if (simulator.SelectedPercIds != null) {
                List<SelectListItem> selectedItems = simulator.PercList.Where(p => simulator.SelectedPercIds.Contains(int.Parse(p.Value))).ToList();
                foreach (var Perc in selectedItems) {
                    Perc.Selected = true;
                    ViewBag.Message += Perc.Text + " | ";
                }
            }

            return View(simulator);

        }

        public ActionResult Index() {
            var model = new DataSimulator {
                SelectedPercIds = new[] { 2 },
                PercList = GetAllPercTypes()
            };
            return View(model);
        }

        public List<SelectListItem> GetAllPercTypes() {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "1%", Value = "1" });
            items.Add(new SelectListItem { Text = "2%", Value = "2" });
            items.Add(new SelectListItem { Text = "3%", Value = "3" });
            items.Add(new SelectListItem { Text = "4%", Value = "4" });
            return items;
        }
    }
}