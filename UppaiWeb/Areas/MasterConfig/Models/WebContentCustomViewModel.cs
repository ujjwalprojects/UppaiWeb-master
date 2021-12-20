using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UppaiWeb.Helpers;

namespace UppaiWeb.Areas.MasterConfig.Models
{
    public class utblBannerDetail
    {
        [Key]
        public long BannerID { get; set; }
        [Required(ErrorMessage = "Enter Banner Title")]
        public string BannerTitle { get; set; }
        public string BannerDesc { get; set; }
        public bool IsPublished { get; set; }
        public string BannerThumbnail { get; set; }
        public string BannerNormal { get; set; }
        public string BannerFilePath { get; set; }
        public string AddedBy { get; set; }
    }
    public class AddBanner
    {
        public utblBannerDetail BannerDetail { get; set; }
    }
    public class BannerVM
    {
        public IEnumerable<utblBannerDetail> BannerList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class BannerAPIVM
    {
        public IEnumerable<utblBannerDetail> BannerList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class EditBanner
    {
        public utblBannerDetail BannerDetail { get; set; }
    }

    #region CMS Content Model
    public class utblCMSContentDetail
    {
        [Key]
        public string CMSContentID { get; set; }
        [Required]
        public string CMSContentTitle { get; set; }
        public string CMSContent { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
    public class CMSContentVM
    {
        public utblCMSContentDetail CMSContent { get; set; }
        public IEnumerable<utblCMSContentDetail> CMSContentList { get; set; }
    }
    #endregion
}