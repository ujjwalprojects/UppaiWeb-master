using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UppaiWeb.Helpers;

namespace UppaiWeb.ViewModels
{
    public class CheckOutVM
    {
        public IEnumerable<CheckOutItem> CheckOutItems { get; set; }
        public utblDeliveryAddr DeliverTo { get; set; }
        public string OrderFrom { get; set; }
        public PartnerLocation PartnerLocation { get; set; }
        public utblOrderDetail AddEditOrder { get; set; }
        //public DeliveryTypeModel DeliveryType { get; set; }
        //public PartnerFare PartnerFare { get; set; }
    }
    public class AddressVM
    {
        public List<utblDeliveryAddr> DeliveryAddress { get; set; }
        public utblDeliveryAddr AddAddress { get; set; }
    }
    public class utblDeliveryAddr
    {
        [Key]
        public long AddressID { get; set; }
        [Required(ErrorMessage = "Contact Person Name fields are required")]
        public string ContactPersonName { get; set; }
        [Required(ErrorMessage = "Contact Number fields are required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered Contact Number format is not valid.")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Flat, Floor, Building fields are required")]
        public string FlatFloorBuilding { get; set; }
        [Required(ErrorMessage = "Landmark fields are required")]
        public string Landmark { get; set; }
        [Required(ErrorMessage = "Postal Pin fields are required")]
        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessage = "Please Enter Valid Postal Code.")]
        public string PinNo { get; set; }
        [Required(ErrorMessage = "Address fields are required")]
        public string FullAddress { get; set; }
        [Required(ErrorMessage = "Select Address type")]
        public string AddressType { get; set; }
        public bool IsSetDefault { get; set; }
        public string AddedBy { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LatLngBoth { get; set; }
    }
    public class DeliveryTypeModel
    {
        [Required(ErrorMessage = "Select Delivery Type")]
        public string DeliveryType { get; set; }
        public DateTime? SheduleDate { get; set; }
        public string DeliveryInstruction { get; set; }
    }
    public class PartnerLocation
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LatLngBoth { get; set; }
        public string PartnerID { get; set; }
        public string PartnerName { get; set; }
    }
    public class PartnerFare
    {
        public Int32 BaseDistance { get; set; }
        public decimal BaseFare { get; set; }
        public decimal PerKMFare { get; set; }
        public float? TotalDistance { get; set; }
        public bool ISSurcharge { get; set; }
        public decimal TotalFee { get; set; }
    }
    public class utblOrderDetail
    {
        [Key]
        public string OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Select Delivery Type")]
        public string OrderType { get; set; }
        public DateTime? OrderSchedule { get; set; }
        public string DeliveryInstruction { get; set; }
        public long AddressID { get; set; }
        public string PartnerID { get; set; }
        public string CouponID { get; set; }
        public decimal NetAmt { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalAmt { get; set; }
        public string OrderBy { get; set; }
        public bool IsOrderConfirmed { get; set; }
        public bool IsPrePaid { get; set; }
    }
    public class OrderDetailView
    {
        public string OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderType { get; set; }
        public DateTime? OrderSchedule { get; set; }
        public string DeliveryInstruction { get; set; }
        public long AddressID { get; set; }
        public string PartnerID { get; set; }
        public string CouponID { get; set; }
        public decimal NetAmt { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalAmt { get; set; }
        public string OrderBy { get; set; }
        public bool IsOrderConfirmed { get; set; }
        public bool IsPrePaid { get; set; }
        public string ShipTo { get; set; }
        public string PartnerName { get; set; }
        public string CouponDtls { get; set; }
    }

    public class OrderVM
    {
        public IEnumerable<OrderDetailView> OrderList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class OrderAPIVM
    {
        public IEnumerable<OrderDetailView> OrderList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class OrderDtlsVM
    {
        public OrderDetailView OrderDtls { get; set; }
        public IEnumerable<CheckOutItem> CheckOutItems { get; set; }
    }
    public class utblOrderTransDetail
    {
        public string TransID { get; set; }
        public string OrderID { get; set; }
        public DateTime TransDate { get; set; }
        public string OrderStatus { get; set; }
        public string TransDescription { get; set; }
    }
    public class OrderTrackVM
    {
        public IEnumerable<utblOrderTransDetail> OrderTransList { get; set; }
        public OrderDetailView OrderDtls { get; set; }
    }

    public class utblPaymentTran
    {
        public string PaymentTransID { get; set; }
        public string OrderID { get; set; }
        public string RPayOrderID { get; set; }
        public string PaymentType { get; set; }
        public int PaymentAmt { get; set; }
        public string Currency { get; set; }
        public string PaymentStatus { get; set; }
        public bool IsSuccess { get; set; }
        public string MerchantName { get; set; }
    }

    public class DeliveryAgentDD
    {
        public string DeliveryAgentID { get; set; }
        public string RiderName { get; set; }
    }
}