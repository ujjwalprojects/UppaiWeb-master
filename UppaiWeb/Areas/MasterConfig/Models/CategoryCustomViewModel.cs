using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UppaiWeb.Helpers;

namespace UppaiWeb.Areas.MasterConfig.Models
{
    #region Business category
    public class utblMstBusinessCategorie
    {
        [Key] 
        public long BusinessCategoryID { get; set; }
        [Required(ErrorMessage ="Enter Business Category")]
        public string BusinessCategoryName { get; set; }
        public string AddedBy { get; set; }
    }
    public class BusinessCategoryAdd
    {
        public utblMstBusinessCategorie BusinessCategory { get; set; }
    }
    public class BusinessCategoryVM
    {
        public IEnumerable<utblMstBusinessCategorie> BusinessCategoryList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class BusinessCategoryAPIVM
    {
        public IEnumerable<utblMstBusinessCategorie> BusinessCategoryList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class BCategoryDD
    {
        public long BusinessCategoryID { get; set; }
        public string BusinessCategoryName { get; set; }
    }
    #endregion
    #region Menu category
    public class utblMenuItemCategorie
    {
        [Key]
        public long ItemCategoryID { get; set; }
        [Required(ErrorMessage = "Enter Menu Category")]
        public string ItemCategoryName { get; set; }
        public string AddedBy { get; set; }
    }
    public class MenuCategoryAdd
    {
        public utblMenuItemCategorie MenuCategory { get; set; }
    }
    public class MenuCategoryVM
    {
        public IEnumerable<utblMenuItemCategorie> MenuCategoryList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class MenuCategoryAPIVM
    {
        public IEnumerable<utblMenuItemCategorie> MenuCategoryList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class MenuCategoryDD
    {
        public long ItemCategoryID { get; set; }
        public string ItemCategoryName { get; set; }
    }
    #endregion
}