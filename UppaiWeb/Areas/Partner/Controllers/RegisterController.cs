using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Areas.MasterConfig.Models;
using UppaiWeb.Areas.Partner.Models;
using UppaiWeb.Controllers;
using UppaiWeb.Helpers;
using UppaiWeb.Models;
//using UppaiWeb.ViewModels;

namespace UppaiWeb.Areas.Partner.Controllers
{
    [Authorize(Roles = "Admin, Partner")]
    public class RegisterController : BaseController
    {
        // GET: Partner/Register
        ApiConnection objAPI = new ApiConnection("Partner");
        ApiConnection objMstAPI = new ApiConnection("MasterConfig");
        ApiConnection objGenAPI = new ApiConnection("");
        [Authorize(Roles = "Admin")]
        public ActionResult PartnerList(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            try
            {
                ViewBag.ActiveURL = "/partner/register/partnerlist";
                PartnerVM model = new PartnerVM();
                ViewBag.SearchTerm = SearchTerm;
                var query = "PageNo=" + PageNo + "&PageSize=" + PageSize + "&SearchTerm=" + SearchTerm;
                PartnerAPIVM Apimodel = objAPI.GetRecordByQueryString<PartnerAPIVM>("register", "PartnerList", query);
                model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = Apimodel.TotalRecords };
                model.PartnerList = Apimodel.PartnerList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_pvPartnerList", model);
                }
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddPartner()
        {
            try
            {
                ViewBag.ActiveURL = "/partner/register/partnerlist";
                PartnerAdd model = new PartnerAdd();
                model.BCategoryDDList = objMstAPI.GetAllRecords<BCategoryDD>("Master", "GetBCategoryDD");
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult AddPartner(PartnerAdd model)
        {
            try
            {
                ViewBag.ActiveURL = "/partner/register/partnerlist";
                if (ModelState.IsValid)
                {
                    model.PartnerDetails.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model.PartnerDetails);
                    string result = objAPI.PostRecordtoApI("register", "Add", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("PartnerList", "register", new { Area = "partner" });
                    }
                    TempData["ErrMsg"] = result;
                }
                model.BCategoryDDList = objMstAPI.GetAllRecords<BCategoryDD>("Master", "GetBCategoryDD");
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Partner")]
        public ActionResult EditPartner(string id)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    ViewBag.ActiveURL = "/partner/register/partnerlist";
                }
                else
                {
                    ViewBag.ActiveURL = "/partner/register/myprofile";
                }
                PartnerEdit model = new PartnerEdit();
                RegisterModel objreg = objGenAPI.GetRecordByQueryString<RegisterModel>("account", "GetUserByID", "ID=" + id);
                model.PartnerDetails = objAPI.GetRecordByQueryString<utblPartnerDetail>("register", "GetPartnerByID", "ID=" + objreg.Email);
                model.RoleName = objreg.RoleName;
                model.BCategoryDDList = objMstAPI.GetAllRecords<BCategoryDD>("Master", "GetBCategoryDD");
                model.OperationalDays = new List<DayOfWeekEnum>();
                model.OperationalDays = objAPI.GetRecordsByQueryString<DayOfWeekEnum>("register", "GetWeekDays", "ID=" + model.PartnerDetails.PartnerID);
                if (model.OperationalDays.Count == 0)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        DayOfWeekEnum daymodel = new DayOfWeekEnum()
                        {
                            DayNo = i,
                            DayName = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[i],
                        };
                        model.OperationalDays.Add(daymodel);
                    }
                }
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Partner")]
        public ActionResult EditPartner(PartnerEdit model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    ViewBag.ActiveURL = "/partner/register/partnerlist";
                }
                else
                {
                    ViewBag.ActiveURL = "/partner/register/myprofile";
                }
                if (string.IsNullOrEmpty(model.PartnerDetails.OpeningTime))
                {
                    ModelState.AddModelError("PartnerDetails.OpeningTime", "Please Enter Opening Timing");
                }
                if (string.IsNullOrEmpty(model.PartnerDetails.ClosedTime))
                {
                    ModelState.AddModelError("PartnerDetails.ClosedTime", "Please Enter Closing Timing");
                }
                if (ModelState.IsValid)
                {
                    model.PartnerDetails.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model);
                    string result = objAPI.PostRecordtoApI("register", "Edit", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("PartnerList", "register", new { Area = "partner" });
                        }
                        else
                        {
                            return RedirectToAction("MyProfile", "register", new { Area = "partner" });
                        }
                    }
                    TempData["ErrMsg"] = result;
                }
                model.BCategoryDDList = objMstAPI.GetAllRecords<BCategoryDD>("Master", "GetBCategoryDD");
                //model.PartnerDetails = objAPI.GetRecordByQueryString<utblPartnerDetail>("register", "GetPartnerByID", "ID=" + model.PartnerDetails.PartnerID);
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Partner")]
        public ActionResult MyProfile()
        {
            try
            {
                ViewBag.ActiveURL = "/partner/register/myprofile";
                PartnerEdit model = new PartnerEdit();
                string id = User.Identity.Name;
                model.PartnerDetails = objAPI.GetRecordByQueryString<utblPartnerDetail>("register", "GetPartnerByID", "ID=" + id);
                RegisterModel objreg = objGenAPI.GetRecordByQueryString<RegisterModel>("account", "GetUserByID", "ID=" + model.PartnerDetails.Id);
                model.OperationalDays = objAPI.GetRecordsByQueryString<DayOfWeekEnum>("register", "GetWeekDays", "ID=" + model.PartnerDetails.PartnerID);
                model.RoleName = objreg.RoleName;
                model.OpenRangeDays = new List<string>();
                string startDays = "";
                for (int i = 0; i < model.OperationalDays.Count(); i++)
                {
                    if (model.OperationalDays.ElementAt(i).IsSelected && startDays == "")
                    {
                        startDays = model.OperationalDays.ElementAt(i).DayName.ToString();
                    }
                    if (!model.OperationalDays.ElementAt(i).IsSelected && startDays != "")
                    {
                        if (startDays != model.OperationalDays.ElementAt(i - 1).DayName)
                            startDays = startDays + "-" + model.OperationalDays.ElementAt(i - 1).DayName;
                        else
                            startDays = model.OperationalDays.ElementAt(i - 1).DayName;
                        model.OpenRangeDays.Add(startDays);
                        startDays = "";
                    }
                    if (i == (model.OperationalDays.Count() - 1) && model.OperationalDays.ElementAt(i).IsSelected)
                    {
                        if (startDays != model.OperationalDays.ElementAt(i).DayName)
                            startDays = startDays + "-" + model.OperationalDays.ElementAt(i).DayName;
                        else
                            startDays = model.OperationalDays.ElementAt(i).DayName;
                        model.OpenRangeDays.Add(startDays);
                        startDays = "";
                    }
                }
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DisableEnableUser(string id)
        {
            try
            {
                TempData["ErrMsg"] = objAPI.PostRecordUsingQueryString("register", "ChangeStatus", "Id=" + id);
                return RedirectToAction("PartnerList", "register", new { Area = "partner" });
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
    }
}