import ApiService from "../ApiService";
import _ from "lodash";

const requestOrgType = "REQUEST_ORG";
const receiveOrgType = "RECEIVE_ORG";

const initialState = {
  organization: null,
  errors: null,
  isLoading: false
};

export const actionCreators = {
  requestOrganization: id => async dispatch => {
    dispatch({ type: requestOrgType });

    var result;
    try {
      result = await ApiService.getOrganization(id);
    } catch (ex) {
      const errors = { message: ex.message };
      dispatch({ type: receiveOrgType, errors });
      return;
    }

    if (result) {
      dispatch({ type: receiveOrgType, organization: result });
    }
  }
};

export const organizationReducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestOrgType) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === receiveOrgType) {
    return {
      ...state,
      organization: action.organization,
      errors: action.errors,
      isLoading: false
    };
  }

  return state;
};
