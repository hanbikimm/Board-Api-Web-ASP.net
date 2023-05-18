<template>
  <transition name="modal">
    <div class="modal-mask">
      <div class="modal-wrapper">
        <div class="modal-container">
          <div class="modal-header">
            <slot name="header">
              <h3>상태 업데이트</h3>
            </slot>
          </div>
          <div class="modal-body">
            <slot name="body">
              <table>
                <tr>
                  <td>현재 상태</td>
                  <td>{{ state }}</td>
                </tr>
                <tr>
                  <td>변경상태</td>
                  <td>
                    <select v-model="selected.STATE">
                      <option disabled selected></option>
                      <option
                        v-for="data in stateList"
                        :key="data.STATE_SEQ"
                        :value="data.STATE_TYPE"
                      >
                        {{ data.STATE_TYPE }}
                      </option>
                    </select>
                  </td>
                </tr>
              </table>
              <div class="button-div">
                <button @click="clickEdit()">수정</button>
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
 * @module components/StatusUpdate
 * @desc 상태 수정 팝업 컴포넌트
 */
export default {
  name: 'StatusUpdate',
  props: {
    boardId: {
      type: Number,
      required: true,
    },
    state: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      stateList: [],
      selected: {
        BOARD_SEQ: this.boardId,
        STATE: '',
      },
    };
  },
  mounted() {
    /**
     * 서버에서 상태 목록을 받아와 option 값을 세팅
     */
    this.setStateList();
  },
  methods: {
    /**
     * 상태 목록 조회 메서드
     * 서버에서 상태 목록을 받아와 데이터에 담음
     */
    setStateList() {
      this.$service.getStateList()
        .then(response => this.stateList = response.data)
        .catch(error => error.response.data);
    },

    /**
     * 상태 수정 메서드
     */
    clickEdit() {
      //선택된 값이 현재값과 같거나 선택된 값이 없는 경우엔 팝업창 닫음
      if (this.selected.STATE == this.state || this.selected.STATE == '') {
        this.$emit('closeStatusUpdate');
      } else {
        //아닌 경우엔 서버로 선택된 값을 보내 상태 수정 후 팝업창 닫음
        this.$service.updateState(this.selected)
          .then(response => {
            if (response.data.Success == true) {
              this.$emit('successUpdate');
            }
          })
          .catch(error => error.response.data);
      }
    },
  },
};
</script>

<style scoped>
table {
  border-collapse: collapse;
  width: 100%;
}

th,
td {
  border: 1px solid black;
  padding: 5px 40px 5px 5px;
}

tr {
  text-align: left;
}

.button-div {
  width: 100%;
  display: flex;
  justify-content: end;
  margin-top: 20px;
}

button {
  padding: 5px 10px;
}

select {
  width: 100px;
  padding: 1px;
  border: 1px solid #999;
  border-radius: 0px;
  margin-top: 3px;
}
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
  width: 300px;
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
</style>