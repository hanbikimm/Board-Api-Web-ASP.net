<template>
  <div class="grid-search">
    <div>
      <label>상태</label>
      <br />
      <select v-model="condition.STATE">
        <option value=''></option>
        <option
          v-for="data in stateList"
          :key="data.STATE_SEQ"
          :value="data.STATE_TYPE"
        >
          {{ data.STATE_TYPE }}
        </option>
      </select>
    </div>

    <div>
      <label>검색조건</label>
      <br />
      <select v-model="condition.SEARCH_TYPE">
        <option value=''></option>
        <option
          v-for="(data, index) in searchList"
          :key="index"
          :value="data.value"
        >
          {{ data.value }}
        </option>
      </select>
    </div>

    <div>
      <br />
      <input type="text" v-model="condition.SEARCH_WORD" />
      <button @click="clickSearch()">검색</button>
    </div>
  </div>
</template>

<script>
/**
 * @module components/Search
 * @desc 목록 조회 화면의 검색 컴포넌트
 */
import { mapGetters, mapMutations } from 'vuex';

export default {
  name: 'Search',
  data() {
    return {
      stateList: [], //상태 목록
      searchList: [
        { value: '제목' },
        { value: '내용' },
        { value: '번호' },
        { value: '작성자' },
      ],
      //검색 조건
      condition: {
        STATE: '',
        SEARCH_TYPE: '',
        SEARCH_WORD: '',
      },
    };
  },
  computed: {
    ...mapGetters({
      mgGetIsConditionExists: 'getIsConditionExists',
      mgGetCondition: 'getCondition',
    }),
  },

  mounted() {
    //상태목록과 데이터 유지를 위한 검색 조건 세팅
    this.setStateList();
    this.setCondition();
  },

  methods: {
    ...mapMutations({
      SET_IS_CONDITION_EXISTS: 'SET_IS_CONDITION_EXISTS',
      SET_CONDITON: 'SET_CONDITION',
      INIT_CONDITION: 'INIT_CONDITION',
    }),

    /**
     * 상태 목록 세팅 메서드
     */
    setStateList() {
      this.$service.getStateList()
        .then(response => this.stateList = response.data)
        .catch(error => error.response.data);
    },

    /**
     * 검색 버튼 클릭 메서드
     */
    clickSearch() {
      // 검색 조건 Null 체크 후 이벤트 부모로 전달
      if (this.checkConditionIsNull()) {
        //조건이 없으면 스토어에 저장된 조건 초기화
        this.SET_IS_CONDITION_EXISTS(false);
        this.INIT_CONDITION();
      } else {
        //조건이 있으면 해당 조건으로 스토어에 저장, 페이지는 1로 초기화
        this.SET_IS_CONDITION_EXISTS(true);
        this.condition.PAGE = 1;
        this.SET_CONDITON(this.condition);
      }
      this.$emit('searchList');
    },

    /**
     * 검색 조건 Null 체크 메서드
     */
    checkConditionIsNull() {
      if (
        this.condition.STATE === '' &&
        this.condition.SEARCH_TYPE === '' &&
        this.condition.SEARCH_WORD.trim() === ''
      ) {
        return true;
      } else {
        return false;
      }
    },

    /**
     * mounted시 스토어에 저장된 검색 조건 세팅 메서드
     */
    setCondition() {
      if (this.mgGetIsConditionExists) {
        this.condition.STATE = this.mgGetCondition.STATE;
        this.condition.SEARCH_TYPE = this.mgGetCondition.SEARCH_TYPE;
        this.condition.SEARCH_WORD = this.mgGetCondition.SEARCH_WORD;
      }
    },
  },
};
</script>

<style scoped>
.grid-search {
  display: grid;
  grid-template-columns: 100px 100px auto;
  gap: 10px;
  margin-bottom: 20px;
  margin-top: 10px;
}

button {
  cursor: pointer;
  margin-left: 10px;
  margin-top: 5px;
  padding: 3px;
  width: 50px;
}

select {
  width: 100px;
  padding: 4px;
  border: 1px solid #999;
  border-radius: 0px;
  margin-top: 3px;
}

input {
  margin-top: 6px;
  width: 200px;
  padding: 4px;
  border: 1px solid #999;
  font-size: 14px;
}
</style>