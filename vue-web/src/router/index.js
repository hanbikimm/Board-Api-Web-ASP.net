import Vue from "vue";
import VueRouter from "vue-router";
import Login from "../components/Login";
import Notice from "../views/Notice";
import HandleNotice from "../views/HandleNotice";
import NoticeDetail from "../views/NoticeDetail";
import store from "../store/index";

Vue.use(VueRouter);

const routes = [
    {
        path: "/",
        component: Login,
        name: "Login",
      },
      {
        path: "/Home",
        component: Notice,
        name: "Notice",
        meta: { authRequired: true } 
      },
      {
        path: "/Detail/:id",
        component: NoticeDetail,
        name: "NoticeDetail",
        meta: { authRequired: true } 
      },
      {
        path: "/Handle",
        component: HandleNotice,
        name: "HandleNotice",
        meta: { authRequired: true }  
      }
]

const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    routes
});

router.beforeEach((to, from, next) => {
  //이동하려는 라우터의 meta.authRequired가 true일 경우
  if(to.matched.some(routeInfo => routeInfo.meta.authRequired)){
    //스토어에 유저 정보가 있나 체크하고 이동을 제한함
    if(store.getters.getUserName === null){
      alert('로그인이 필요합니다.');
      next("/");
    }else{
      next();
    }
  }else{
    next();
  }
})

export default router;