using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootCampus.CMM.Notice.BSL;
using BootCampus.Models;

namespace BootCampus.Web.Controllers
{
    public class MemberController : Controller
    {
        MemberBsl memberBsl = new MemberBsl();

        #region 로그인
        public JsonResult Login(MemberModel user)
        {
            MemberModel response = memberBsl.CheckUser(user);

            return Json(response);
        }
        #endregion
    }
}