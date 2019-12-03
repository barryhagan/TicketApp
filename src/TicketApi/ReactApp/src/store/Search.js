import ApiService from "../ApiService";
import _ from "lodash";

const requestSearchType = "REQUEST_SEARCH";
const receiveSearchType = "RECEIVE_SEARCH";
const initialState = {
  searchResults: {},
  errors: null,
  isLoading: false
};

export const actionCreators = {
  requestSearchResults: searchInput => async dispatch => {
    dispatch({ type: requestSearchType });

    var result;
    try {
      result = await ApiService.getSearchResults(searchInput);
    } catch (ex) {
      const errors = { message: ex.message };
      dispatch({ type: receiveSearchType, errors });
      return;
    }

    if (result) {
      const searchResults = [
        ..._.map(result.tickets, t => {
          t.docType = "Ticket";
          return t;
        }),
        ..._.map(result.users, u => {
          u.docType = "User";
          return u;
        }),
        ..._.map(result.organizations, o => {
          o.docType = "Organization";
          return o;
        })
      ];

      dispatch({ type: receiveSearchType, searchResults });
    }
  }
};

export const searchReducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestSearchType) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === receiveSearchType) {
    return {
      ...state,
      searchResults: action.searchResults,
      errors: action.errors,
      isLoading: false
    };
  }

  return state;
};
