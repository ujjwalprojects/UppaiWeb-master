using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Areas.MasterConfig.Models;
using UppaiWeb.Controllers;
using UppaiWeb.Helpers;

namespace UppaiWeb.Areas.MasterConfig.Controllers
{
    public class MenuController : BaseController
    {
        ApiConnection objAPI = new ApiConnection("MasterConfig");
        //ApiConnection objMstAPI = new ApiConnection("MasterConfig");
        #region Menu
        [Authorize(Roles = "Admin,Partner")]
        public ActionResult MenuList(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/menu/menulist";
                MenuVM model = new MenuVM();
                string Id = "";
                ViewBag.SearchTerm = SearchTerm;
                if (User.IsInRole("Partner"))
                    Id = User.Identity.Name;
                var query = "PageNo=" + PageNo + "&PageSize=" + PageSize + "&SearchTerm=" + SearchTerm + "&AddedBy=" + Id;
                MenuAPIVM Apimodel = objAPI.GetRecordByQueryString<MenuAPIVM>("Menu", "MenuList", query);
                model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = Apimodel.TotalRecords };
                model.MenuList = Apimodel.MenuList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_pvMenuList", model);
                }
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }

        }
        [HttpGet]
        [Authorize(Roles = "Admin,Partner")]
        public ActionResult AddMenu()
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/menu/menulist";
                MenuAdd model = new MenuAdd();
                model.AddMenu = new utblMenuDetail();
                model.AddMenu.IsAvailable = true;
                model.MenuCategoryDDList = objAPI.GetAllRecords<MenuCategoryDD>("Master", "MenuCategoryDD");
                model.PartnerDDList = objAPI.GetAllRecords<PartnerDD>("Master", "PartnerDD");
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
        [Authorize(Roles = "Admin,Partner")]
        public ActionResult AddMenu(MenuAdd model)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/menu/menulist";
                string Img_result = "";
                if (User.IsInRole("Admin") && string.IsNullOrEmpty(model.AddMenu.AddedBy))
                {
                    ModelState.AddModelError("AddMenu.AddedBy", "Select Partner Name. Please try again.");
                }
                else if (User.IsInRole("Admin") && !string.IsNullOrEmpty(model.AddMenu.AddedBy))
                {
                    model.AddMenu.IsAddedByAdmin = true;
                }
                else
                {
                    model.AddMenu.AddedBy = User.Identity.Name;
                    model.AddMenu.IsAddedByAdmin = false;
                }
                if (!string.IsNullOrEmpty(model.AddMenu.ImageNormal))
                {
                    Img_result = SaveImage(model.AddMenu.ImageNormal);
                    if (!Img_result.Contains("Error"))
                    {
                        model.AddMenu.ImagesFile = Img_result;
                    }
                    else
                    {
                        ModelState.AddModelError("MenuImages", "Menu Image could not be uploaded. Please try again.");
                    }
                }
                if (ModelState.IsValid)
                {
                    //if (!string.IsNullOrEmpty(model.AddMenu.AddedBy))
                    //    model.AddMenu.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model.AddMenu);
                    string result = objAPI.PostRecordtoApI("Menu", "AddMenu", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("MenuList", "Menu", new { Area = "masterconfig" });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Img_result))
                        {
                            DeleteFile(Img_result);
                        }
                        TempData["ErrMsg"] = result;
                    }
                }
                model.MenuCategoryDDList = objAPI.GetAllRecords<MenuCategoryDD>("Master", "MenuCategoryDD");
                model.PartnerDDList = objAPI.GetAllRecords<PartnerDD>("Master", "PartnerDD");
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Partner")]
        public ActionResult EditMenu(long id = 0)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/menu/menulist";
                MenuEdit model = new MenuEdit();
                model.MenuCategoryDDList = objAPI.GetAllRecords<MenuCategoryDD>("Master", "MenuCategoryDD");
                model.PartnerDDList = objAPI.GetAllRecords<PartnerDD>("Master", "PartnerDD");
                if (id != 0)
                    model.EditMenu = objAPI.GetRecordByQueryString<utblMenuDetail>("Menu", "GetMenuByID", "ID=" + id);
                model.PrevImg = model.EditMenu.ImagesFile;
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
        [Authorize(Roles = "Admin,Partner")]
        public ActionResult EditMenu(MenuEdit model)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/menu/menulist";
                string Img_result = "";
                if (User.IsInRole("Admin") && string.IsNullOrEmpty(model.EditMenu.AddedBy))
                {
                    ModelState.AddModelError("EditMenu.AddedBy", "Select Partner Name. Please try again.");
                }
                else if (User.IsInRole("Admin") && !string.IsNullOrEmpty(model.EditMenu.AddedBy))
                {
                    model.EditMenu.IsAddedByAdmin = true;
                }
                else
                {
                    model.EditMenu.AddedBy = User.Identity.Name;
                    model.EditMenu.IsAddedByAdmin = false;
                }
                if (!string.IsNullOrEmpty(model.EditMenu.ImageNormal))
                {
                    Img_result = SaveImage(model.EditMenu.ImageNormal);
                    if (!Img_result.Contains("Error"))
                    {
                        model.EditMenu.ImagesFile = Img_result;
                    }
                    else
                    {
                        ModelState.AddModelError("MenuImages", "Menu Image could not be uploaded. Please try again.");
                    }
                }
                if (ModelState.IsValid)
                {
                    string jsonStr = JsonConvert.SerializeObject(model.EditMenu);
                    string result = objAPI.PostRecordtoApI("Menu", "AddMenu", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        if (!string.IsNullOrEmpty(model.EditMenu.ImageNormal))
                            DeleteFile(model.PrevImg);
                        return RedirectToAction("MenuList", "Menu", new { Area = "masterconfig" });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Img_result))
                        {
                            DeleteFile(Img_result);
                        }
                        TempData["ErrMsg"] = result;
                    }
                }
                model.MenuCategoryDDList = objAPI.GetAllRecords<MenuCategoryDD>("Master", "MenuCategoryDD");
                model.PartnerDDList = objAPI.GetAllRecords<PartnerDD>("Master", "PartnerDD");
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Partner")]
        public ActionResult DeleteMenu(long id)
        {
            try
            {
                utblMenuDetail objmenu = objAPI.GetRecordByQueryString<utblMenuDetail>("Menu", "GetMenuByID", "ID=" + id);
                string result= objAPI.DeleteRecordByQuerystring("Menu", "DeleteMenu", "id=" + id);
                if (result.Contains("Success"))
                {
                    TempData["ErrMsg"] = result;
                    if (!string.IsNullOrEmpty(objmenu.ImagesFile))
                        DeleteFile(objmenu.ImagesFile);
                }
                else
                {
                    TempData["ErrMsg"] = result;
                }
                return RedirectToAction("MenuList", "Menu", new { Area = "masterconfig" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Menu Non Available Days
        [HttpGet]
        [Authorize(Roles = "Admin,Partner")]
        public ActionResult AddNonAvailable()
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/menu/menulist";
                MenuNonAvlAdd model = new MenuNonAvlAdd();
                model.MenuDDList = objAPI.GetRecordsByQueryString<MenuDD>("Menu", "MenuDD", "UserName=" + User.Identity.Name);
                //model.NonAvlDays = new List<DayOfWeekEnum>();
                //for (int i = 0; i < 7; i++)
                //{
                //    DayOfWeekEnum daymodel = new DayOfWeekEnum()
                //    {
                //        DayNo = i,
                //        DayName = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[i],
                //    };
                //    model.NonAvlDays.Add(daymodel);
                //}
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
        [Authorize(Roles = "Admin,Partner")]
        public ActionResult AddNonAvailable(MenuNonAvlAdd model)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/menu/menulist";
                int TrueCount = 0;
                //foreach (var days in model.NonAvlDays)
                //{
                //    if (days.IsSelected == true)
                //    {
                //        TrueCount = TrueCount + 1;
                //    }
                //}
                if (TrueCount == 0)
                {
                    ModelState.AddModelError("ErrorMsg", "Please select Menu Non Available Day.");
                }
                if (ModelState.IsValid)
                {
                    model.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model);
                    string result = objAPI.PostRecordtoApI("Menu", "AddNonAvailableMenu", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("MenuList", "Menu", new { Area = "masterconfig" });
                    }
                    TempData["ErrMsg"] = result;
                }
                model.MenuDDList = objAPI.GetRecordsByQueryString<MenuDD>("Menu", "MenuDD", "UserName=" + User.Identity.Name);
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        #endregion

        #region Helper
        private string SaveImage(string imageStr)
        {
            try
            {
                string name = Guid.NewGuid().ToString();
                var path = ""; var folderpath = ""; string NamewithExt = "";
                NamewithExt = name + ".jpeg";
                path = Path.Combine(Server.MapPath("~/Uploads/Menu"), NamewithExt);
                folderpath = Server.MapPath("~/Uploads/Menu");
                //Check if directory exist
                if (!System.IO.Directory.Exists(folderpath))
                {
                    System.IO.Directory.CreateDirectory(folderpath); //Create directory if it doesn't exist
                }
                string x = imageStr.Replace("data:image/jpeg;base64,", "");
                byte[] imageBytes = Convert.FromBase64String(x);

                System.IO.File.WriteAllBytes(path, imageBytes);
                return NamewithExt;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        private void DeleteFile(string filepath)
        {
            try
            {
                string[] fileArr = filepath.Split('/');
                string filename = fileArr[fileArr.Length - 1];
                string serverPath = "";
                serverPath = Path.Combine(Server.MapPath("~/Uploads/Menu"), filename);
                if (System.IO.File.Exists(serverPath))
                {
                    System.IO.File.Delete(serverPath);
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        #endregion
    }
}