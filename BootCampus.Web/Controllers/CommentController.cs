using BootCampus.CMM.Notice.BSL;
using BootCampus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BootCampus.Web.Controllers
{
    public class CommentController : Controller
    {
        CommentBsl commentBsl = new CommentBsl();
        #region 댓글 생성
        public JsonResult Create(CommentModel comment)
        {
            ResultModel response = commentBsl.SaveComment(comment);

            return Json(response);
        }
        #endregion

        
        #region 댓글 수정
        public JsonResult Update(CommentModel comment)
        {
            ResultModel response = commentBsl.ModifyComment(comment);

            return Json(response);
        }
        #endregion

        #region 댓글 삭제
        public JsonResult Delete(int id)
        {
            ResultModel response = commentBsl.RemoveComment(id);

            return Json(response);
        }
        #endregion
    }
}