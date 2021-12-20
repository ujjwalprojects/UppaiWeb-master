using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Areas.DeliveryAgent.Models;
using UppaiWeb.Controllers;
using UppaiWeb.Helpers;
using UppaiWeb.Models;

namespace UppaiWeb.Areas.DeliveryAgent.Controllers
{
    public class RegisterController : BaseController
    {
        // GET: DeliveryAgent/Register
        ApiConnection objAPI = new ApiConnection("DeliveryAgent");
        ApiConnection objGenAPI = new ApiConnection("");
        [Authorize(Roles = "Admin")]
        public ActionResult DeliveryAgentList(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            try
            {
                ViewBag.ActiveURL = "/deliveryagent/register/deliveryagentlist";
                DeliveryAgentVM model = new DeliveryAgentVM();
                ViewBag.SearchTerm = SearchTerm;
                var query = "PageNo=" + PageNo + "&PageSize=" + PageSize + "&SearchTerm=" + SearchTerm;
                DeliveryAgentAPIVM Apimodel = objAPI.GetRecordByQueryString<DeliveryAgentAPIVM>("RegisterAgent", "DeliveryAgentList", query);
                model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = Apimodel.TotalRecords };
                model.DeliveryAgentList = Apimodel.DeliveryAgentList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_pvDeliveryAgentList", model);
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
        public ActionResult AddDeliveryAgent()
        {
            try
            {
                ViewBag.ActiveURL = "/deliveryagent/register/deliveryagentlist";
                return View();
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
        public ActionResult AddDeliveryAgent(DeliveryAgentAdd model)
        {
            try
            {
                ViewBag.ActiveURL = "/deliveryagent/register/deliveryagentlist";
                if (ModelState.IsValid)
                {
                    model.DeliveryAgentDetails.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model.DeliveryAgentDetails);
                    string result = objAPI.PostRecordtoApI("RegisterAgent", "Add", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("DeliveryAgentList", "register", new { Area = "DeliveryAgent" });
                    }
                    TempData["ErrMsg"] = result;
                }
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Delivery Agent")]
        public ActionResult EditDeliveryAgent(string id)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    ViewBag.ActiveURL = "/deliveryagent/register/deliveryagentlist";
                }
                else
                {
                    ViewBag.ActiveURL = "/deliveryagent/register/myprofile";
                }
                DeliveryAgentEdit model = new DeliveryAgentEdit();
                RegisterModel objreg = objGenAPI.GetRecordByQueryString<RegisterModel>("account", "GetUserByID", "ID=" + id);
                model.DeliveryAgentDetails = objAPI.GetRecordByQueryString<utblDeliveryAgent>("RegisterAgent", "GetDeliveryAgentByID", "ID=" + objreg.UserName);
                model.RoleName = objreg.RoleName;
                model.UserName = objreg.UserName;
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
        [Authorize(Roles = "Admin, Delivery Agent")]
        public ActionResult EditDeliveryAgent(DeliveryAgentEdit model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    ViewBag.ActiveURL = "/deliveryagent/register/deliveryagentlist";
                }
                else
                {
                    ViewBag.ActiveURL = "/deliveryagent/register/myprofile";
                }
                
                if (ModelState.IsValid)
                {
                    model.DeliveryAgentDetails.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model.DeliveryAgentDetails);
                    string result = objAPI.PostRecordtoApI("RegisterAgent", "Edit", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("DeliveryAgentList", "register", new { Area = "DeliveryAgent" });
                        }
                        else
                        {
                            return RedirectToAction("MyProfile", "register", new { Area = "DeliveryAgent" });
                        }
                    }
                    TempData["ErrMsg"] = result;
                }
                //model.DeliveryAgentDetails = objAPI.GetRecordByQueryString<utblDeliveryAgentDetail>("register", "GetDeliveryAgentByID", "ID=" + model.DeliveryAgentDetails.DeliveryAgentID);
                return View(model);
            }

            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Delivery Agent")]
        public ActionResult MyProfile()
        {
            try
            {
                ViewBag.ActiveURL = "/deliveryagent/register/myprofile";
                DeliveryAgentEdit model = new DeliveryAgentEdit();
                string id = User.Identity.Name;
                model.DeliveryAgentDetails = objAPI.GetRecordByQueryString<utblDeliveryAgent>("RegisterAgent", "GetDeliveryAgentByID", "ID=" + id);
                RegisterModel objreg = objGenAPI.GetRecordByQueryString<RegisterModel>("account", "GetUserByID", "ID=" + model.DeliveryAgentDetails.Id);
                model.RoleName = objreg.RoleName;
                model.UserName = objreg.UserName;
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
                TempData["ErrMsg"] = objAPI.PostRecordUsingQueryString("RegisterAgent", "ChangeStatus", "Id=" + id);
                return RedirectToAction("DeliveryAgentList", "register", new { Area = "DeliveryAgent" });
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
    }
}