using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UppaiWeb.Models;

namespace UppaiWeb.Controllers
{
    public class HomeBaseController : Controller
    {
        protected SessionModel _sModel;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _sModel = Session["SessionVar"] as SessionModel;

            if (_sModel != null)
            {
                ViewBag.CurrentUserName = _sModel.UserName;
                ViewBag.ProfileName = _sModel.ProfileName;
                ViewBag.Role = _sModel.Role;

                if (_sModel.UserImage != "")
                {
                    ViewBag.UserImageSrc = _sModel.UserImage;
                }
            }
            else
            {
                ViewBag.CurrentUserName = "";
                
            }

        }
    }
}