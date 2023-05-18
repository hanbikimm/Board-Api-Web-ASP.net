import Vue from 'vue';
import Vuex from 'vuex';
import memberStore from './modules/memberStore';
import noticeStore from './modules/noticeStore';


Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        memberStore: memberStore,
        noticeStore: noticeStore
    },
});