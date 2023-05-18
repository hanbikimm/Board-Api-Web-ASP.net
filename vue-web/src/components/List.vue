<template>
  <div>
    <div>
      <!-- notice 목록 -->
      <table>
        <thead class="txt-cent">
          <tr>
            <td>번호</td>
            <td>상태</td>
            <td>제목</td>
            <td>작성자</td>
            <td>작성일</td>
            <td>조회수</td>
          </tr>
        </thead>
        <tbody>
          <tr v-for="data in list" :key="data.BOARD_SEQ">
            <td class="txt-cent">{{ data.BOARD_SEQ }}</td>
            <td>{{ data.STATE }}</td>
            <td class="cursor" @click="goToDetail(data.BOARD_SEQ)">
              {{ data.TITLE }} <strong>[{{ data.REPLY_COUNT }}]</strong>
            </td>
            <td class="txt-cent">{{ data.USER_NAME }}</td>
            <td class="txt-cent">{{ data.WRITE_DATE }}</td>
            <td class="txt-cent">{{ data.VIEW_COUNT }}</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- 목록 하단 버튼 -->
    <div class="grid-buttons">
      <div>
        <button class="download-button"
        @click="clickDownload()">
          엑셀 다운로드
        </button>
        <span class="upload-button">
            <label for="selectExcel"> 엑셀 업로드 </label>
          </span>
          <input
            type="file"
            id="selectExcel"
            accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            @change="uploadExcel($event)"
            hidden
          />
      </div>

      <!-- 페이징 -->
      <div class="page-div">
        <button class="page-button" @click="clickPrev()">이전</button>
        <button
          class="page-button"
          v-for="page in pages.pageList"
          :key="page"
          :class="[page === pages.nowPage ? 'selected' : '']"
          @click="clickNowPage(page)"
        >
          {{ page }}
        </button>
        <button class="page-button" @click="clickNext()">다음</button>
      </div>

      <!-- 작성버튼 -->
      <div class="register-div">
        <button class="register-button" @click="goToHandle()">작성</button>
      </div>
    </div>

    <ExcelList
    v-if="isUploadOpened"
    v-on:closeExcel="closeList"
    :list="uploadList">
    </ExcelList>
  </div>
</template>

<script>
/**
 * @module components/List
 * @desc notice 목록 조회 컴포넌트
 */
import moment from 'moment';
import * as xlsx from 'xlsx';
import { mapGetters, mapMutations } from 'vuex';
import ExcelList from '@/components/ExcelList.vue';

export default {
  name: 'List',
  components:{
    ExcelList
  },
  props: {
    list: {
      type: Array,
      required: false,
    },
  },
  data() {
    return {
      pages: {
        nowPage: '', //현재 페이지
        totalPage: '', //전체 페이지
        pageList: [], //페이징 번호 목록
      },
      isUploadOpened: false,
      uploadList: [] //엑셀 업로드 시 반환된 목록
    };
  },

  computed:{
    ...mapGetters({
      mgGetIsConditionExists: 'getIsConditionExists',
      mgGetCondition: 'getCondition'
    })
  },

  mounted() {},
  methods: {
    ...mapMutations({
      SET_IS_CONDITION_EXISTS: 'SET_IS_CONDITION_EXISTS',
      SET_PAGE: 'SET_PAGE',
    }),

    /**
     * 상세 조회 화면 이동 메서드
     * @param {Number} id - 게시글 번호
     * 조회수 증가 성공시 상세 화면으로 이동
     */
    goToDetail(id) {
      this.$service.updateViewCount(id)
        .then(response => {
          if (response.data.Success == true) {
            this.$router.push({
              name: 'NoticeDetail',
              params: {
                id: id,
              },
            });
          }
        })
        .catch(error => error.response.data);
    },

    /**
     * 작성 화면 이동 메서드
     * 수정화면인지 구분하기 위해 id 0으로 세팅
     */
    goToHandle() {
      this.$router.push({
        name: 'HandleNotice',
        params: {
          id: 0,
        },
      });
    },

    /**
     * 엑셀 다운로드 클릭 메서드
     */
    clickDownload(){
      //검색 조건 유무 체크 후 다운로드 메서드 호출
      if(this.mgGetIsConditionExists){
        this.$service.downloadNoticeList(this.mgGetCondition)
          .then(response => {
            this.downloadExcel(response.data);
          })
        .catch(error => error.response.data);
      }else{
        this.$service.downloadNoticeList(null)
          .then(response => {
            this.downloadExcel(response.data);
          })
        .catch(error => error.response.data);
      }
    },

    /**
     * 엑셀 다운로드 메서드
     * @param {Array} noticeList - axios 요청 반환 데이터
     */
    downloadExcel(noticeList){
      var excelList = [];
      //파일명에 사용할 현재 시간
      var now = moment().format('YYMMDDHHmmss');
      //엑셀에 담을 데이터 정제
      noticeList.forEach(notice => {
        var data = {
          BOARD_SEQ: notice.BOARD_SEQ,
          STATE: notice.STATE,
          TITLE: notice.TITLE,
          REPLY_COUNT: notice.REPLY_COUNT,
          USER_NAME: notice.USER_NAME,
          WRITE_DATE: notice.WRITE_DATE,
          VIEW_COUNT: notice.VIEW_COUNT
        }
        excelList.push(data);
      });
      
      //패키지 이용해 json->excel로 변환 후 다운로드
      const WORKBOOK = xlsx.utils.book_new();
      const WORKSHEET = xlsx.utils.json_to_sheet(excelList);
      xlsx.utils.book_append_sheet(WORKBOOK, WORKSHEET, 'Notice');
      xlsx.writeFile(WORKBOOK, `Notice_${now}.xlsx`);
    },

    /**
     * 엑셀 업로드 메서드
     * @param event
     */
    uploadExcel(event){
      //1) 선택된 파일을 저장
      const FILE = event.target.files[0];
      var reader = new FileReader();
      reader.onload = () => {
        var data = reader.result;
        //3) 데이터를 arrayBuffer -> excel -> json 순으로 변환 후 데이터에 담음
        const WORKBOOK = xlsx.read(data, {type: 'binary'});
        const ROA = xlsx.utils.sheet_to_json(WORKBOOK.Sheets['Notice']);
        this.uploadList = ROA;
      };
      //2) 파일을 arraybuffer로 변환
      reader.readAsArrayBuffer(FILE);
      this.isUploadOpened = true;
    },

    /**
     * upload excel 데이터 목록 팝업창 닫는 메서드
     */
    closeList(){
      this.isUploadOpened = false;
      this.uploadList = [];
    },

    /**
     * 페이지 세팅 메서드
     * @param {Object} data - 페이지 관련 값을 갖는 객체
     */
    setPage(data) {
      this.pages.pageList = [];
      //초기값에 맞게 현재 페이지와 전체 페이지 수 저장
      if (data.PAGE == 0) {
        this.pages.nowPage = 1;
      } else {
        this.pages.nowPage = data.PAGE;
      }
      this.pages.totalPage = data.TOTAL_PAGE;

      //현재 페이지, 전체 페이지 값으로 페이징 계산
      //페이징에 보여줄 페이지 수
      var showPage = 5;
      //시작될 페이지 번호
      var startPage = Math.floor((this.pages.nowPage - 1) / showPage) * showPage + 1;
      var checkRestPage = this.pages.totalPage - (startPage - 1);
      //페이징에 보여줄 실제 페이지 수
      var pageCount = checkRestPage < showPage ? checkRestPage : showPage;

      for (var i = 0; i < pageCount; i++) {
        this.pages.pageList.push(i + startPage);
      }
    },

    /**
     * 이전 페이지 클릭 이벤트
     */
    clickPrev() {
      //페이지가 1이면 페이지 이동 막음
      if (this.pages.nowPage === 1) {
        alert('첫번째 페이지입니다.');
      } else {
        //이동할 페이지 스토어에 저장 후 이벤트 부모로 전달
        this.SET_IS_CONDITION_EXISTS(true);
        this.SET_PAGE(this.pages.nowPage - 1);
        this.$emit('goToPrevPage');
      }
    },

    /**
     * 다음 페이지 클릭 이벤트
     */
    clickNext() {
      //페이지가 마지막 페이지이면 페이지 이동 막음
      if (this.pages.nowPage === this.pages.totalPage) {
        alert('마지막 페이지입니다.');
      } else {
        //이동할 페이지 스토어에 저장 후 이벤트 부모로 전달
        this.SET_IS_CONDITION_EXISTS(true);
        this.SET_PAGE(this.pages.nowPage + 1);
        this.$emit('goToNextPage');
      }
    },

    /**
     * 페이지 번호 클릭 이벤트
     * @param {Number} page - 클릭된 페이지 번호
     * 현재 페이지 스토어에 저장 후 이벤트 부모로 전달
     */
    clickNowPage(page) {
      this.SET_IS_CONDITION_EXISTS(true);
      this.SET_PAGE(page);
      this.$emit('changePage');
    },
  },
};
</script>

<style scoped>
table {
  border-collapse: collapse;
  width: 100%;
}
tr,td {
  border: 1px solid #999;
  padding: 5px;
}
.txt-cent {
  text-align: center;
}
.cursor {
  cursor: pointer;
}

.selected {
  color: orangered;
}

.download-button {
  padding: 4px;
  margin-right: 10px;
}

label {
  cursor: pointer;
}

.upload-button {
  font-size: 13.9px;
  cursor: pointer;
  background-color: #F0F0F0;
  border: 1px solid rgb(122, 122, 122);
  border-radius: 2px;
  padding: 7px 7px 6px 6px;
}

.register-div {
  display: flex;
  justify-content: right;
}

.register-button {
  padding: 4px;
  margin-left: 120px;
  width: 50px;
  height: 30px;
}

.page-div {
  display: flex;
  justify-content: center;
}

.page-button {
  border: none;
  background-color: white;
  padding: 6px;
  border-radius: 2px;
  width: 40px;
  height: 30px;
}

.page-button:hover {
  background: #e3e2e2;
  cursor: pointer;
}

.grid-buttons {
  display: grid;
  width: 100%;
  grid-template-columns: auto 400px auto;
  gap: 10px;
  margin-top: 20px;
}

button {
  cursor: pointer;
}
</style>