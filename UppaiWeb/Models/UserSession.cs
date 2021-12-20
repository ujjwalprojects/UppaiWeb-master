using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace UppaiWeb.Models
{
    public interface IUserSession
    {
        string userId { get; }
        string role { get; }
    }
    public class UserSession : IUserSession
    {

        public string userId
        {
            get { return ((ClaimsPrincipal)HttpContext.Current.User).FindFirst("UserID").Value; }
        }

        public string role
        {
            get { return ((ClaimsPrincipal)HttpContext.Current.User).FindFirst("Role").Value; }
        }

    }
}