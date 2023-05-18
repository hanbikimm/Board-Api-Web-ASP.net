<template>
  <transition name="modal">
    <div class="modal-mask">
      <div class="modal-wrapper">
        <div class="modal-container">
          <div class="modal-header">
            <slot name="header">
              <!-- 헤더 -->
              <h3>{{ confirmMsg }}</h3>
            </slot>
          </div>
          <div class="modal-body">
            <slot name="body">
              <!-- 바디 -->
              <button @click="clickConfirm()">확인</button>
              <button @click="clickCancel()">취소</button>
            </slot>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
/**
 * @module components/Confirm
 * @desc notice 저장/수정/삭제 시 확인 팝업 컴포넌트
 */
export default {
  name: 'Confirm',
  props:{
    confirmMsg: {
      type: String,
      required: true
    }
  },
  methods:{
    /**
     * 확인창 닫기 메서드
     * 부모 컴포넌트로 이벤트를 보내줌
     */
    clickCancel(){
      this.$emit('closeConfirm');
    },

    /**
     * 게시글 저장/수정/삭제 실행 메서드
     * 부모 컴포넌트로 이벤트를 보내줍니다.
     */
    clickConfirm(){
      this.$emit('clickSave');
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

button {
  padding: 5px 10px;
  margin-left: 10px;
  cursor: pointer;
}
</style>