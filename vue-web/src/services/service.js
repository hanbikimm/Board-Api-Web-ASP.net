import axios from "axios";

//params
const NOTICE_URL = "/Notice";
const COMMENT_URL = "/Comment";
const MEMBER_URL = "/Member";

const service = {
  
  //로그인
  checkUser(user){
    return axios.post(MEMBER_URL + "/Login", user);
  },

  //상태 목록 조회
  getStateList(){
    return axios
      .get(NOTICE_URL + "/StateList");
  },

  //엑셀 다운로드 
  downloadNoticeList(condition){
    if(condition === null){
      return axios
      .get(NOTICE_URL + "/ExcelDownLoad");
    }else{
      return axios
      .get(NOTICE_URL + "/ExcelDownLoad",{
        params:{
          STATE: condition.STATE,
          SEARCH_TYPE: condition.SEARCH_TYPE,
          SEARCH_WORD: condition.SEARCH_WORD
        }
      });
    }
  },

  //엑셀 업로드
  uploadNoticeList(){

  },

  //게시글 상태 수정
  updateState(state){
    return axios
      .post(NOTICE_URL + "/State", state);
  },

  //게시글 목록 조회 
  getNoticeList(condition){
    if(condition === null){
      return axios
      .get(NOTICE_URL + "/NoticeList");
    }else{
      return axios
      .get(NOTICE_URL + "/NoticeList",{
        params:{
          PAGE: condition.PAGE,
          STATE: condition.STATE,
          SEARCH_TYPE: condition.SEARCH_TYPE,
          SEARCH_WORD: condition.SEARCH_WORD
        }
      });
    }
  },

  //조회수 증가
  updateViewCount(id){
    return axios
      .post(NOTICE_URL + "/ViewCount", null, {
        params:{
          id: id
        }
      })
  },

  //상세 조회
  getNoticeDetail(id){
    return axios
      .get(NOTICE_URL + "/Detail", {
        params:{
          id: id
        }
      });
  },

  //게시글 생성
  createNotice(notice){
    return axios
      .post(NOTICE_URL + "/Create", notice)
  },

  //게시글 수정
  updateNotice(notice){
    return axios
      .post(NOTICE_URL + "/Update", notice)
  },

  //게시글 삭제
  deleteNotice(id){
    return axios
      .post(NOTICE_URL + "/Delete", null, {
        params:{
          id: id
        }
      })
  },

  //댓글 생성
  createComment(comment){
    return axios
      .post(COMMENT_URL + "/Create", comment)
  },

  //댓글 수정
  updateComment(comment){
    return axios
      .post(COMMENT_URL + "/Update", comment)
  },

  //댓글 삭제
  deleteComment(replyId){
    return axios
      .post(COMMENT_URL + "/Delete", null, {
        params:{
          id: replyId
        }
      })
  }
};

export default service;