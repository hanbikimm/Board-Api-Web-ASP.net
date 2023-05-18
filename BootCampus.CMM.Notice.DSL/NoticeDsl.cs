using BootCampus.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BootCampus.CMM.Notice.DSL
{
    public class NoticeDsl
    {
        private const string _connectionString = @"server=DESKTOP-6L322N3;database=BOOTCAMPUS;pwd=mssql;user id=sa;Pooling=True;Min Pool Size=10;Max Pool Size=150;Load Balance Timeout=360;Connection Lifetime=360;Connect Timeout=30;Enlist=True;TrustServerCertificate=True";

        #region State List 조회
        public DataTable SelectStateList()
        {
            DataTable stateTable = new DataTable();
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_STATE_R", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(stateTable);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return stateTable;
        }
        #endregion

        #region Notice State 수정
        public bool UpdateState(NoticeModel notice)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_STATE_U", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", notice.BOARD_SEQ);
                    cmd.Parameters.AddWithValue("@STATE", notice.STATE);

                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return result;
        }
        #endregion

        #region 엑셀 다운로드
        /// <summary>
        /// 만약 조건이 있다면 조건 파라미터값을 넣어줌
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable SelectNoticeListForExcel(SearchModel condition)
        {
            DataTable noticeTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_FOR_EXCEL_L", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (condition.STATE != null || condition.SEARCH_TYPE != null || condition.SEARCH_WORD != null)
                    {
                        cmd.Parameters.AddWithValue("@STATE", condition.STATE);
                        if (condition.SEARCH_WORD != null)
                        {
                            cmd.Parameters.AddWithValue("@SEARCH_TYPE", condition.SEARCH_TYPE);
                            cmd.Parameters.AddWithValue("@SEARCH_WORD", condition.SEARCH_WORD);
                        }
                    }

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(noticeTable);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }

            return noticeTable;

        }
        #endregion

        #region Notice List 조회
        /// <summary>
        /// 만약 조건이 있다면 조건 파라미터값을 넣어주고,
        /// 페이지가 1보다 크다면 페이지 파라미터값을 넣어줌
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet SelectNoticeList(SearchModel condition)
        {
            DataSet noticeListSet = new DataSet();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_L", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(condition.STATE != null || condition.SEARCH_TYPE != null || condition.SEARCH_WORD != null)
                    {
                        cmd.Parameters.AddWithValue("@STATE", condition.STATE);
                        if(condition.SEARCH_WORD != null)
                        {
                            cmd.Parameters.AddWithValue("@SEARCH_TYPE", condition.SEARCH_TYPE);
                            cmd.Parameters.AddWithValue("@SEARCH_WORD", condition.SEARCH_WORD);
                        }
                        
                    }
                    if(condition.PAGE > 1)
                    {
                        cmd.Parameters.AddWithValue("@page", condition.PAGE);
                    }

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(noticeListSet);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                
            }

            return noticeListSet;
        }
        #endregion

        #region 조회수 증가
        /// <summary>
        /// id값을 가져와 조회수를 증가해줌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateViewCount(int id)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_VIEW_COUNT_U", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", id);

                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return result;
        }
        #endregion

        #region Notice Detail 조회
        /// <summary>
        /// 해당 id값의 상세조회 데이터를 반환함
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public DataSet SelectNoticeDetail(int boardId)
        {
            DataSet detailSet = new DataSet();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_R", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", boardId);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(detailSet);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return detailSet;
        }
        #endregion


        #region Notice 생성
        /// <summary>
        /// CONTENTS와 FILE은 필수조건이 아니기 때문에, 구분 후 파라미터값 넣어줌
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public bool InsertNotice(NoticeModel notice)
        {
            bool result = false;
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_C", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@STATE", "오픈");
                    cmd.Parameters.AddWithValue("@TITLE", notice.TITLE);
                    if(notice.CONTENTS == null)
                    {
                        notice.CONTENTS = "";
                    }
                    cmd.Parameters.AddWithValue("@CONTENTS", notice.CONTENTS);
                    cmd.Parameters.AddWithValue("@USER_NAME", notice.USER_NAME);
                    if (!string.IsNullOrEmpty(notice.FILE_NAME))
                    {
                        cmd.Parameters.AddWithValue("@FILE_NAME", notice.FILE_NAME);
                        cmd.Parameters.AddWithValue("@FILE", notice.FILE);
                    }

                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return result;
        }
        #endregion

        #region Notice 수정
        /// <summary>
        /// CONTENTS와 FILE은 필수조건이 아니기 때문에, 구분 후 파라미터값 넣어줌
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public bool UpdateNotice(NoticeModel notice)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_U", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", notice.BOARD_SEQ);
                    cmd.Parameters.AddWithValue("@TITLE", notice.TITLE);
                    cmd.Parameters.AddWithValue("@CONTENTS", notice.CONTENTS);
                    if (!string.IsNullOrEmpty(notice.FILE_NAME))
                    {
                        cmd.Parameters.AddWithValue("@FILE_NAME", notice.FILE_NAME);
                        cmd.Parameters.AddWithValue("@FILE", notice.FILE);
                    }


                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return result;
        }
        #endregion

        #region Notice 삭제
        /// <summary>
        /// id값에 해당하는 데이터를 삭제해줌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteNotice(int id)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.UP_BOOTCAMPUS_BOARD_D", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BOARD_SEQ", id);

                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return result;
        }
        #endregion
    }
}