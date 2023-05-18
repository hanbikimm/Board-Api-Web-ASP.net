<template>
  <div class="detail-contents">
    <Detail 
    :notice="data"
    v-on:confirmDelete="clickDelete"
    v-on:refreshDetail="setDetail">
    </Detail>

    <Comment 
    v-on:refreshDetail="setDetail"
    :boardId="data.BOARD_SEQ"
    :comments="data.commentList">
    </Comment>

    <Confirm 
    v-if="isDeleteOpened"
    v-on:clickSave="removeNotice"
    v-on:closeConfirm="clickDelete"
    :confirmMsg="message">
    </Confirm>
  </div>
</template>

<script>
/**
 * @module views/NoticeDetail
 * @desc notice 상세 조회 뷰
 */
import Detail from '@/components/Detail.vue';
import Comment from '@/components/Comment.vue';
import Confirm from '@/components/Confirm.vue';

export default {
  name: 'NoticeDetail',
  components: {
    Detail,
    Comment,
    Confirm
  },

  data(){
    return {
      isDeleteOpened: false, //삭제창 오픈 여부
      message: '삭제하시겠습니까?', //Confirm 컴포넌트 종류 세팅
      data: {
        BOARD_SEQ: 0
      },
    }
  },
  
  created(){
    this.setDetail();
  },

  methods:{
    /**
     * 상세 조회 데이터 세팅
     */
    setDetail(){
      this.$service.getNoticeDetail(this.$route.params.id)
        .then(response => this.data = response.data)
        .catch(error => error.response.data);
    },
    
    /**
     * 삭제창 클릭 메서드
     */
    clickDelete(){
      this.isDeleteOpened = !this.isDeleteOpened;
    },

    /**
     * 목록 화면 이동 메서드
     */
    goToNotice(){
      this.$router.push({
        name: 'Notice'
      });
    },

    /**
     * notice 삭제 메서드
     */
    removeNotice(){
      this.$service.deleteNotice(this.$route.params.id)
        .then(response => {
          if(response.data.Success == true){
            alert('게시글이 삭제되었습니다.');
            this.goToNotice();
          }
        })
        .catch(error => error.response.data);
    },
  },
  
}
</script>

<style scoped>
.detail-contents{
  width:85%;
  margin-left: 10%;
  margin-right: 10%;
}
</style>