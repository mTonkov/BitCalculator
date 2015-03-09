using BitCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BitCalculator.Controllers
{
    public class CalculatorController : Controller
    {
        private IList<string> measures;

        public CalculatorController()
        {
            measures = new List<string> { "Bit", "Byte", "KiloBit", "KiloByte", "MegaBit", "MegaByte", 
        "GigaBit", "GigaByte", "TeraBit", "TeraByte","PetaBit", "PetaByte", "ExaBit", "ExaByte", "ZettaBit",
        "ZettaByte", "YottaBit", "YottaByte"};
        }

        // GET: Calculator
        [HttpGet]
        public ActionResult Index()
        {
            var list = this.measures
                .Select(k =>
                    new SelectListItem
                    {
                        Text = k,
                        Value = k
                    });

            ViewBag.MeasuresDropDown = list;

            var model = new CalculatorViewModel() { Quantity = 1, Kilo = 1024, Measure = this.measures[6] };

            var measuresValues = GetCalculatedData(model);
            return View(measuresValues);
        }

        [HttpPost]
        public ActionResult Index(CalculatorViewModel model)
        {
            var isAjax = Request.IsAjaxRequest();
            var measuresValues = GetCalculatedData(model);

            return View("_CalculationResults", measuresValues);
        }

        private Dictionary<string, string> GetCalculatedData(CalculatorViewModel model)
        {
            var values = CalculateValues(model);
            var measuresValues = new Dictionary<string, string>();

            for (int i = 0; i < values.Count(); i++)
            {
                var value = values[i].ToString().Length > 15 ?
                            string.Format("{0:e}", values[i]) :
                            values[i].ToString();

                measuresValues.Add(this.measures[i], value);
            }

            return measuresValues;
        }

        private IList<decimal> CalculateValues(CalculatorViewModel model)
        {
            var values = new decimal[this.measures.Count];
            int index = this.measures.IndexOf(model.Measure);
            int kiloIndicator = model.Kilo;
            values[index] = (decimal)model.Quantity;

            for (int i = index + 1; i < this.measures.Count; i++)
            {
                if (this.measures[i].IndexOf("Bit") >= 0)
                {
                    values[i] = values[i - 1] * 8 / kiloIndicator;
                }
                else
                {
                    values[i] = values[i - 1] / 8;
                }
            }

            for (int i = index - 1; i >= 0; i--)
            {
                if (this.measures[i].IndexOf("Bit") >= 0)
                {
                    values[i] = values[i + 1] * 8;
                }
                else
                {
                    values[i] = values[i + 1] / 8 * kiloIndicator;
                }
            }

            return values;
        }
    }
}