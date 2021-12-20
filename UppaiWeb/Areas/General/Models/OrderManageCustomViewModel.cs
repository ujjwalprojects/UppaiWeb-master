using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UppaiWeb.Helpers;
using UppaiWeb.ViewModels;

namespace UppaiWeb.Areas.General.Models
{
    public class OrderManageVM
    {
        public IEnumerable<OrderManageView> OrderManageList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class OrderManageAPIVM
    {
        public IEnumerable<OrderManageView> OrderManageList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class OrderAckVM
    {
        public OrderManageView OrderManage { get; set; }
        public IEnumerable<CheckOutItem> OrderItems { get; set; }
        public OrderAckModel OrderAck { get; set; }
    }
    public class OrderAckModel
    {
        public string OrderID { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }
    public class OrderManageView
    {
        public string OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderType { get; set; }
        public DateTime? OrderSchedule { get; set; }
        public string DeliveryInstruction { get; set; }
        public long AddressID { get; set; }
        public string DeliverTo { get; set; }
        public string PartnerID { get; set; }
        public string PartnerName { get; set; }
        public string CouponID { get; set; }
        public string CouponDtls { get; set; }
        public decimal NetAmt { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalAmt { get; set; }
        public string OrderBy { get; set; }
        public bool IsOrderConfirmed { get; set; }
        public bool IsPrePaid { get; set; }
        public string LatestStatus { get; set; }
        public string AssignTo { get; set; }
    }

    public class OrderAssignVM
    {
        public OrderManageView OrderManage { get; set; }
        public IEnumerable<CheckOutItem> OrderItems { get; set; }
        public utblOrderAssignDetail OrderAssign { get; set; }
        public IEnumerable<DeliveryAgentDD> DeliveryAgentDDList { get; set; }
        public string OrderID { get; set; }
    }
    public class utblOrderAssignDetail
    {
        [Key]
        public long AssignID { get; set; }
        public string OrderID { get; set; }
        public DateTime AssignDateTime { get; set; }
        [Required(ErrorMessage = "Select Delivery Agent")]
        public string DeliveryAgentID { get; set; }
        public bool IsEngage { get; set; }
        public string Status { get; set; }
    }
    public class OrderTrackingVM
    {
        public IEnumerable<utblOrderTransDetail> OrderTransList { get; set; }
        public OrderManageView OrderManage { get; set; }
    }
    public class RiderAckVM
    {
        public IEnumerable<utblOrderTransDetail> OrderTransList { get; set; }
        public IEnumerable<CheckOutItem> OrderItems { get; set; }
        public OrderManageView OrderManage { get; set; }
        public AddressView AddressView { get; set; }
        public string OrderID { get; set; }
    }
    public class AddressView
    {
        public string OrderID { get; set; }
        public string PartnerName { get; set; }
        public string PartnerNo { get; set; }
        public string PartnerAddr { get; set; }
        public string PartnerLat { get; set; }
        public string PartnerLong { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerAddr { get; set; }
        public string Landmark { get; set; }
        public string CustomerLat { get; set; }
        public string CustomerLong { get; set; }
    }

}