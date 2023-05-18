<template>
  <!-- 상태 수정창 -->
  <div>
    <StatusUpdate
      v-on:successUpdate="refreshDetail"
      v-on:closeStatusUpdate="clickUpdateState"
      :boardId="notice.BOARD_SEQ"
      :state="notice.STATE"
      v-if="isStatusUpdateOpened"
    >
    </StatusUpdate>

    <!-- 상단 헤더 -->
    <div class="grid-title">
      <div>
        <h1>> 게시글 상세</h1>
      </div>
      <div class="nav-div">
        <button class="status-button" @click="clickUpdateState()">
          상태 업데이트
        </button>
        <span v-if="notice.USER_NAME == mgGetUserName">
          <button class="nav-button" @click="goToHandle()">수정</button>
          <button class="nav-button" @click="clickDelete()">삭제</button>
        </span>
      </div>
    </div>

    <!-- 게시글 상세 -->
    <div>
      <table>
        <tr>
          <td>제목</td>
          <td colspan="6">{{ notice.TITLE }}</td>
        </tr>
        <tr>
          <td>작성자</td>
          <td>{{ notice.USER_NAME }}</td>
          <td>작성일</td>
          <td>{{ notice.WRITE_DATE }}</td>
          <td>조회수</td>
          <td>{{ notice.VIEW_COUNT }}</td>
        </tr>
      </table>
    </div>
    <div class="grid-contents" v-if="notice.FILE != ''">
      <div class="img-div">
        <img alt="이미지 미리보기" :src="notice.FILE" />
      </div>
      <div class="contents-div">
        <p>
          {{ notice.CONTENTS }}
        </p>
      </div>
    </div>
    <div class="contents-div" v-if="notice.FILE == ''">
      <p>
        {{ notice.CONTENTS }}
      </p>
    </div>
  </div>
</template>

<script>
/**
 * @module components/Detail
 * @desc 상세 조회 시 notice 부분 컴포넌트
 */
import StatusUpdate from '@/components/StatusUpdate.vue';
import { mapGetters } from 'vuex';

export default {
  name: 'Detail',
  props: {
    notice: {
      type: Object,
      required: true,
    },
  },
  components: {
    StatusUpdate,
  },
  data() {
    return {
      isStatusUpdateOpened: false, //상태 수정창 열고 닫기 위한 값
    };
  },

  computed: {
    ...mapGetters({
      mgGetUserName: 'getUserName',
    }),
  },

  methods: {
    /**
     * 상태 수정창 핸들 메서드
     * 참조하고 있는 boolean값을 통해 창을 열고 닫음
     */
    clickUpdateState() {
      this.isStatusUpdateOpened = !this.isStatusUpdateOpened;
    },

    /**
     * 상세조회 새로고침 메서드
     * 상태가 수정된 경우, 수정창을 닫아주고 부모로 이벤트를 보내 새로고침
     */
    refreshDetail() {
      this.isStatusUpdateOpened = !this.isStatusUpdateOpened;
      this.$emit('refreshDetail');
    },

    /**
     * 게시글 수정 화면 이동 메서드
     * 수정 버튼을 누를 경우 해당 게시글의 데이터를 
     * router params로 보내고 수정 화면으로 이동
     */
    goToHandle() {
      this.$router.push({
        name: 'HandleNotice',
        params: {
          id: this.notice.BOARD_SEQ,
          notice: this.notice,
        },
      });
    },

    /**
     * 삭제 모달 오픈 메서드
     * 부모 컴포넌트로 이벤트를 보냅니다.
     */
    clickDelete() {
      this.$emit('confirmDelete');
    },
  },
};
</script>

<style scoped>
.grid-title {
  display: grid;
  grid-template-columns: 350px auto;
  gap: 10px;
}

button {
  cursor: pointer;
}

.nav-button {
  margin: 30px 0px 25px 5px;
  width: 70px;
  padding: 6px 0px;
}

.status-button {
  margin: 30px 0px 25px 5px;
  width: 100px;
  padding: 6px 0px;
}

.nav-div {
  display: flex;
  justify-content: right;
}

table {
  border-collapse: collapse;
  width: 100%;
  margin-bottom: 10px;
}

tr,
td {
  border: 1px solid #999;
  padding: 5px;
}

.img-div {
  width: 100%;
}

img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.grid-contents {
  display: grid;
  grid-template-columns: 300px auto;
  gap: 10px;
  height: 60%;
}

.contents-div {
  width: 100%;
  border: 1px solid #999;
}

p {
  white-space: pre-line;
  padding: 10px 10px;
}
</style>