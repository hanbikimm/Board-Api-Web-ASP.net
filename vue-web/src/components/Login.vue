<template>
  <div class="content">
    <h1 class="txt-cent">로그인</h1>
    <div>
      <table>
        <tr>
          <td>아이디</td>
          <td><input type="text" v-model="userInfo.USER_ID" /></td>
        </tr>
        <tr>
          <td>패스워드</td>
          <td><input type="password" v-model="userInfo.USER_PW" /></td>
        </tr>
        <tr>
          <td class="txt-right" colspan="2">
            <button @click="login()">로그인</button>
          </td>
        </tr>
      </table>
    </div>
  </div>
</template>

<script>
/**
 * @module components/Login
 * @desc Login 컴포넌트
 */
import { mapMutations } from 'vuex';
export default {
  name: 'Login',
  data() {
    return {
      userInfo: {
        USER_ID: '',
        USER_PW: '',
      },
    };
  },

  methods: {
    ...mapMutations({
      ADD_USER: 'ADD_USER',
    }),

    /**
     * 아이디,패스워드 Empty 체크
     */
    checkInput() {
      if (this.userInfo.USER_ID === '' && this.userInfo.USER_PW === '') {
        alert('아이디와 패스워드를 입력해주세요.');
        return false;
      } else if (this.userInfo.USER_ID === '') {
        alert('아이디를 입력해주세요.');
        return false;
      } else if (this.userInfo.USER_PW === '') {
        alert('패스워드를 입력해주세요.');
        return false;
      } else {
        return true;
      }
    },

    /**
     * 로그인 메서드
     * Null값 체크 후 axios 요청을 보내고 후처리
     */
    login() {
      if (this.checkInput()) {
        this.$service.checkUser(this.userInfo)
          .then(response => this.afterCheckUser(response.data))
          .catch(error => error.response.data);
      }
    },

    /**
     * Axios 요청 이후 then 처리 메서드
     * @param {Object} data - 요청 반환 데이터
     */
    afterCheckUser(data) {
      if (data.Success === true) {
        //로그인 성공의 경우 스토어에 유저 정보 저장 후 목록 조회 화면으로 이동
        this.ADD_USER(data);
        alert(`${data.USER_NAME}님 환영합니다!`);
        this.$router.push({
          name: 'Notice',
        });
      } else {
        alert('로그인에 실패하였습니다. 아이디와 비밀번호를 확인해주세요!');
      }
    },
  },
};
</script>

<style scoped>
table {
  margin-left: auto;
  margin-right: auto;
  border-collapse: collapse;
  border: none;
}

td {
  border: none;
  padding: 5px;
  min-width: 100px;
}

.content {
  margin-top: 100px;
  margin-left: 100px;
  margin-right: 100px;
  padding: 50px;
}

input {
  width: 200px;
  padding: 4px;
  font-size: 15px;
  border: 1px solid #999;
}

button {
  margin-top: 2px;
  padding: 3px;
  width: 60px;
  cursor: pointer;
}
</style>