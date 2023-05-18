<template>
  <div>
    <!-- 댓글 입력창 -->
    <h3>> 댓글</h3>
    <div class="textarea-div">
      <textarea v-model="reply.REPLY_CONTENTS" rows="3"></textarea>
      <button class="reply-register-button" @click="saveReply()">등록</button>
    </div>

    <!-- v-for 댓글 -->
    <div v-for="comment in comments" :key="comment.REPLY_SEQ">
      <hr />
      <div class="reply-contents">
        <span v-if="comment.LEVEL == 1">➥ </span>
        <!-- 댓글 내용 -->
        <span v-if="editId !== comment.REPLY_SEQ">
          {{ comment.USER_NAME }} ({{ comment.WRITE_DATE }})
          <button
            class="edit-button"
            v-if="comment.LEVEL == 0"
            @click="handleSecondReply(comment.REPLY_SEQ)"
          >
            답변
          </button>
          <span v-if="comment.USER_NAME == mgGetUserName">
            <button
              class="edit-button"
              @click="openEdit(comment.REPLY_SEQ, comment.REPLY_CONTENTS)"
            >
              편집
            </button>
            <button class="edit-button" @click="deleteReply(comment.REPLY_SEQ)">
              삭제
            </button>
          </span>

          <div>
            <p v-if="comment.LEVEL === 0">
              <strong>{{ comment.REPLY_CONTENTS }}</strong>
            </p>
            <blockquote v-if="comment.LEVEL === 1">
              <strong>{{ comment.REPLY_CONTENTS }}</strong>
            </blockquote>
          </div>
        </span>
        <!-- 댓글 편집창 -->
        <span class="textarea-div" v-if="editId === comment.REPLY_SEQ">
          <textarea v-model="editReply.REPLY_CONTENTS"></textarea>
          <button
            class="reply-edit-button"
            @click="saveEditedReply(comment.REPLY_CONTENTS)"
          >
            등록
          </button>
        </span>
      </div>

      <!-- 대댓글 입력창 -->
      <div class="textarea-div" v-if="secondId === comment.REPLY_SEQ">
        <hr />
        <div class="grid-reply">
          <div>➥</div>
          <div>
            <textarea
              v-model="secondReply.REPLY_CONTENTS"
              placeholder="답변 입력란"
            />
            <button class="second-reply-button" @click="saveSecondReply()">
              등록
            </button>
          </div>
        </div>
      </div>
    </div>
    <hr />
  </div>
</template>

<script>
/**
 * @module components/Comment
 * @desc 상세 조회 시 사용되는 댓글 컴포넌트
 */
import { mapGetters } from 'vuex';
export default {
  name: 'Comment',
  props: {
    boardId: {
      type: Number,
      required: true,
    },
    comments: {
      type: Array,
      required: false,
    },
  },
  data() {
    return {
      editId: 0, //편집창을 열고 닫기 위한 값
      secondId: 0, // 대댓글 입력창을 열고 닫기 위한 값

      //입력 댓글
      reply: {
        BOARD_SEQ: 0,
        REPLY_CONTENTS: '',
        USER_NAME: '',
      },

      //편집 댓글
      editReply: {
        REPLY_SEQ: 0,
        REPLY_CONTENTS: '',
      },

      //입력 대댓글 
      secondReply: {
        BOARD_SEQ: 0,
        PARENT_SEQ: 0,
        REPLY_CONTENTS: '',
        USER_NAME: '',
      },
    };
  },

  computed: {
    ...mapGetters({
      mgGetUserName: 'getUserName',
    }),
  },

  methods: {
    /**
     * 댓글 저장 메서드
     */
    saveReply() {
      //댓글의 내용이 null 인지 체크
      if (!this.checkContentsIsNull(this.reply.REPLY_CONTENTS)) {
        this.reply.BOARD_SEQ = this.boardId;
        this.reply.USER_NAME = this.mgGetUserName;
        this.$service.createComment(this.reply)
          .then(response => {
            //결과가 성공이면 입력된 데이터를 초기화하고 새로고침
            if (response.data.Success === true) {
              this.reply.REPLY_CONTENTS = '';
              this.$emit('refreshDetail');
            } else {
              alert('댓글 생성에 실패했습니다.');
            }
          })
          .catch(error => error.response.data);
      }
    },
    /**
     * 댓글 삭제 메서드
     * @param {Number} replyId - 댓글 번호
     */
    deleteReply(replyId) {
      this.$service.deleteComment(replyId)
        .then(response => {
          if (response.data.Success === true) {
            this.$emit('refreshDetail');
          } else {
            alert('댓글 삭제에 실패했습니다.');
          }
        })
        .catch(error => error.response.data);
    },

    /**
     * 편집창 오픈 메서드
     * @param {Number} id - 댓글 번호
     * @param {String} contents - 댓글 내용
     */
    openEdit(id, contents) {
      //편집창 오픈시, 대댓글 입력창이 열려있다면 닫아줌
      this.secondId = 0;
      //해당 ID의 편집창을 열어주고 데이터를 세팅
      this.editId = id;
      this.editReply.REPLY_SEQ = id;
      this.editReply.REPLY_CONTENTS = contents;
    },

    /**
     * 댓글 편집 메서드
     */
    saveEditedReply(contents) {
      //댓글 내용 Null 체크
      if (!this.checkContentsIsNull(this.editReply.REPLY_CONTENTS)) {
        //댓글 내용이 원본과 같은지 체크
        if (this.editReply.REPLY_CONTENTS !== contents) {
          this.$service.updateComment(this.editReply)
            .then(response => {
              //결과가 성공이라면 새로고침을 하고, 편집창을 닫음
              if (response.data.Success === true) {
                this.$emit('refreshDetail');
                this.editId = 0;
              } else {
                alert('댓글 수정에 실패했습니다.');
              }
            })
            .catch(error => error.response.data);
        } else {
          this.editId = 0;
        }
      }
    },

    /**
     * 대댓글 입력창 핸들 메서드
     * @param {Number} id = 댓글 번호
     */
    handleSecondReply(id) {
      //해당 대댓글창 닫음
      if (this.secondId == id) {
        this.secondId = 0;
        //다른 대댓글창을 닫거나, 해당 대댓글 창을 엶
      } else if (this.secondId >= 0) {
        this.secondId = 0;
        this.secondId = id;
        this.secondReply.PARENT_SEQ = id;
        this.secondReply.REPLY_CONTENTS = '';
      }
    },

    /**
     * 대댓글 저장 메서드
     */
    saveSecondReply() {
      //대댓글 내용 Null 체크
      if (!this.checkContentsIsNull(this.secondReply.REPLY_CONTENTS)) {
        this.secondReply.BOARD_SEQ = this.boardId;
        this.secondReply.USER_NAME = this.mgGetUserName;
        this.$service.createComment(this.secondReply)
          .then(response => {
            //결과가 성공이면 데이터 초기화 및 새로고침
            if (response.data.Success === true) {
              this.secondReply.REPLY_CONTENTS = "";
              this.$emit('refreshDetail');
              this.secondId = 0;
            } else {
              alert('댓글 생성에 실패했습니다.');
            }
          })
          .catch(error => error.response.data);
      }
    },

    /**
     * Null 체크 메서드
     * @param {String} contents - 댓글 내용
     * 댓글 내용에 따라 Null 값을 체크합니다.
     */
    checkContentsIsNull(contents) {
      if (contents === '') {
        alert('댓글을 입력해주세요.');
        return true;
      } else {
        return false;
      }
    },
  },
};
</script>

<style scoped>
.textarea-div {
  width: 100%;
  position: relative;
}

textarea {
  width: 97.3%;
  border: 1px solid #999;
  padding: 10px;
  font-size: 15px;
  resize: none;
}

button {
  cursor: pointer;
}

.edit-button {
  margin-left: 7px;
  padding: 0;
  border: none;
  text-decoration: underline;
  color: cornflowerblue;
  background-color: white;
}

.reply-edit-button {
  padding: 5px 10px;
  position: absolute;
  top: -29px;
  right: 20px;
}

.reply-register-button {
  padding: 5px 10px;
  position: absolute;
  top: 30px;
  right: 20px;
}

.second-reply-button {
  padding: 5px 10px;
  position: absolute;
  top: 23px;
  right: 20px;
}

.grid-reply {
  display: grid;
  width: 100%;
  grid-template-columns: 30px auto;
  gap: 0px;
}

.reply-contents {
  margin: 10px 0px 10px 5px;
}

blockquote {
  margin: 5px 0px 5px 21px;
}

p {
  margin: 5px;
  text-indent: -3px;
}
</style>