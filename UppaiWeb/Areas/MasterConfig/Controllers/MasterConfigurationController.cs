using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Areas.MasterConfig.Models;
using UppaiWeb.Controllers;
using UppaiWeb.Helpers;

namespace UppaiWeb.Areas.MasterConfig.Controllers
{
    [Authorize(Roles = "Admin, Partner")]
    public class MasterConfigurationController : BaseController
    {
        // GET: MasterConfig/MasterConfiguration
        ApiConnection objAPI = new ApiConnection("MasterConfig");
        #region BusinessCategory
        [Authorize(Roles = "Admin")]
        public ActionResult BusinessCategoryList(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/masterconfiguration/businesscategorylist";
                BusinessCategoryVM model = new BusinessCategoryVM();
                ViewBag.SearchTerm = SearchTerm;
                var query = "PageNo=" + PageNo + "&PageSize=" + PageSize + "&SearchTerm=" + SearchTerm;
                BusinessCategoryAPIVM Apimodel = objAPI.GetRecordByQueryString<BusinessCategoryAPIVM>("Master", "BCategoryList", query);
                model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = Apimodel.TotalRecords };
                model.BusinessCategoryList = Apimodel.BusinessCategoryList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_pvBusinessCategoryList", model);
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
        public ActionResult AddBusinessCategory(long id = 0)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/masterconfiguration/businesscategorylist";
                BusinessCategoryAdd model = new BusinessCategoryAdd();
                if (id != 0)
                    model.BusinessCategory = objAPI.GetRecordByQueryString<utblMstBusinessCategorie>("Master", "GetBCategoryByID", "ID=" + id);
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
        public ActionResult AddBusinessCategory(BusinessCategoryAdd model)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/masterconfiguration/businesscategorylist";
                if (ModelState.IsValid)
                {
                    model.BusinessCategory.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model.BusinessCategory);
                    string result = objAPI.PostRecordtoApI("Master", "AddBCategory", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("BusinessCategoryList", "masterconfiguration", new { Area = "masterconfig" });
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteBusinesscategory(long id)
        {
            try
            {
                TempData["ErrMsg"] = objAPI.DeleteRecordByQuerystring("Master", "DeleteBCategory", "id=" + id);
                return RedirectToAction("BusinessCategoryList", "masterconfiguration", new { Area = "masterconfig" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region MenuCategory
        [Authorize(Roles = "Admin,Partner")]
        public ActionResult MenuCategoryList(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/masterconfiguration/menucategorylist";
                MenuCategoryVM model = new MenuCategoryVM();
                ViewBag.SearchTerm = SearchTerm;
                var query = "PageNo=" + PageNo + "&PageSize=" + PageSize + "&SearchTerm=" + SearchTerm;
                MenuCategoryAPIVM Apimodel = objAPI.GetRecordByQueryString<MenuCategoryAPIVM>("Master", "MCategoryList", query);
                model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = Apimodel.TotalRecords };
                model.MenuCategoryList = Apimodel.MenuCategoryList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_pvMenuCategoryList", model);
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
        public ActionResult AddMenuCategory(long id = 0)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/masterconfiguration/menucategorylist";
                MenuCategoryAdd model = new MenuCategoryAdd();
                if (id != 0)
                {
                    model.MenuCategory = objAPI.GetRecordByQueryString<utblMenuItemCategorie>("Master", "GetMCategoryByID", "ID=" + id);
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
        [Authorize(Roles = "Admin")]
        public ActionResult AddMenuCategory(MenuCategoryAdd model)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/masterconfiguration/menucategorylist";
                if (ModelState.IsValid)
                {
                    model.MenuCategory.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model.MenuCategory);
                    string result = objAPI.PostRecordtoApI("Master", "AddMCategory", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("MenuCategoryList", "masterconfiguration", new { Area = "masterconfig" });
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteMenucategory(long id)
        {
            try
            {
                TempData["ErrMsg"] = objAPI.DeleteRecordByQuerystring("Master", "DeleteMCategory", "id=" + id);
                return RedirectToAction("MenuCategoryList", "masterconfiguration", new { Area = "masterconfig" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
       
    }
}