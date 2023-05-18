const state = () => {
  return {
    isConditionExists: false,
    condition: {
      PAGE: 1
    }
  }
}

const getters = {
  getIsConditionExists(state) {
    return state.isConditionExists;
  },

  getCondition(state) {
    return state.condition;
  },

  getPage(state) {
    return state.condition.PAGE;
  },
}

const mutations = {
  SET_PAGE(state, data) {
    state.condition.PAGE = data;
  },

  SET_CONDITION(state, data) {
    state.condition.STATE = data.STATE;
    state.condition.SEARCH_TYPE = data.SEARCH_TYPE;
    state.condition.SEARCH_WORD = data.SEARCH_WORD;
    state.condition.PAGE = data.PAGE;
  },

  INIT_CONDITION(state) {
    state.condition = {
      PAGE: 1
    }
  },

  SET_IS_CONDITION_EXISTS(state, data) {
    state.isConditionExists = data;
  }
}

export default {
  state,
  getters,
  mutations,
}