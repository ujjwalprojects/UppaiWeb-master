using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Helpers;
using UppaiWeb.ViewModels;

namespace UppaiWeb.Controllers
{
    public class CustomerController : BaseController
    {
        ApiConnection objGenAPI = new ApiConnection("general");
        string key = "8080808080808080";
        string iv = "8080808080808080";
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult CheckOut(long AddrID = 0,string OrdID="")
        {
            try
            {
                CheckOutVM model = new CheckOutVM();
                if (Session["CheckOutItems"] == null)
                    model.CheckOutItems = objGenAPI.GetRecordsByQueryString<CheckOutItem>("Customer", "GetCheckOutItems", "Id=" + User.Identity.Name);
                else
                    model.CheckOutItems = Session["CheckOutItems"] as List<CheckOutItem>;
                if (model.CheckOutItems.Count() > 0)
                {
                    if (AddrID != 0)
                        model.DeliverTo = objGenAPI.GetRecordByQueryString<utblDeliveryAddr>("Customer", "GetAddrDtlsByID", "Id=" + AddrID);
                    else
                        model.DeliverTo = objGenAPI.GetRecordByQueryString<utblDeliveryAddr>("Customer", "GetDefaultAddr", "Id=" + User.Identity.Name);
                    model.PartnerLocation = objGenAPI.GetRecordByQueryString<PartnerLocation>("Customer", "GetPartnerLoc", "ID=" + model.CheckOutItems.ElementAt(0).PartnerID);
                    model.OrderFrom = model.CheckOutItems.ElementAt(0).AddedBy;
                    model.AddEditOrder = new utblOrderDetail();
                    {
                        model.AddEditOrder.AddressID = model.DeliverTo.AddressID;
                        model.AddEditOrder.PartnerID = model.PartnerLocation.PartnerID;
                        model.AddEditOrder.OrderBy = User.Identity.Name;
                        model.AddEditOrder.OrderID = OrdID;
                    }
                    return View(model);
                }
                else
                {
                    Session["CheckOutItems"] = null;
                    return RedirectToAction("PartnerList", "home", new { Area = "" });
                }
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        #region Order Details
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult CheckOut(CheckOutVM model)
        {
            try
            {
                if (model.AddEditOrder.OrderType == "Delivery Schedule" && model.AddEditOrder.OrderSchedule == null)
                {
                    ModelState.AddModelError("AddEditOrder.OrderSchedule", "Schedule Delivery Date & Time");
                }
                model.CheckOutItems = Session["CheckOutItems"] as List<CheckOutItem>;
                if (ModelState.IsValid)
                {
                    string jsonStr = JsonConvert.SerializeObject(model);
                    string result = objGenAPI.PostRecordtoApI("Customer", "AddOrder", jsonStr);
                    string EncOrderID = AES.Encrypt(result, key, iv);
                    if (!result.Contains("Error"))
                    {
                        //Session["CheckOutItems"] = null;
                        TempData["ErrMsg"] = "Success:Proceed for Order Placed Confirmed";
                        return RedirectToAction("PaymentMode", "customer", new { Area = "", OrderID = EncOrderID });
                    }
                    TempData["ErrMsg"] = result;
                }
                model.DeliverTo = objGenAPI.GetRecordByQueryString<utblDeliveryAddr>("Customer", "GetAddrDtlsByID", "Id=" + model.AddEditOrder.AddressID);
                model.PartnerLocation = objGenAPI.GetRecordByQueryString<PartnerLocation>("Customer", "GetPartnerLoc", "ID=" + model.AddEditOrder.PartnerID);
                model.OrderFrom = model.CheckOutItems.ElementAt(0).AddedBy;
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        #endregion

        #region Payment Mode
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult PaymentMode(string OrderID = "")
        {
            try
            {
                utblOrderDetail model = new utblOrderDetail();
                string DecOrderID = AES.Decrypt(OrderID, key, iv);
                model = objGenAPI.GetRecordByQueryString<utblOrderDetail>("Customer", "GetOrderDtlsByID", "Id=" + DecOrderID);
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult PaymentMode(string OrdID, bool IsPrePaid, string OrdType)
        {
            try
            {
                string EncOrderID = AES.Encrypt(OrdID, key, iv);
                // Generate random receipt number for order
                Random randomObj = new Random();
                string transactionId = randomObj.Next(10000000, 100000000).ToString();
                //Get Order Details By Order ID
                utblOrderDetail model = objGenAPI.GetRecordByQueryString<utblOrderDetail>("Customer", "GetOrderDtlsByID", "Id=" + OrdID);
                if (OrdType == "Delivery Schedule" && !IsPrePaid)
                {
                    ModelState.AddModelError("ErrorMsg", "COD Option not available on Delivery Schedule.");
                }
                if (ModelState.IsValid)
                {
                    if (IsPrePaid)
                    {
                        Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_mG2hLdgNv5THmp", "leZQSOYvPcvoaoPVvfTAsOdB");
                        Dictionary<string, object> options = new Dictionary<string, object>();
                        options.Add("amount", model.TotalAmt * 100);  // Amount will in paise
                        options.Add("receipt", transactionId);
                        options.Add("currency", "INR");
                        options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                                                             //options.Add("notes", "-- You can put any notes here --");
                        Razorpay.Api.Order orderResponse = client.Order.Create(options);
                        string orderId = orderResponse["id"].ToString();
                        // Create PaymentTrans model for return on view
                        utblPaymentTran PaymentTrans = new utblPaymentTran
                        {
                            PaymentTransID = transactionId,
                            OrderID = model.OrderID,
                            RPayOrderID = orderResponse.Attributes["id"],
                            PaymentType = "PrePaid",
                            PaymentAmt = Convert.ToInt32(model.TotalAmt) * 100,
                            Currency = "INR",
                            IsSuccess = true,
                            MerchantName = "Uppai"
                        };
                        //Session["CheckOutItems"] = null;
                        //return RedirectToAction("PaymentGateway", "customer", new { Area = "", OrderID = EncOrderID });
                        string jsonStr = JsonConvert.SerializeObject(PaymentTrans);
                        string result = objGenAPI.PostRecordtoApI("Customer", "OrderConfirmed", jsonStr);
                        if (!result.Contains("Error"))
                        {
                            return View("PaymentPage", PaymentTrans);
                        }
                        TempData["ErrMsg"] = result;
                    }
                    else
                    {
                        utblPaymentTran PaymentTrans = new utblPaymentTran
                        {
                            PaymentTransID = transactionId,
                            OrderID = model.OrderID,
                            RPayOrderID = null,
                            PaymentType = "COD",
                            PaymentAmt = Convert.ToInt32(model.TotalAmt) * 100,
                            Currency = "INR",
                            IsSuccess = true,
                            MerchantName = "Uppai"
                        };
                        string jsonStr = JsonConvert.SerializeObject(PaymentTrans);
                        //var query = "OrderID=" + OrdID + "&IsPrePaid=" + IsPrePaid;
                        //string result = objGenAPI.PostRecordUsingQueryString("Customer", "OrderConfirmed", query);
                        string result = objGenAPI.PostRecordtoApI("Customer", "OrderConfirmed", jsonStr);
                        if (!result.Contains("Error"))
                        {
                            Session["CheckOutItems"] = null;
                            TempData["ErrMsg"] = "Success:Order has been placed";
                            return RedirectToAction("OrderTrack", "customer", new { Area = "", OrderID = EncOrderID });
                        }
                        TempData["ErrMsg"] = result;
                    }
                }
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        [HttpPost]
        public ActionResult Complete()
        {
            // Payment data comes in url so we have to get it from url

            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = Request.Params["rzp_paymentid"];

            // This is orderId
            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_mG2hLdgNv5THmp", "leZQSOYvPcvoaoPVvfTAsOdB");

            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];

            //// Check payment made successfully

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                // Create these action method
                return RedirectToAction("Complete","Customer", new { Area = "" });
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult PaymentGateway(string OrderID = "")
        {
            try
            {
                utblOrderDetail model = new utblOrderDetail();
                string DecOrderID = AES.Decrypt(OrderID, key, iv);
                ViewBag.OrderID = OrderID;
                model = objGenAPI.GetRecordByQueryString<utblOrderDetail>("Customer", "GetOrderDtlsByID", "Id=" + DecOrderID);
                return View(model);

            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        #endregion
        #region Order History
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult OrderList(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            try
            {
                OrderVM model = new OrderVM();
                ViewBag.SearchTerm = SearchTerm;
                var query = "PageNo=" + PageNo + "&PageSize=" + PageSize + "&SearchTerm=" + SearchTerm + "&UserID=" + User.Identity.Name;
                OrderAPIVM Apimodel = objGenAPI.GetRecordByQueryString<OrderAPIVM>("Customer", "GetOrderHistory", query);
                model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = Apimodel.TotalRecords };
                model.OrderList = Apimodel.OrderList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_pvOrderList", model);
                }
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        public ActionResult GetOrderDetailsByID(string OrderID)
        {
            OrderDtlsVM model = new OrderDtlsVM();
            model.CheckOutItems = objGenAPI.GetRecordsByQueryString<CheckOutItem>("Customer", "GetOrderItemsByID", "Id=" + OrderID);
            model.OrderDtls = objGenAPI.GetRecordByQueryString<OrderDetailView>("Customer", "GetOrderDtlsByID", "Id=" + OrderID);
            return PartialView("_pvOrderDtlsView", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Authorize(Roles = "Customer")]
        public ActionResult RepeatOrder(string id, long addID)
        {
            try
            {
                Session["CheckOutItems"] = null;
                List<CheckOutItem> ItemOutList = objGenAPI.GetRecordsByQueryString<CheckOutItem>("Customer", "GetOrderItemsByID", "Id=" + id);
                Session["CheckOutItems"] = ItemOutList;
                return RedirectToAction("CheckOut", "customer", new { Area = "", AddrID = addID });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult OrderTrack(string OrderID)
        {
            try
            {
                OrderTrackVM model = new OrderTrackVM();
                string DecOrderID = AES.Decrypt(OrderID, key, iv);
                model.OrderDtls = objGenAPI.GetRecordByQueryString<OrderDetailView>("Customer", "GetOrderDtlsByID", "Id=" + DecOrderID);
                model.OrderTransList = objGenAPI.GetRecordsByQueryString<utblOrderTransDetail>("Customer", "GetOrderTransByID", "Id=" + DecOrderID);
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        #endregion

        #region Delivery Address
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult AddAddress()
        {
            try
            {
                List<CheckOutItem> objsession = new List<CheckOutItem>();
                int SessionCount = 0;
                if (Session["CheckOutItems"] != null)
                {
                    objsession = Session["CheckOutItems"] as List<CheckOutItem>;
                    SessionCount = objsession.Sum(x => x.Qty);
                }
                ViewBag.LayoutCart = SessionCount;
                AddressVM model = new AddressVM();
                model.AddAddress = new utblDeliveryAddr();
                model.DeliveryAddress = objGenAPI.GetRecordsByQueryString<utblDeliveryAddr>("Customer", "GetDeliveryAddr", "Id=" + User.Identity.Name);
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult AddAddress(AddressVM model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.AddAddress.AddressType))
                {
                    ModelState.AddModelError("AddAddress.AddressType", "Select Address Type");
                }
                if (ModelState.IsValid)
                {
                    model.AddAddress.AddedBy = User.Identity.Name;
                    model.AddAddress.IsSetDefault = true;
                    string jsonStr = JsonConvert.SerializeObject(model.AddAddress);
                    string result = objGenAPI.PostRecordtoApI("Customer", "AddAddress", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("AddAddress", "Customer", new { Area = "" });
                    }
                    TempData["ErrMsg"] = result;
                }
                model.DeliveryAddress = objGenAPI.GetRecordsByQueryString<utblDeliveryAddr>("Customer", "GetDeliveryAddr", "Id=" + User.Identity.Name); ;
                TempData["ErrMsg"] = "Error: Operation Failed. Please Check and try again";
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult EditAddress(long id)
        {
            try
            {
                List<CheckOutItem> objsession = new List<CheckOutItem>();
                int SessionCount = 0;
                if (Session["CheckOutItems"] != null)
                {
                    objsession = Session["CheckOutItems"] as List<CheckOutItem>;
                    SessionCount = objsession.Sum(x => x.Qty);
                }
                ViewBag.LayoutCart = SessionCount;
                AddressVM model = new AddressVM();
                model.AddAddress = objGenAPI.GetRecordByQueryString<utblDeliveryAddr>("Customer", "GetAddrDtlsByID", "Id=" + id);
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult EditAddress(AddressVM model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.AddAddress.AddressType))
                {
                    ModelState.AddModelError("AddAddress.AddressType", "Select Address Type");
                }
                if (ModelState.IsValid)
                {
                    model.AddAddress.AddedBy = User.Identity.Name;
                    model.AddAddress.IsSetDefault = true;
                    string jsonStr = JsonConvert.SerializeObject(model.AddAddress);
                    string result = objGenAPI.PostRecordtoApI("Customer", "AddAddress", jsonStr);
                    if (result.Contains("Success"))
                    {
                        TempData["ErrMsg"] = result;
                        return RedirectToAction("AddAddress", "Customer", new { Area = "" });
                    }
                    TempData["ErrMsg"] = result;
                }
                model.DeliveryAddress = objGenAPI.GetRecordsByQueryString<utblDeliveryAddr>("Customer", "GetDeliveryAddr", "Id=" + User.Identity.Name); ;
                TempData["ErrMsg"] = "Error: Operation Failed. Please Check and try again";
                return View(model);
            }
            catch (AuthorizationException)
            {
                return RedirectToAction("GeneralLogin", "Account", new { Area = "" });
            }
        }
        #endregion
        #region Calculate Delivery Fee In JSON
        [HttpGet]
        public JsonResult GetDeliveryFee(string Dist)
        {
            PartnerFare objfare = new PartnerFare();
            char[] MyChar = { 'k', 'm', ' ' };
            string NewDist = Dist.TrimEnd(MyChar);
            objfare = objGenAPI.GetRecordByQueryString<PartnerFare>("Customer", "GetDeliverFee", "Dist=" + float.Parse(NewDist));
            objfare.TotalDistance = float.Parse(NewDist);
            return Json(objfare, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}