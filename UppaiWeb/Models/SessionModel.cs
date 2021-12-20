using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UppaiWeb.Models
{
    public class SessionModel
    {
        public string AccessToken { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserImage { get; set; }
        public string Role { get; set; }
        public string ProfileName { get; set; }
    }
    //public class ChkOutCountModel
    //{
    //    public int ChkOutCount { get; set; }
    //}
   
}