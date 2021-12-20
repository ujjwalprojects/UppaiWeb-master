using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Areas.MasterConfig.Models;
using UppaiWeb.Controllers;
using UppaiWeb.Helpers;

namespace UppaiWeb.Areas.MasterConfig.Controllers
{
    public class WebContentController : BaseController
    {
        ApiConnection objAPI = new ApiConnection("MasterConfig");
        // GET: MasterConfig/WebContent
        #region Banner
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult BannerList(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/webcontent/bannerlist";
                BannerVM model = new BannerVM();
                ViewBag.SearchTerm = SearchTerm;
                var query = "PageNo=" + PageNo + "&PageSize=" + PageSize + "&SearchTerm=" + SearchTerm;
                BannerAPIVM Apimodel = objAPI.GetRecordByQueryString<BannerAPIVM>("WebContent", "BannerList", query);
                model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = Apimodel.TotalRecords };
                model.BannerList = Apimodel.BannerList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_pvBannerList", model);
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
        public ActionResult AddBanner()
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/webcontent/bannerlist";
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
        public ActionResult AddBanner(AddBanner model)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/webcontent/bannerlist";
                string Img_result = "";
                if (!string.IsNullOrEmpty(model.BannerDetail.BannerNormal))
                {
                    Img_result = SaveImage(model.BannerDetail.BannerNormal);
                    if (!Img_result.Contains("Error"))
                    {
                        model.BannerDetail.BannerFilePath = Img_result;
                    }
                    else
                    {
                        ModelState.AddModelError("BannerDetail.BannerNormal", "Banner Image could not be uploaded. Please try again.");
                    }
                }
                else
                {
                    ModelState.AddModelError("BannerDetail.BannerNormal", "Banner Image could not be uploaded. Please try again.");
                }
                if (ModelState.IsValid)
                {
                    model.BannerDetail.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model.BannerDetail);
                    string result = objAPI.PostRecordtoApI("WebContent", "AddBanner", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("BannerList", "WebContent", new { Area = "masterconfig" });
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
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditBanner(long id)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/webcontent/bannerlist";
                EditBanner model = new EditBanner();
                model.BannerDetail = objAPI.GetRecordByQueryString<utblBannerDetail>("WebContent", "GetBannerByID", "ID=" + id);
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
        public ActionResult EditBanner(EditBanner model)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/webcontent/bannerlist";
                string Img_result = "";
                string PrevBanner = model.BannerDetail.BannerFilePath;
                if (!string.IsNullOrEmpty(model.BannerDetail.BannerNormal))
                {
                    Img_result = SaveImage(model.BannerDetail.BannerNormal);
                    if (!Img_result.Contains("Error"))
                    {
                        model.BannerDetail.BannerFilePath = Img_result;
                    }
                    else
                    {
                        ModelState.AddModelError("BannerDetail.BannerNormal", "Banner Image could not be uploaded. Please try again.");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(model.BannerDetail.BannerFilePath))
                        ModelState.AddModelError("BannerDetail.BannerNormal", "Banner Image could not be uploaded. Please try again.");
                }
                if (ModelState.IsValid)
                {
                    model.BannerDetail.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model.BannerDetail);
                    string result = objAPI.PostRecordtoApI("WebContent", "AddBanner", jsonStr);
                    if (result.Contains("Success"))
                    {
                        if (!string.IsNullOrEmpty(model.BannerDetail.BannerNormal))
                            DeleteFile(PrevBanner);
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("BannerList", "WebContent", new { Area = "masterconfig" });
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
        public ActionResult DeleteBanner(long id, string FilePath)
        {
            try
            {
                string result = objAPI.DeleteRecordByQuerystring("WebContent", "DeleteBanner", "ID=" + id);
                if (result.Contains("Success"))
                {
                    TempData["ErrMsg"] = result;
                    if (!string.IsNullOrEmpty(FilePath))
                        DeleteFile(FilePath);
                }
                else
                {
                    TempData["ErrMsg"] = result;
                }
                return RedirectToAction("BannerList", "WebContent", new { Area = "masterconfig" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult BannerPublish(long id)
        {
            TempData["ErrMsg"] = objAPI.PostRecordUsingQueryString("WebContent", "Publish", "ID=" + id);
            return RedirectToAction("BannerList", "WebContent", new { Area = "masterconfig" });
        }
        #endregion
        #region CMS Content
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CMSContentList()
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/webcontent/CMSContentList";
                CMSContentVM model = new CMSContentVM();
                model.CMSContentList = objAPI.GetAllRecords<utblCMSContentDetail>("WebContent", "CMSContentList");
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_pvCMSContentList", model);
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
        public ActionResult CMSContentDetails(string id = "")
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/webcontent/CMSContentList";
                CMSContentVM model = new CMSContentVM();
                utblCMSContentDetail _cmsmodel = objAPI.GetRecordByQueryString<utblCMSContentDetail>("WebContent", "CMSContentByID", "Id=" + id);
                model.CMSContent = new utblCMSContentDetail()
                {
                    CMSContentID = _cmsmodel.CMSContentID,
                    CMSContentTitle = _cmsmodel.CMSContentTitle,
                    CMSContent = HttpUtility.HtmlDecode(_cmsmodel.CMSContent),
                    AddedBy = _cmsmodel.AddedBy,
                    AddedOn = _cmsmodel.AddedOn
                };
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
        public ActionResult CMSContentDetails(CMSContentVM model)
        {
            try
            {
                ViewBag.ActiveURL = "/masterconfig/webcontent/CMSContentList";
                if (ModelState.IsValid)
                {
                    model.CMSContent.AddedBy = User.Identity.Name;
                    string jsonStr = JsonConvert.SerializeObject(model.CMSContent);
                    string result = objAPI.PostRecordtoApI("WebContent", "SaveCMSContent", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("CMSContentList", "WebContent", new { Area = "masterconfig" });
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
        #endregion
        #region Helper
        private string SaveImage(string imageStr)
        {
            try
            {
                string name = Guid.NewGuid().ToString();
                var path = ""; var folderpath = ""; string NamewithExt = "";
                NamewithExt = name + ".jpeg";
                path = Path.Combine(Server.MapPath("~/Uploads/Banner"), NamewithExt);
                folderpath = Server.MapPath("~/Uploads/Banner");
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
                serverPath = Path.Combine(Server.MapPath("~/Uploads/Banner"), filename);
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