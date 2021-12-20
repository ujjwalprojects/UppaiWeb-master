using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using UppaiWeb.Areas.Admin.CustomModels;
using UppaiWeb.Helpers;
using UppaiWeb.Models;
using UppaiWeb.ViewModels;

namespace UppaiWeb.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {

        ApiConnection objAPI = new ApiConnection("");
        ApiConnection objAPICon = new ApiConnection("Admin");
        ApiConnection objAPINav = new ApiConnection("general");
        private string LoginUrl = ConfigurationManager.AppSettings["LoginURL"];

        public UserController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public UserController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        //
        // GET: /User/
        public ActionResult Dashboard(string returnnav)
        {
            if (User.IsInRole("Customer"))
            {
                return RedirectToAction("AddToShoppingCart", "account", new { UrlVal = returnnav });
            }
            return View();
        }

        // GET: /User/Register from Admin Section
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.ActiveURL = "/user/userlist";
            UserRegisterVM model = new UserRegisterVM();
            model.RoleDDList = objAPI.GetAllRecords<RoleDD>("account", "RoleDDList");
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Register(UserRegisterVM model)
        {
            //ViewBag.ActiveURL = "/user/userlist";
            if (ModelState.IsValid)
            {
                string jsonStr = JsonConvert.SerializeObject(model.RegisterModel);
                TempData["ErrMsg"] = objAPI.PostRecordtoApI("account", "register", jsonStr);
                return RedirectToAction("UserList", "User");
            }
            // If we got this far, something failed, redisplay form
            model.RoleDDList = objAPI.GetAllRecords<RoleDD>("account", "RoleDDList");
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UserList(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            ViewBag.ActiveURL = "/user/userlist";
            UserListVM model = new UserListVM();
            ViewBag.SearchTerm = SearchTerm;
            var query = "PageNo=" + PageNo + "&PageSize=" + PageSize + "&SearchTerm=" + SearchTerm;
            UserListAPIVM Apimodel = objAPI.GetRecordByQueryString<UserListAPIVM>("account", "UserList", query);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = Apimodel.TotalRecords };
            model.UserList = Apimodel.UserList;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvUserList", model);
            }
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult EditUser(string id)
        {
            ViewBag.ActiveURL = "/user/userlist";
            UserRegisterVM model = new UserRegisterVM();
            model.RoleDDList = objAPI.GetAllRecords<RoleDD>("account", "RoleDDList");
            model.RegisterModel = objAPI.GetRecordByQueryString<RegisterModel>("account", "GetUserByID", "ID=" + id);
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(UserRegisterVM model)
        {
            if (ModelState.IsValid)
            {
                string jsonStr = JsonConvert.SerializeObject(model.RegisterModel);
                TempData["ErrMsg"] = objAPI.PostRecordtoApI("account", "Edit", jsonStr);
                return RedirectToAction("UserList", "User");
            }
            // If we got this far, something failed, redisplay form
            model.RoleDDList = objAPI.GetAllRecords<RoleDD>("account", "RoleDDList");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DisableEnableUser(string id)
        {
            TempData["ErrMsg"] = objAPI.PostRecordUsingQueryString("account", "ChangeStatus", "Id=" + id);
            return RedirectToAction("UserList", "User");
        }


        internal class UserAuthorizeAttribute : Attribute
        {
        }
    }
}