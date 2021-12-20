using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UppaiWeb.Helpers;

namespace UppaiWeb.Areas.DeliveryAgent.Models
{
    public class utblDeliveryAgent
    {
        [Key]
        public string DeliveryAgentID { get; set; }
        [Required(ErrorMessage = "Enter DeliveryAgent Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Contact No.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string MobileNo { get; set; }
        //[Required(ErrorMessage = "Enter User Email ID")]
        [EmailAddress(ErrorMessage = "Enter Valid Email ID")]
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public string Id { get; set; }
        public string ImagesUpload { get; set; }
        [Required(ErrorMessage = "Enter DeliveryAgent Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Select Delivery Type")]
        public string DeliveryType { get; set; }
        public string BankName { get; set; }
        public long? AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string ACHolderName { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string AlternateMobileNo { get; set; }
        public string AddedBy { get; set; }
    }
    public class DeliveryAgentAdd
    {
        public utblDeliveryAgent DeliveryAgentDetails { get; set; }
    }
    public class DeliveryAgentView
    {
        public string DeliveryAgentID { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public string Id { get; set; }
        public string ImagesUpload { get; set; }
        public string Address { get; set; }
        public string DeliveryType { get; set; }
        public string BankName { get; set; }
        public long? AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string ACHolderName { get; set; }
        public string AlternateMobileNo { get; set; }
        //AspNetUser
        public string RoleName { get; set; }
        public bool EmailConfirmed { get; set; }


    }
    public class DeliveryAgentVM
    {
        public IEnumerable<DeliveryAgentView> DeliveryAgentList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class DeliveryAgentAPIVM
    {
        public IEnumerable<DeliveryAgentView> DeliveryAgentList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class DeliveryAgentEdit
    {
        public utblDeliveryAgent DeliveryAgentDetails { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}