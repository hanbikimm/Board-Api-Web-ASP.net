<template>
  <transition name="modal">
    <div class="modal-mask">
      <div class="modal-wrapper">
        <div class="modal-container">
          <div class="modal-header">
            <button @click="clickClose()">X</button>
            <slot name="header">
              <!-- 헤더 -->
              <h3>엑셀 업로드 데이터</h3>
            </slot>
          </div>
          <div class="modal-body">
            <!-- 바디 -->
            <slot name="body">
              <div class="table-div">
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
                      <td class="cursor">
                        {{ data.TITLE }} <strong>[{{ data.REPLY_COUNT }}]</strong>
                      </td>
                      <td class="txt-cent">{{ data.USER_NAME }}</td>
                      <td class="txt-cent">{{ data.WRITE_DATE }}</td>
                      <td class="txt-cent">{{ data.VIEW_COUNT }}</td>
                    </tr>
                    
                    
                  </tbody>
                </table>
              </div>
            </slot>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
/**
 * @module components/ExcelList
 * @desc 엑셀 업데이트 시 데이터를 보여주는 컴포넌트
 */
export default {
  name: 'ExcelList',
  props:{
    list:{
      type: Array,
      required: false
    }
  },
  methods:{
    /**
     * 모달창 닫기 클릭 메서드
     */
    clickClose(){
      this.$emit("closeExcel");
    }
  }
}
</script>

<style scoped>
.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
  transition: opacity 0.3s ease;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.modal-container {
  width: 90%;
  height: 80vh;
  margin: 0px auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  transition: all 0.3s ease;
  font-family: Helvetica, Arial, sans-serif;
}

.modal-header h3 {
  text-align: center;
  margin-top: 20px;
}

.modal-body {
  text-align: center;
  margin: 20px 0;
}

.modal-default-button {
  float: right;
}

.modal-enter {
  opacity: 0;
}

.modal-leave-active {
  opacity: 0;
}

.modal-enter .modal-container,
.modal-leave-active .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}

button {
  padding: 6px 10px;
  margin-top: 20px;
  cursor: pointer;
}

table {
  border-collapse: collapse;
  width: 100%;
}

tr,td {
  border: 1px solid #999;
  padding: 5px;
}

.table-div {
  overflow-y: auto;
  height: 400px;
}
</style>