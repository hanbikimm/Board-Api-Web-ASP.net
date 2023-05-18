using BootCampus.CMM.Notice.DSL;
using BootCampus.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampus.CMM.Notice.BSL
{
    public class MemberBsl
    {
        MemberDsl memberDsl = new MemberDsl();

        #region User 확인
        /// <summary>
        /// 입력된 ID, PW가 맞는지 확인 후, 맞다면 유저 정보를 반환함
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public MemberModel CheckUser(MemberModel user)
        {
            MemberModel result = new MemberModel();

            DataTable userTable = memberDsl.SelectMember(user.USER_ID, user.USER_PW);
            if(userTable.Rows.Count > 0)
            {
                result.Success = true;
                result.Message = "로그인 성공";

                DataRow data = userTable.Rows[0];
                result.USER_ID = data["USER_ID"].ToString();
                result.USER_NAME = data["USER_NAME"].ToString();
            }
            else
            {
                result.Success = false;
                result.Message = "로그인 실패";
            }

            return result;
        }
        #endregion
    }
}
