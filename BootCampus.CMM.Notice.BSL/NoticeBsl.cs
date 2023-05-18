using BootCampus.CMM.Notice.DSL;
using BootCampus.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Transactions;

namespace BootCampus.CMM.Notice.BSL
{
    public class NoticeBsl
    {
        NoticeDsl noticeDsl = new NoticeDsl();
        CommentDsl commentDsl = new CommentDsl();

        #region State List 조회
        /// <summary>
        /// 프론트 Select에 사용할 상태 목록을 가져옴
        /// </summary>
        /// <returns></returns>
        public List<StateModel> GetStateList()
        {
            List<StateModel> result = new List<StateModel>();

            DataTable stateTable = noticeDsl.SelectStateList();
            foreach(DataRow state in stateTable.Rows)
            {
                StateModel stateModel = new StateModel();
                stateModel.Success = true;
                stateModel.Message = "State 목록 조회 성공";
                stateModel.STATE_SEQ = Convert.ToInt32(state["STATE_SEQ"]);
                stateModel.STATE_TYPE = state["STATE_TYPE"].ToString();

                result.Add(stateModel);
            }

            return result;
        }
        #endregion

        #region State 수정
        /// <summary>
        /// 상태값을 수정하고, 그에 맞는 실행 결과를 반환해줌
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ResultModel ModifyState(NoticeModel notice)
        {
            ResultModel result = new ResultModel();

            bool updateResult = noticeDsl.UpdateState(notice);
            result.Success = updateResult;
            if (updateResult == true)
                result.Message = "게시글 상태 수정 성공";
            else
                result.Message = "게시글 상태 수정 실패";

            return result;
        }
        #endregion

        #region 엑셀 다운로드
        /// <summary>
        /// 페이징 처리되지 않은, 검색 조건에 맞는 전체 목록을 조회해 데이터를 가져옴
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<NoticeModel> DownloadExcel(SearchModel condition)
        {
            List<NoticeModel> result = new List<NoticeModel>();

            DataTable noticeTable = noticeDsl.SelectNoticeListForExcel(condition);
            if (noticeTable.Rows.Count > 0)
            {
                foreach (DataRow notice in noticeTable.Rows)
                {
                    NoticeModel noticeModel = new NoticeModel();

                    noticeModel.Success = true;
                    noticeModel.Message = "게시글 목록 조회 성공";
                    noticeModel.search = condition;

                    noticeModel.BOARD_SEQ = Convert.ToInt32(notice["BOARD_SEQ"]);
                    noticeModel.STATE = notice["STATE"].ToString();
                    noticeModel.TITLE = notice["TITLE"].ToString();
                    noticeModel.USER_NAME = notice["USER_NAME"].ToString();
                    noticeModel.WRITE_DATE = Convert.ToDateTime(notice["WRITE_DATE"]).ToString("yyyy-MM-dd");
                    noticeModel.VIEW_COUNT = Convert.ToInt32(notice["VIEW_COUNT"]);
                    noticeModel.REPLY_COUNT = Convert.ToInt32(notice["REPLY_COUNT"]);

                    result.Add(noticeModel);
                }
            }
            else
            {
                NoticeModel noticeModel = new NoticeModel();

                noticeModel.Success = false;
                noticeModel.Message = "게시글 목록이 없거나 조회 실패";

                result.Add(noticeModel);
            }

            return result;
        }
        #endregion

        #region Notice List 조회
        /// <summary>
        /// 검색 조건에 맞는 리스트를 가져와 검색조건, 페이지, 게시글 목록 정보를 가져옴
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<NoticeModel> GetNoticeList(SearchModel condition)
        {
            List<NoticeModel> result = new List<NoticeModel>();

            DataSet noticeListSet = noticeDsl.SelectNoticeList(condition);
            DataTable noticeTable = noticeListSet.Tables[0];
            if(noticeTable.Rows.Count > 0)
            {
                foreach (DataRow notice in noticeTable.Rows)
                {
                    NoticeModel noticeModel = new NoticeModel();

                    noticeModel.Success = true;
                    noticeModel.Message = "게시글 목록 조회 성공";

                    noticeModel.search = condition;
                    noticeModel.search.TOTAL_PAGE = Convert.ToInt32(noticeListSet.Tables[1].Rows[0]["TOTAL_PAGE"]);

                    noticeModel.BOARD_SEQ = Convert.ToInt32(notice["BOARD_SEQ"]);
                    noticeModel.STATE = notice["STATE"].ToString();
                    noticeModel.TITLE = notice["TITLE"].ToString();
                    noticeModel.USER_NAME = notice["USER_NAME"].ToString();
                    noticeModel.WRITE_DATE = Convert.ToDateTime(notice["WRITE_DATE"]).ToString("yyyy-MM-dd");
                    noticeModel.VIEW_COUNT = Convert.ToInt32(notice["VIEW_COUNT"]);
                    noticeModel.REPLY_COUNT = Convert.ToInt32(notice["REPLY_COUNT"]);

                    result.Add(noticeModel);
                }
            }
            else
            {
                NoticeModel noticeModel = new NoticeModel();

                noticeModel.Success = false;
                noticeModel.Message = "게시글 목록이 없거나 조회 실패";

                result.Add(noticeModel);
            }

            return result;
        }
        #endregion

        #region 조회수 증가
        /// <summary>
        /// 조회수를 증가시키고, 그에 맞는 실행 결과를 반환해줌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel PlusViewCount(int id)
        {
            ResultModel result = new ResultModel();

            bool updateResult = noticeDsl.UpdateViewCount(id);

            result.Success = updateResult;
            if (updateResult == true)
            {
                result.Message = "조회수 증가 성공";
            }
            else
            {
                result.Message = "조회수 증가 실패";
            }

            return result;
        }
        #endregion

        #region Notice Detail 조회
        /// <summary>
        /// notice의 id값을 토대로 상세조회(게시글, 댓글 목록)함
        /// 데이터가 있는지 DataTable.Rows.Count로 확인함
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public NoticeModel GetNoticeDetail(int boardId)
        {
            NoticeModel result = new NoticeModel();

            DataSet detailSet = noticeDsl.SelectNoticeDetail(boardId);
            DataTable noticeTable = detailSet.Tables[0];
            if(noticeTable.Rows.Count > 0)
            {
                
                result.Success = true;
                result.Message = "게시글 상세 조회 성공";

                DataRow notice = noticeTable.Rows[0];
                result.BOARD_SEQ = Convert.ToInt32(notice["BOARD_SEQ"]);
                result.TITLE = notice["TITLE"].ToString();
                result.STATE = notice["STATE"].ToString();
                result.USER_NAME = notice["USER_NAME"].ToString();
                result.CONTENTS = notice["CONTENTS"].ToString();
                result.WRITE_DATE = Convert.ToDateTime(notice["WRITE_DATE"]).ToString("yyyy-MM-dd");
                result.VIEW_COUNT = Convert.ToInt32(notice["VIEW_COUNT"]);
                result.FILE_NAME =  notice["FILE_NAME"].ToString();
                result.FILE = notice["FILE"].ToString();

                DataTable commentTable = detailSet.Tables[1];
                if (commentTable.Rows.Count > 0)
                {
                    foreach (DataRow comment in commentTable.Rows)
                    {
                        CommentModel commentModel = new CommentModel();

                        commentModel.REPLY_SEQ = Convert.ToInt32(comment["REPLY_SEQ"]);
                        if (!string.IsNullOrEmpty(comment["PARENT_SEQ"].ToString()))
                        {
                            commentModel.PARENT_SEQ = Convert.ToInt32(comment["PARENT_SEQ"]);
                        }
                        commentModel.BOARD_SEQ = Convert.ToInt32(comment["BOARD_SEQ"]);
                        commentModel.REPLY_CONTENTS = comment["REPLY_CONTENTS"].ToString();
                        commentModel.USER_NAME = comment["USER_NAME"].ToString();
                        commentModel.WRITE_DATE = Convert.ToDateTime(comment["CREATE_DATE"]).ToString("yyyy-MM-dd HH:mm");
                        commentModel.LEVEL = Convert.ToInt32(comment["LEVEL"]);

                        result.commentList.Add(commentModel);
                    }
                }
            }
            else
            {
                result.Success = false;
                result.Message = "게시글 상세 조회 실패";
            }

            return result;
        }
        #endregion



        #region 게시글 생성
        /// <summary>
        /// 게시글 생성의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ResultModel SaveNotice(NoticeModel notice)
        {
            ResultModel result = new ResultModel();

            bool insertResult = noticeDsl.InsertNotice(notice);
            result.Success = insertResult;
            if (insertResult == true)
            {
                result.Message = "게시글 생성 성공";
            }
            else
            {
                result.Message = "게시글 생성 실패";
            }

            return result;
        }
        #endregion

        #region 게시글 수정
        /// <summary>
        /// 게시글 수정의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ResultModel ModifyNotice(NoticeModel notice)
        {
            ResultModel result = new ResultModel();

            bool updateResult = noticeDsl.UpdateNotice(notice);
            result.Success = updateResult;
            if (updateResult == true)
            {
                result.Message = "게시글 수정 성공";
            }
            else
            {
                result.Message = "게시글 수정 실패";
            }

            return result;
        }
        #endregion

        #region 게시글 삭제 (transaction)
        /// <summary>
        /// 게시글 삭제의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// DB FK 조건에 따라 해당 게시글의 댓글들 먼저 삭제하고, 게시글을 삭제함
        /// 댓글, 게시글 삭제에 모두 성공하면 그에 맞는 결과값을 반환함
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel RemoveNotice(int id)
        {
            ResultModel result = new ResultModel();

            bool commentResult = false;
            bool noticeResult = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                commentResult = commentDsl.DeleteAllComment(id);
                noticeResult = noticeDsl.DeleteNotice(id);
                if (noticeResult == true && commentResult == true)
                {
                    result.Success = true;
                    result.Message = "게시글 삭제 성공";
                }
                else
                {
                    result.Success = false;
                    result.Message = "게시글 삭제 실패";
                }
                scope.Complete();
            }
            
            

            return result;
        }
        #endregion
    }
}