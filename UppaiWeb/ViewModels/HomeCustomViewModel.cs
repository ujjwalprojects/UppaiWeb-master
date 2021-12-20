using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UppaiWeb.Areas.MasterConfig.Models;

namespace UppaiWeb.ViewModels
{
    //public class BusinessCategory { get; set; }
    public class HomeVM
    {
        public IEnumerable<BCategoryDD> BizCategotyList { get; set; }
        public IEnumerable<utblBannerDetail> BannerList { get; set; }
        public IEnumerable<PartnerDtlsView> PartnerList { get; set; }
    }
    public class MenuDtlsVM
    {
        public PartnerDtlsView PartnerDtls { get; set; }
        public IEnumerable<MenuDtlsView> MenuList { get; set; }
        public IEnumerable<MenuCategory> MenuCategoryList { get; set; }
        public IEnumerable<CheckOutItem> CheckOutItems { get; set; }
        //public IEnumerable<MenuDtls> MenuDtlsList { get; set; }
        public List<string> OpenRangeDays { get; set; }
        public string OrdFrom { get; set; }
    }
    public class PartnerDtlsView
    {
        public string PartnerID { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public string ImagesUpload { get; set; }
        public string Address { get; set; }
        public string OpeningTime { get; set; }
        public string ClosedTime { get; set; }
        public long BusinessCategoryID { get; set; }
        public string OperationDay { get; set; }
        public string TodayIsClosed { get; set; }
    }
    public class MenuCategory
    {
        public long ItemCategoryID { get; set; }
        public string ItemCategoryName { get; set; }
    }
    public class MenuDtlsView
    {
        public long MenuID { get; set; }
        public string MenuName { get; set; }
        public long ItemCategoryID { get; set; }
        public string ItemCategoryName { get; set; }
        public bool IsPublish { get; set; }
        public string MenuType { get; set; }
        public string ImageThumbnail { get; set; }
        public string ImageNormal { get; set; }
        public string ImagesFile { get; set; }
        public string Description { get; set; }
        public decimal OrginalPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string ServedUpto { get; set; }
        public int CustomerRating { get; set; }
        public bool IsAvailable { get; set; }
        public string AddedBy { get; set; }
        public bool IsAddedByAdmin { get; set; }
        public int DiscountPercentage { get; set; }
    }
    public class PartnerDtlsVM
    {
        public IEnumerable<BCategoryDD> BizCategotyList { get; set; }
        public IEnumerable<PartnerDtlsView> PartnerList { get; set; }
    }
    public class MenuDtls
    {
        public long MenuID { get; set; }
        public string MenuName { get; set; }
        public decimal Price { get; set; }
        public string MenuType { get; set; }
        public string ImageThumbnail { get; set; }
        public string AddedBy { get; set; }
        public string PartnerID { get; set; }
    }
    public class CheckOutItem
    {
        public long MenuID { get; set; }
        public string MenuName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal TotalAmt { get; set; }
        public string MenuType { get; set; }
        public string ImageThumbnail { get; set; }
        public string AddedBy { get; set; }
        public string PartnerID { get; set; }
    }
    public class utblShoppingCart
    {
        [Key]
        public string CartID { get; set; }
        public long MenuID { get; set; }
        public int Qty { get; set; }
        public string AddedBy { get; set; }
    }

    //public class CMSContentDtlsVM
    //{
    //    public utblCMSContentDetail CMSContent { get; set; }
    //}

    public class CMSContentView
    {
        public string CMSContentID { get; set; }
        public string CMSContentTitle { get; set; }
        public string CMSContent { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }

}