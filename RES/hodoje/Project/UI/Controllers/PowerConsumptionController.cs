using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Entities.Models;
using DataAccess;
using FileReader;
using FileReader.Interfaces;
using DataProxy;

namespace UI.Controllers
{
    public class PowerConsumptionController : Controller
    {
        private readonly IPowerConsumptionCachedData _cachedData;

        public PowerConsumptionController(IPowerConsumptionCachedData cachedData)
        {
            _cachedData = cachedData;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        public ActionResult GetData(InputDate inputDate)
        {
            TempData["inputDate"] = inputDate;
            return RedirectToAction("ShowData");
        }

        public ActionResult ShowData()
        {
            InputDate inputDate = (InputDate) TempData["inputDate"];
            List<PowerConsumptionData> listOfData;

            if (inputDate != null)
            {
                if (inputDate.From > DateTime.Now || inputDate.To > DateTime.Now)
                {
                    TempData["ErrorMessage"] = "'From' or 'To' date can't pass the date that is now.";
                    return RedirectToAction("GetData");
                }
                if (inputDate.To != DateTime.MinValue)
                {
                    if (inputDate.From > inputDate.To)
                    {
                        TempData["ErrorMessage"] = "'From' date has to be earlier than 'To' date.";
                        return RedirectToAction("GetData");
                    }
                }

                listOfData = (List<PowerConsumptionData>)_cachedData.Get(inputDate);

                listOfData = listOfData.OrderBy(x => x.GeoAreaId).ThenBy(x => x.Timestamp.TimeOfDay).ToList();
                return View(listOfData);
            }
            else
            {
                return RedirectToAction("GetData");
            }
        }
    }
}
