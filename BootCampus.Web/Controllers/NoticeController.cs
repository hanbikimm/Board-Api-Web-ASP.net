using BootCampus.CMM.Notice.BSL;
using BootCampus.Models;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.Mvc;

namespace BootCampus.Web.Controllers
{
    public class NoticeController : Controller
    {
        NoticeBsl noticeBsl = new NoticeBsl();

        #region 상태 목록 조회
        public JsonResult StateList()
        {
            List<StateModel> stateList = noticeBsl.GetStateList();

            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 상태 수정
        public JsonResult State(NoticeModel notice)
        {
            ResultModel response = noticeBsl.ModifyState(notice);

            return Json(response);
        }
        #endregion

        #region 엑셀 다운로드
        public JsonResult ExcelDownLoad(SearchModel condition)
        {
            List<NoticeModel> response = noticeBsl.DownloadExcel(condition);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 목록 조회
        public JsonResult NoticeList(SearchModel condition)
        {
            List<NoticeModel> response = noticeBsl.GetNoticeList(condition);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 조회수 증가
        public JsonResult ViewCount(int id)
        {
            ResultModel response = noticeBsl.PlusViewCount(id);

            return Json(response);
        }
        #endregion

        #region 상세 조회
        public JsonResult Detail(int id)
        {
            NoticeModel response = noticeBsl.GetNoticeDetail(id);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 게시글 생성
        public JsonResult Create(NoticeModel notice)
        {
            ResultModel response = noticeBsl.SaveNotice(notice);

            return Json(response);
        }
        #endregion

        
        #region 게시글 수정
        public JsonResult Update(NoticeModel notice)
        {
            ResultModel response = noticeBsl.ModifyNotice(notice);

            return Json(response);
        }
        #endregion

        #region 게시글 삭제
        public JsonResult Delete(int id)
        {
            ResultModel response = noticeBsl.RemoveNotice(id);

            return Json(response);
        }
        #endregion
    }
}