using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesUI.Pages
{
    public class AssignmentModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        [BindProperty]
        public DateTime UserInput { get; set; }
        [BindProperty]
        public List<string> ResultDataOutside { get; set; }
        public List<string> ResultDataInside { get; set; }
        public List<string> ResultData { get; set; }
        [BindProperty]
        public bool InputBool { get; set; }
        [BindProperty]
        public bool InputBoolFalse { get; set; }

        public void OnGet()
        {
            switch (Id)
            {
                case "1":
                    ResultData = QueryMethods.MoldRiskQuery(false);
                    break;
                case "2":
                    ResultData = QueryMethods.MoldRiskQuery(true);
                    break;
                case "3":
                    ResultData = QueryMethods.MeteorologicalFallWinterQuery(true);
                    break;
                case "4":
                    ResultData = QueryMethods.MeteorologicalFallWinterQuery(false);
                    break;
                case "5":
                    ResultData = QueryMethods.HotToColdestQuery(false);
                    break;
                case "6":
                    ResultData = QueryMethods.HotToColdestQuery(true);
                    break;
                case "7":
                    ResultData = QueryMethods.AvgHumQuery(false);
                    break;
                case "8":
                    ResultData = QueryMethods.AvgHumQuery(true);
                    break;
                case "9":
                    ResultData = QueryMethods.HotToColdestQuery(true);
                    break;
            }
        }
        public void OnPost()
        {

            InputBool = true;
            InputBoolFalse = false;

            ResultDataOutside = QueryMethods.AverageTempUserInput(UserInput, InputBoolFalse);
            ResultDataInside = QueryMethods.AverageTempUserInput(UserInput, InputBool);

        }
    }
}
