using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Areas.General.Models;
using UppaiWeb.Controllers;
using UppaiWeb.Helpers;
using UppaiWeb.ViewModels;

namespace UppaiWeb.Areas.General.Controllers
{
    public class OrderManageController : BaseController
    {
        ApiConnection objAPI = new ApiConnection("General");

        // GET: General/OrderManage
        [Authorize(Roles = "Admin, Partner, Delivery Agent")]
        public ActionResult OrderManageList(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            try
            {
                ViewBag.ActiveURL = "/general/ordermanage/ordermanagelist";
                OrderManageVM model = new OrderManageVM();
                ViewBag.SearchTerm = SearchTerm;
                //string UserRole = ViewBag.Role;
                var query = "PageNo=" + PageNo + "&PageSize=" + PageSize + "&SearchTerm=" + SearchTerm + "&UserName=" + User.Identity.Name;
                OrderManageAPIVM Apimodel = objAPI.GetRecordByQueryString<OrderManageAPIVM>("OrderManage", "OrderManageList", query);
                model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = Apimodel.TotalRecords };
                model.OrderManageList = Apimodel.OrderManageList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_pvOrderManageList", model);
                }
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Partner")]
        public ActionResult OrderAckView(string OrderID)
        {
            try
            {
                ViewBag.ActiveURL = "/general/ordermanage/ordermanagelist";
                OrderAckVM model = new OrderAckVM();
                model.OrderItems = objAPI.GetRecordsByQueryString<CheckOutItem>("Customer", "GetOrderItemsByID", "Id=" + OrderID);
                model.OrderManage = objAPI.GetRecordByQueryString<OrderManageView>("OrderManage", "GetOrderByID", "Id=" + OrderID);
                model.OrderAck = new OrderAckModel()
                {
                    OrderID = model.OrderManage.OrderID,
                    Remarks = ""
                };
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            //return PartialView("_pvOrderDtlsView", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Partner")]
        public ActionResult OrderAckView(OrderAckVM model, string btnReq)
        {
            try
            {
                ViewBag.ActiveURL = "/general/ordermanage/ordermanagelist";
                if (string.IsNullOrEmpty(model.OrderAck.Reason) && string.IsNullOrEmpty(model.OrderAck.Remarks))
                {
                    if (btnReq == "Order Cancelled")
                        ModelState.AddModelError("OrderAck.Reason", "Enter Cancelled Reason");
                    else
                        ModelState.AddModelError("OrderAck.Remarks", "Select Time Taken To Prepare Order");
                }
                //if (btnReq == "Order Cancelled" && string.IsNullOrEmpty(Reason))
                //    ModelState.AddModelError("Reason", "Enter Cancelled Reason");
                if (btnReq == "Order Cancelled")
                    model.OrderAck.Remarks = model.OrderAck.Reason;
                if (ModelState.IsValid)
                {
                    model.OrderAck.Status = btnReq;
                    string jsonStr = JsonConvert.SerializeObject(model.OrderAck);
                    string result = objAPI.PostRecordtoApI("OrderManage", "OrderAckByPartner", jsonStr);
                    if (!result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("OrderManageList", "OrderManage", new { Area = "General" });
                    }
                    TempData["ErrMsg"] = result;
                }
                ViewBag.BtnReq = btnReq;
                model.OrderItems = objAPI.GetRecordsByQueryString<CheckOutItem>("Customer", "GetOrderItemsByID", "Id=" + model.OrderAck.OrderID);
                model.OrderManage = objAPI.GetRecordByQueryString<OrderManageView>("OrderManage", "GetOrderByID", "Id=" + model.OrderAck.OrderID);
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult OrderAssign(string OrderID)
        {
            try
            {
                ViewBag.ActiveURL = "/general/ordermanage/ordermanagelist";
                OrderAssignVM model = new OrderAssignVM();
                model.OrderItems = objAPI.GetRecordsByQueryString<CheckOutItem>("Customer", "GetOrderItemsByID", "Id=" + OrderID);
                model.OrderManage = objAPI.GetRecordByQueryString<OrderManageView>("OrderManage", "GetOrderByID", "Id=" + OrderID);
                model.DeliveryAgentDDList = objAPI.GetAllRecords<DeliveryAgentDD>("Customer", "GetDeliveryAgentDD");
                model.OrderAssign = objAPI.GetRecordByQueryString<utblOrderAssignDetail>("OrderManage", "GetAssignToByID", "Id=" + OrderID);
                model.OrderID = model.OrderManage.OrderID;
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
        public ActionResult OrderAssign(OrderAssignVM model)
        {
            try
            {
                ViewBag.ActiveURL = "/general/ordermanage/ordermanagelist";
                if (ModelState.IsValid)
                {
                    model.OrderAssign.OrderID = model.OrderID;
                    string jsonStr = JsonConvert.SerializeObject(model.OrderAssign);
                    string result = objAPI.PostRecordtoApI("OrderManage", "OrderAssignTo", jsonStr);
                    if (!result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("OrderManageList", "OrderManage", new { Area = "General" });
                    }
                    TempData["ErrMsg"] = result;
                }
                model.OrderItems = objAPI.GetRecordsByQueryString<CheckOutItem>("Customer", "GetOrderItemsByID", "Id=" + model.OrderID);
                model.OrderManage = objAPI.GetRecordByQueryString<OrderManageView>("OrderManage", "GetOrderByID", "Id=" + model.OrderID);
                model.DeliveryAgentDDList = objAPI.GetAllRecords<DeliveryAgentDD>("Customer", "GetDeliveryAgentDD");
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        public ActionResult GetOrderTrackingByID(string OrderID)
        {
            OrderTrackingVM model = new OrderTrackingVM();
            model.OrderManage = objAPI.GetRecordByQueryString<OrderManageView>("OrderManage", "GetOrderByID", "Id=" + OrderID);
            model.OrderTransList = objAPI.GetRecordsByQueryString<utblOrderTransDetail>("Customer", "GetOrderTransByID", "Id=" + OrderID);
            return PartialView("_pvOrderTracking", model);
        }
        [HttpGet]
        [Authorize(Roles = "Delivery Agent")]
        public ActionResult OrderAckByRider(string OrderID)
        {
            try
            {
                ViewBag.ActiveURL = "/general/ordermanage/ordermanagelist";
                RiderAckVM model = new RiderAckVM();
                model.OrderItems = objAPI.GetRecordsByQueryString<CheckOutItem>("Customer", "GetOrderItemsByID", "Id=" + OrderID);
                model.OrderManage = objAPI.GetRecordByQueryString<OrderManageView>("OrderManage", "GetOrderByID", "Id=" + OrderID);
                model.OrderTransList = objAPI.GetRecordsByQueryString<utblOrderTransDetail>("Customer", "GetOrderTransByID", "Id=" + OrderID);
                model.AddressView = objAPI.GetRecordByQueryString<AddressView>("Customer", "GetAddressDtlsByID", "Id=" + OrderID);
                model.OrderID = OrderID;
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            //return PartialView("_pvOrderDtlsView", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Delivery Agent")]
        public ActionResult OrderAckByRider(RiderAckVM model, string btnreq = "")
        {
            try
            {
                ViewBag.ActiveURL = "/general/ordermanage/ordermanagelist";
                if (string.IsNullOrEmpty(btnreq) || string.IsNullOrEmpty(model.OrderID))
                {
                    ModelState.AddModelError("", "");
                }
                if (ModelState.IsValid)
                {
                    var query = "OrderID=" + model.OrderID + "&UpdateStatus=" + btnreq;
                    string result = objAPI.PostRecordUsingQueryString("OrderManage", "OrderUpdateStatus", query);
                    if (!result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("OrderManageList", "OrderManage", new { Area = "General" });
                    }
                    TempData["ErrMsg"] = result;
                }
                model.OrderItems = objAPI.GetRecordsByQueryString<CheckOutItem>("Customer", "GetOrderItemsByID", "Id=" + model.OrderID);
                model.OrderManage = objAPI.GetRecordByQueryString<OrderManageView>("OrderManage", "GetOrderByID", "Id=" + model.OrderID);
                model.OrderTransList = objAPI.GetRecordsByQueryString<utblOrderTransDetail>("Customer", "GetOrderTransByID", "Id=" + model.OrderID);
                model.AddressView = objAPI.GetRecordByQueryString<AddressView>("Customer", "GetAddressDtlsByID", "Id=" + model.OrderID);
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }
    }
}