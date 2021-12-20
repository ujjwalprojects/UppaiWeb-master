using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UppaiWeb.Helpers;

namespace UppaiWeb.Areas.MasterConfig.Models
{
    #region Menu
    public class utblMenuDetail
    {
        [Key]
        public long MenuID { get; set; }
        [Required(ErrorMessage = "Enter Menu Name")]
        public string MenuName { get; set; }
        [Required(ErrorMessage = "Select Menu Category")]
        public long ItemCategoryID { get; set; }
        public bool IsPublish { get; set; }
        [Required(ErrorMessage = "Select Menu Type")]
        public string MenuType { get; set; }
        public string ImageThumbnail { get; set; }
        public string ImageNormal { get; set; }
        public string ImagesFile { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Enter Item Price")]
        public decimal OrginalPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string ServedUpto { get; set; }
        public int CustomerRating { get; set; }
        public bool IsAvailable { get; set; }
        public string AddedBy { get; set; }
        public bool IsAddedByAdmin { get; set; }
    }
    public class MenuDetailView
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
    }
    public class MenuAdd
    {
        public utblMenuDetail AddMenu { get; set; }
        public IEnumerable<MenuCategoryDD> MenuCategoryDDList { get; set; }
        public IEnumerable<PartnerDD> PartnerDDList { get; set; }
    }
    public class MenuEdit
    {
        public utblMenuDetail EditMenu { get; set; }
        public IEnumerable<MenuCategoryDD> MenuCategoryDDList { get; set; }
        public IEnumerable<PartnerDD> PartnerDDList { get; set; }
        public string PrevImg { get; set; }
    }
    public class PartnerDD
    {
        public string UserName { get; set; }
        public string PartnerName { get; set; }
    }
    public class MenuVM
    {
        public IEnumerable<MenuDetailView> MenuList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class MenuAPIVM
    {
        public IEnumerable<MenuDetailView> MenuList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class MenuDD
    {
        public long MenuID { get; set; }
        public string MenuName { get; set; }
    }
    #endregion
    #region Menu Non Avl
    public class utblMenuNonAvailableDay
    {
        [Key]
        public long AvailableID { get; set; }
        [Required(ErrorMessage = "Select Menu Name")]
        public long MenuID { get; set; }
        public int NotAvailableDays { get; set; }
        public string AddedBy { get; set; }
        //public DayOfWeek NonAvl { get; set; }
    }

    public class MenuNonAvlAdd
    {
        //public utblMenuNonAvailableDay MenuNonAvlDay { get; set; }
        public IEnumerable<MenuDD> MenuDDList { get; set; }
        //public List<DayOfWeekEnum> NonAvlDays { get; set; }
        [Required(ErrorMessage = "Select Menu Name")]
        public List<long> MenuSelect { get; set; }
        public string AddedBy { get; set; }
    }


    #endregion
}