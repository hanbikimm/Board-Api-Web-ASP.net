<template>
  <div>
    <!-- 확인창 -->
    <Confirm
      v-if="isConfirmOpened"
      :confirmMsg="message"
      v-on:closeConfirm="changeIsConfirmOpened"
      v-on:clickSave="saveNotice"
    >
    </Confirm>

    <!-- 상단 헤더 -->
    <div class="grid-title">
      <div>
        <h1>> 게시글 {{ type }}</h1>
      </div>
      <div class="nav-div">
        <button class="nav-button" @click="goToNotice()">목록</button>
        <button class="nav-button" @click="clickConfirm()">등록</button>
      </div>
    </div>

    <!-- 게시글 내용 수정 -->
    <div>
      <p v-if="isMessageShow">제목은 필수 입력사항입니다.</p>
      <table>
        <tr>
          <td>제목</td>
          <td colspan="5">
            <input
              type="text"
              placeholder="제목 입력란"
              v-model="data.TITLE"
              required
            />
          </td>
        </tr>
        <tr>
          <td>작성자</td>
          <td>{{ notice.USER_NAME }}</td>
          <td>작성일</td>
          <td>{{ notice.WRITE_DATE }}</td>
        </tr>
      </table>
      <textarea
        rows="6"
        v-model="data.CONTENTS"
        placeholder="내용 입력란"
      ></textarea>
    </div>

    <!-- 이미지 첨부 -->
    <div>
      <h2>> 이미지 첨부하기</h2>
      <div class="grid-img">
        <div class="img-div">
          <img v-if="data.FILE != ''"
          alt="이미지 미리보기" :src="data.FILE" />
          <img v-if="data.FILE == ''"
          alt="이미지 미리보기" src="../assets/noImg.png" />
        </div>
        <div>
          <input type="text" v-model="data.FILE_NAME" disabled />
          <span class="search-button">
            <label for="selectFile"> 찾아보기 </label>
          </span>
          <input
            type="file"
            id="selectFile"
            accept="image/*"
            @change="selectFile($event)"
            hidden
          />
          <button @click="removeFile()" class="delete-button">삭제</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
/**
 * @module components/Edit
 * @desc notice 등록 또는 수정하는 컴포넌트
 */
import Confirm from '@/components/Confirm.vue';
import { mapGetters, mapMutations } from 'vuex';

export default {
  name: 'Edit',
  props: {
    //notice 상세
    notice: {
      type: Object,
      required: true,
    },
    //확인창에 띄울 메세지
    message: {
      type: String,
      required: true,
    },
    //등록/수정 구분
    type: {
      type: String,
      required: true,
    },
  },
  components: {
    Confirm,
  },
  data() {
    return {
      isConfirmOpened: false, //확인창 열고 닫기 위한 값
      isMessageShow: false, //메세지를 띄우기 위한 값

      data: {
        BOARD_SEQ: this.$route.params.id,
        TITLE: '',
        CONTENTS: null,
        USER_NAME: '',
        FILE_NAME: '',
        FILE: '',
      },
    };
  },

  computed: {
    ...mapGetters({
      mgGetUserName: 'getUserName',
    }),
  },

  created() {
    //router param으로 넘어온 id값이 있으면 수정, 아니면 등록으로 간주하고 데이터 세팅
    if (this.$route.params.id > 0) {
      var titleIndex = this.notice.TITLE.indexOf(' ');
      this.data.TITLE = this.notice.TITLE.substring(titleIndex + 1);
      this.data.CONTENTS = this.notice.CONTENTS;
      this.data.USER_NAME = this.notice.USER_NAME;
      this.data.FILE_NAME = this.notice.FILE_NAME;
      this.data.FILE = this.notice.FILE;
    }
  },

  methods: {
    ...mapMutations({
      INIT_CONDITION: 'INIT_CONDITION',
      SET_IS_CONDITION_EXISTS: 'SET_IS_CONDITION_EXISTS'
    }),

    /**
     * 게시글 제목 Null 체크 메서드
     * if문을 통해 메세지를 띄워줌
     */
    checkNull() {
      if (this.data.TITLE === '' || this.data.TITLE.trim() === '') {
        this.isMessageShow = true;
        return true;
      } else {
        this.isMessageShow = false;
        return false;
      }
    },

    /**
     * 확인창 클릭 메서드
     * 등록버튼 클릭 시 제목이 Null이 아닌지 체크하고 확인창 띄움
     */
    clickConfirm() {
      if (!this.checkNull()) {
        this.isConfirmOpened = true;
      } else {
        this.isConfirmOpened = false;
      }
    },

    /**
     * 확인창 핸들 메서드
     */
    changeIsConfirmOpened() {
      this.isConfirmOpened = !this.isConfirmOpened;
    },

    /**
     * 목록 이동 메서드
     */
    goToNotice() {
      this.$router.push({
        name: 'Notice',
      });
    },

    /**
     * notice 수정 메서드
     */
    modifyNotice() {
      this.$service.updateNotice(this.data)
        .then(response => {
          //수정 성공의 경우 검색조건을 초기화한 후 목록으로 이동
          if (response.data.Success == true) {
            alert('게시글이 수정되었습니다.');
            this.initCondition();
            this.goToNotice();
          } else {
            alert('게시글 수정에 실패했습니다.');
          }
        })
        .catch(error => error.response.data);
    },

    /**
     * notice 등록 메서드
     */
    registerNotice() {
      this.$service.createNotice(this.data)
        .then(response => {
          //등록 성공의 경우 검색조건을 초기화한 후 목록으로 이
          if (response.data.Success == true) {
            alert('게시글이 등록되었습니다.');
            this.initCondition();
            this.goToNotice();
          } else {
            alert('게시글 등록에 실패했습니다.');
          }
        })
        .catch(error => error.response.data);
    },

    /**
     * notice 저장 메서드
     * router params id값에 따라 수정/등록 메서드 호출
     */
    saveNotice() {
      if (this.$route.params.id > 0) {
        this.modifyNotice();
      } else {
        this.data.USER_NAME = this.mgGetUserName;
        this.registerNotice();
      }
    },

    /**
     * 검색 조건 초기화 메서드
     */
    initCondition(){
      this.INIT_CONDITION();
      this.SET_IS_CONDITION_EXISTS(false);
    },

    /**
     * 이미지 파일 선택 메서드
     * @param event
     */
    selectFile(event) {
      const FILE = event.target.files[0];
      var fileType = FILE.type;
      if(this.checkFileType(fileType)){
        //선택된 파일의 이름을 저장
        this.data.FILE_NAME = FILE.name;
        var reader = new FileReader();
        reader.onload = (e) => {
          this.data.FILE = e.target.result;
        };
        //선택된 파일을 base64 인코딩해주고, 결과를 저장
        reader.readAsDataURL(FILE);
      }
      //선택된 파일값 초기화
      event.target.value = '';
      
    },

    /**
     * 파일 확장자 체크 메서드
     */
    checkFileType(fileType){
      if(!fileType.includes('image')){
        alert('이미지 파일만 등록 가능합니다.');
        return false;
      }else {
        return true;
      }
    },

    /**
     * 선택 파일 삭제 메서드
     */
    removeFile() {
      this.data.FILE_NAME = '';
      this.data.FILE = '';
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

.nav-button {
  margin: 30px 0px 25px 5px;
  width: 70px;
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

.search-button {
  margin-left: 10px;
  font-size: 15px;
  border: 1px solid rgb(69, 69, 189);
  padding: 7px 10px;
  border-radius: 2px;
  color: white;
  background-color: cornflowerblue;
}

.delete-button {
  margin-left: 10px;
  font-size: 15px;
  border: none;
  padding: 6px 10px;
  border-radius: 2px;
  border: 1px solid cornflowerblue;
  color: cornflowerblue;
  background-color: white;
}

.grid-img {
  display: grid;
  grid-template-columns: 300px auto;
  gap: 10px;
  height: 300px;
  align-items: center;
}

.img-div {
  overflow: hidden;
}

img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

label,
button {
  cursor: pointer;
}

p {
  color: rgb(215, 4, 4);
}

input {
  margin-left: 10px;
  width: 50%;
  padding: 6px;
  border: 1px solid #999;
  font-size: 15px;
  background-color: white;
}

textarea {
  width: 97.3%;
  border: 1px solid #999;
  padding: 10px;
  font-size: 15px;
  resize: none;
}
</style>