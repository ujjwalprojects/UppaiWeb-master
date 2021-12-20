using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Areas.MasterConfig.Models;
using UppaiWeb.Helpers;
using UppaiWeb.ViewModels;

namespace UppaiWeb.Controllers
{
    public class HomeController : HomeBaseController
    {
        // GET: Home
        GenWebApiConnection objGenApi = new GenWebApiConnection();
        public ActionResult Index()
        {
            HomeVM model = new HomeVM();
            List<CheckOutItem> objsession = new List<CheckOutItem>();
            int SessionCount = 0;
            if (Session["CheckOutItems"] != null)
            {
                objsession = Session["CheckOutItems"] as List<CheckOutItem>;
                SessionCount = objsession.Sum(x => x.Qty);
            }
            ViewBag.LayoutCart = SessionCount;
            model.BizCategotyList = objGenApi.GetRecords<BCategoryDD>("Home", "GetBizCategoryList");
            model.BannerList = objGenApi.GetRecords<utblBannerDetail>("Home", "GetBannerList");
            var query = "BizID=" + 0 + "&SearchTerm=" + "";//For Top Picks
            model.PartnerList = objGenApi.GetRecordsByQueryString<PartnerDtlsView>("Home", "GetPartnerByBiz", query);//Get Partner Details By Business category
            return View(model);
        }

        public ActionResult MenuDetails(string ID)
        {
            Session["MenuItems"] = null;
            //Session["CheckOutItems"] = null;
            MenuDtlsVM model = new MenuDtlsVM();
            model.PartnerDtls = objGenApi.GetRecordByQueryString<PartnerDtlsView>("Home", "GetPartnerByID", "ID=" + ID);//Get Partner Details By ID
            model.MenuCategoryList = objGenApi.GetRecordsByQueryString<MenuCategory>("Home", "GetMenuCatByPartner", "ID=" + ID);//Get Menu Category List By Partner ID
            if (Session["CheckOutItems"] != null)
            {
                model.CheckOutItems = Session["CheckOutItems"] as List<CheckOutItem>;
            }
            else
            {
                model.CheckOutItems = null;
            }
            model.OrdFrom = model.PartnerDtls.Name;
            return View(model);
            //model.MenuList = objGenApi.GetRecordsByQueryString<MenuDtlsView>("Home", "GetMenuByPartner", "ID=" + ID);//Get Menu List By Partner
            //model.OpenRangeDays = new List<string>();
            //model.OpenRangeDays = model.PartnerDtls.OperationDay.Split(',').ToList();

        }
        public ActionResult GetMenuList(long CatID = 0, string SearchTerm = "", string PID = "", bool IsVegOnly = false)
        {
            MenuDtlsVM model = new MenuDtlsVM();
            string MenuType = "";

            if (IsVegOnly)
                MenuType = "Veg Menu";
            var query = "CatID=" + CatID + "&PartnerID=" + PID + "&SearchTerm=" + SearchTerm + "&MenuType=" + MenuType;
            model.MenuList = objGenApi.GetRecordsByQueryString<MenuDtlsView>("Home", "GetMenuByPartner", query);//Get Menu List By Partner
            //model.MenuList = objGenApi.GetRecordsByQueryString<MenuDtlsView>("Home", "GetMenuByPartner", "ID=" + PID);//Get Menu List By Partner
            return PartialView("_pvMenuDtl", model);
        }
        public ActionResult PartnerList()
        {
            PartnerDtlsVM model = new PartnerDtlsVM();
            List<CheckOutItem> objsession = new List<CheckOutItem>();
            int SessionCount = 0;
            if (Session["CheckOutItems"] != null)
            {
                objsession = Session["CheckOutItems"] as List<CheckOutItem>;
                SessionCount = objsession.Sum(x => x.Qty);
            }
            ViewBag.LayoutCart = SessionCount;
            model.BizCategotyList = objGenApi.GetRecords<BCategoryDD>("Home", "GetBizCategoryList");
            return View(model);
        }
        public ActionResult GetPartnerByBiz(long BizID = 0, string SearchTerm = "")
        {
            HomeVM model = new HomeVM();
            var query = "BizID=" + BizID + "&SearchTerm=" + SearchTerm;
            model.PartnerList = objGenApi.GetRecordsByQueryString<PartnerDtlsView>("Home", "GetPartnerByBiz", query);//Get Partner Details By Business category
            return PartialView("_pvPartnerDtl", model);
        }

        public ActionResult CMSContent(string Title)
        {
            CMSContentView model = new CMSContentView();
            List<CheckOutItem> objsession = new List<CheckOutItem>();
            int SessionCount = 0;
            if (Session["CheckOutItems"] != null)
            {
                objsession = Session["CheckOutItems"] as List<CheckOutItem>;
                SessionCount = objsession.Sum(x => x.Qty);
            }
            ViewBag.LayoutCart = SessionCount;
            model = objGenApi.GetRecordByQueryString<CMSContentView>("Home", "CMSContentByTitle", "Title=" + Title.ToLower());
            return View(model);
        }

        #region Cart Section
        [HttpGet]
        public JsonResult GetItemDtlsByID(long id)
        {
            MenuDtls objmenu = new MenuDtls();
            if (Session["MenuItems"] == null)
            {
                List<MenuDtls> MenuList = new List<MenuDtls>();
                MenuList = objGenApi.GetRecords<MenuDtls>("Home", "GetMenuList");
                Session["MenuItems"] = MenuList;
                objmenu = MenuList.Where(x => x.MenuID == id).FirstOrDefault();
            }
            else
            {
                List<MenuDtls> curMenuList = Session["MenuItems"] as List<MenuDtls>;
                objmenu = curMenuList.Where(x => x.MenuID == id).FirstOrDefault();
            }
            return Json(objmenu, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PostCheckOutItems(List<CheckOutItem> CheckOutList)
        {
            List<CheckOutItem> ItemOutList = new List<CheckOutItem>();
            ItemOutList = CheckOutList;
            Session["CheckOutItems"] = ItemOutList;
            int SessionCount = 0;
            if (Session["CheckOutItems"] != null)
            {
                SessionCount = ItemOutList.Sum(x => x.Qty);
            }
            return Json(ItemOutList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}