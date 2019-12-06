import ApiService from "../ApiService";
import _ from "lodash";

const requestSearchType = "REQUEST_SEARCH";
const receiveSearchType = "RECEIVE_SEARCH";
const receiveSearchTermType = "RECEIVE_SEARCH_TERM";
const initialState = {
  searchResults: {},
  errors: null,
  isLoading: false
};

export const actionCreators = {
  saveSearchTerm: searchTerm => async dispatch => {
    dispatch({ type: receiveSearchTermType, searchTerm });
  },
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
      const searchResults = _.orderBy(
        [
          ..._.map(result.tickets, t => {
            const scoredTicket = {
              ...t.item,
              docType: "Ticket",
              score: t.score
            };
            return scoredTicket;
          }),
          ..._.map(result.users, u => {
            const scoredUser = { ...u.item, docType: "User", score: u.score };
            return scoredUser;
          }),
          ..._.map(result.organizations, o => {
            const scoredOrg = {
              ...o.item,
              docType: "Organization",
              score: o.score
            };
            return scoredOrg;
          })
        ],
        ["score"],
        ["desc"]
      );

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

  if (action.type === receiveSearchTermType) {
    return {
      ...state,
      searchTerm: action.searchTerm
    };
  }

  return state;
};
