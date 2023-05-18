const state = () => {
  return {
    user: {
      USER_ID: null,
      USER_NAME: null
    }
  }
}

const getters = {
  getUserName(state) {
    return state.user.USER_NAME;
  }
}

const mutations = {
  ADD_USER(state, data) {
    state.user.USER_ID = data.USER_ID;
    state.user.USER_NAME = data.USER_NAME;
  },

  INIT_USER(state) {
    state.user.USER_ID = null;
    state.user.USER_NAME = null;
  }
}

export default {
  state,
  getters,
  mutations,
}