<template>
  <div class="notice-content">
    <div class="grid-notice">
      <div>
        <h1>> Q&A 게시판</h1>
      </div>
      <div class="logout-div">
        <button class="logout-button" @click="logOut()">로그아웃</button>
      </div>
    </div>
    
    
    <Search
    v-on:searchList="setNoticeList">
    </Search>

    <List 
    ref="listComponent"
    v-on:goToPrevPage="setNoticeList"
    v-on:goToNextPage="setNoticeList"
    v-on:changePage="setNoticeList"
    :list="data">
    </List>
  </div>
</template>

<script>
/**
 * @module views/Notice
 * @desc notice 목록 조회 뷰
 */
import Search from '@/components/Search.vue';
import List from '@/components/List.vue';
import { mapGetters, mapMutations } from 'vuex';
export default {
  name: 'Notice',
  components:{
    Search,
    List
  },
  data(){
    return{
      data: [],
    }
  },

  computed:{
    ...mapGetters({
      mgGetCondition: 'getCondition',
      mgGetIsConditionExists: 'getIsConditionExists'
    })
  },

  mounted(){
    this.setNoticeList();
  },

  methods:{
    ...mapMutations({
      INIT_USER: 'INIT_USER',
      INIT_CONDITION: 'INIT_CONDITION',
      SET_IS_CONDITION_EXISTS: 'SET_IS_CONDITION_EXISTS',
      SET_TOTAL_PAGE: 'SET_TOTAL_PAGE',
    }),

    /**
     * 목록 데이터 세팅 메서드
     */
    setNoticeList(){
      //스토어에 저장된 검색 조건 체크
      if(this.mgGetIsConditionExists){
        //조건이 있다면 검색 조건을 서버로 보내 반환값 가져옴
        this.$service.getNoticeList(this.mgGetCondition)
          .then(response => {
            //목록 가져온 후 데이터 처리 메서드 호출
            this.afterGetNoticeList(response.data);            
          })
          .catch(error => error.response.data);
      }else{
        this.$service.getNoticeList(null)
        //조건이 없다면 null을 서버로 보내 반환값 가져옴
          .then(response => {
            //목록 가져온 후 데이터 처리 메서드 호출
            this.afterGetNoticeList(response.data);
          })
          .catch(error => error.response.data);
      }
    },

    /**
     * 리스트 데이터 처리 메서드
     * 조회 성공이면 목록/페이지 데이터 세팅을 하고,
     * 실패면 목록은 빈값, 페이지는 1로 세팅함
     */
    afterGetNoticeList(data){
      if(data[0].Success === true){
        this.$refs.listComponent.setPage(data[0].search);
        this.data = data;
      }else{
        var condition = {
          PAGE: 1,
          TOTAL_PAGE: 1
        }
        this.data = [];
        this.$refs.listComponent.setPage(condition);
      }
    },

    /**
     * 로그아웃 메서드
     * 스토어에 저장된 유저 정보, 검색 조건을 
     * 초기화하고 로그인 화면으로 이동
     */
    logOut(){
      this.INIT_USER();
      this.SET_IS_CONDITION_EXISTS(false);
      this.INIT_CONDITION();
      this.$router.push({
        name: 'Login'
      });
    }
  },
}
</script>

<style scoped>
.grid-notice {
  display: grid;
  grid-template-columns: 350px auto;
  gap: 10px;
}

.logout-button {
  margin: 30px 0px 25px 5px;
  width: 70px;
}

.logout-div {
    display: flex;
    justify-content: right;
}

.notice-content {
  width: 85%;
  margin-left: 10%;
  margin-right: 10%;
}

button {
  cursor: pointer;
}

</style>