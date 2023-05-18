<template>
  <div class="handle-contents">
    <Edit :notice="data" 
    :message="confirmMsg"
    :type="handleType">
    </Edit>
  </div>
</template>

<script>
/**
 * @module views/HandleNotice
 * @desc 게시글 등록/수정 뷰
 */
import Edit from '@/components/Edit.vue';
import { mapGetters } from 'vuex';
import moment from 'moment';

export default {
  name: 'HandleNotice',
  components: {
    Edit,
  },
  data(){
    return{
      //화면의 종류
      handleType: '',
      confirmMsg: '',

      data:{
        BOARD_SEQ: 0,
        TITLE: '',
        CONTENTS: '',
        USER_NAME: '',
        WRITE_DATE: '',
        FILE_NAME: '',
        FILE: ''
      }
    }
  },

  computed:{
    ...mapGetters({
      mgGetUserName: 'getUserName'
    })
  },

  created(){
    this.setHandleInfo();
  },

  methods:{
    /**
     * 화면 종류 세팅
     * router params id값을 통해 등록/수정 화면 설정
     */
    setHandleInfo(){
      if(this.$route.params.id > 0){
        this.handleType = '수정';
        this.confirmMsg = '수정하시겠습니까?';
        this.data = this.$route.params.notice;
      }else{
        this.handleType = '등록';
        this.confirmMsg = '저장하시겠습니까?';
        this.data.USER_NAME = this.mgGetUserName;
        this.data.WRITE_DATE = moment().format('YYYY-MM-DD');
      }
    }
  }
}
</script>

<style scoped>
.handle-contents{
  width:85%;
  margin-left: 10%;
  margin-right: 10%;
}
</style>