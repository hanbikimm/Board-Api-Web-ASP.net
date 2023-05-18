using BootCampus.CMM.Notice.DSL;
using BootCampus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.CMM.Notice.BSL
{
    public class CommentBsl
    {
        CommentDsl commentDsl = new CommentDsl();

        #region 댓글 생성
        /// <summary>
        /// 댓글 생성의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public ResultModel SaveComment(CommentModel comment)
        {
            ResultModel result = new ResultModel();

            bool insertResult = commentDsl.InsertComment(comment);
            result.Success = insertResult;
            if (insertResult == true)
            {
                result.Message = "댓글 생성 성공";
            }
            else
            {
                result.Message = "댓글 생성 실패";
            }

            return result;
        }
        #endregion

        #region 댓글 수정
        /// <summary>
        /// 댓글 수정의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>

        public ResultModel ModifyComment(CommentModel comment)
        {
            ResultModel result = new ResultModel();

            bool updateResult = commentDsl.UpdateComment(comment);
            result.Success = updateResult;
            if (updateResult == true)
            {
                result.Message = "댓글 수정 성공";
            }
            else
            {
                result.Message = "댓글 수정 실패";
            }

            return result;
        }
        #endregion 

        #region 댓글 삭제
        /// <summary>
        /// 댓글 삭제의 성공 여부에 따라 데이터를 담고 결과를 반환함
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel RemoveComment(int id)
        {
            ResultModel result = new ResultModel();

            bool removeResult = commentDsl.DeleteComment(id);
            result.Success = removeResult;
            if (removeResult == true)
            {
                result.Message = "댓글 삭제 성공";
            }
            else
            {
                result.Message = "댓글 삭제 실패";
            }

            return result;
        }
        #endregion 
    }
}
