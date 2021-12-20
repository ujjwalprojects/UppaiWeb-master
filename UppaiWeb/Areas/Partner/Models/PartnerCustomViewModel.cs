using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UppaiWeb.Areas.MasterConfig.Models;
using UppaiWeb.Helpers;
using UppaiWeb.Models;

namespace UppaiWeb.Areas.Partner.Models
{

    public class utblPartnerDetail
    {
        [Key]
        public string PartnerID { get; set; }
        [Required(ErrorMessage = "Enter Partner Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Contact No.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Enter User Email ID")]
        [EmailAddress(ErrorMessage = "Enter Valid Email ID")]
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public string Id { get; set; }
        public string ImagesUpload { get; set; }
        [Required(ErrorMessage = "Enter Partner Address")]
        public string Address { get; set; }
        public string OpeningTime { get; set; }
        public string ClosedTime { get; set; }
        public string BankName { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Account No must be Number")]
        //[RegularExpression("[^0-9]", ErrorMessage = "Account No must be Number")]
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string ACHolderName { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string AlternateMobileNo { get; set; }
        public string CoverPhoto { get; set; }
        public long BusinessCategoryID { get; set; }
        public string AddedBy { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    public class PartnerAdd
    {
        public utblPartnerDetail PartnerDetails { get; set; }
        public IEnumerable<BCategoryDD> BCategoryDDList { get; set; }
    }
    public class PartnerView
    {
        public string PartnerID { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public string Id { get; set; }
        public string ImagesUpload { get; set; }
        public string Address { get; set; }
        public string OpeningTime { get; set; }
        public string ClosedTime { get; set; }
        public string BankName { get; set; }
        public long? AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string ACHolderName { get; set; }
        public string AlternateMobileNo { get; set; }
        public string CoverPhoto { get; set; }
        public long BusinessCategoryID { get; set; }
        public string BusinessCategoryName { get; set; }
        //AspNetUser
        public string RoleName { get; set; }
        public bool EmailConfirmed { get; set; }
        //Add new Field for Calculate distance
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    public class PartnerVM
    {
        public IEnumerable<PartnerView> PartnerList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public List<DayOfWeekEnum> OperationalDays { get; set; }
        public List<string> OpenRangeDays { get; set; }
    }
    public class PartnerAPIVM
    {
        public IEnumerable<PartnerView> PartnerList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class PartnerEdit
    {
        public utblPartnerDetail PartnerDetails { get; set; }
        public IEnumerable<BCategoryDD> BCategoryDDList { get; set; }
        public List<DayOfWeekEnum> OperationalDays { get; set; }
        public string RoleName { get; set; }
        public List<string> OpenRangeDays { get; set; }
    }
    public class DayOfWeekEnum
    {
        public int DayNo { get; set; }
        public string DayName { get; set; }
        public bool IsSelected { get; set; }
    }

}