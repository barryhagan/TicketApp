import ApiService from "../ApiService";
import _ from "lodash";

const requestUserType = "REQUEST_USER";
const receiveUserType = "RECEIVE_USER";

const initialState = {
  user: null,
  errors: null,
  isLoading: false
};

export const actionCreators = {
  requestUser: id => async dispatch => {
    dispatch({ type: requestUserType });

    var result;
    try {
      result = await ApiService.getUser(id);
    } catch (ex) {
      const errors = { message: ex.message };
      dispatch({ type: receiveUserType, errors });
      return;
    }

    if (result) {
      dispatch({ type: receiveUserType, user: result });
    }
  }
};

export const userReducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestUserType) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === receiveUserType) {
    return {
      ...state,
      user: action.user,
      errors: action.errors,
      isLoading: false
    };
  }

  return state;
};
