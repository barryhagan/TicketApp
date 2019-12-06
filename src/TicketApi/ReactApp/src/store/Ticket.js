import ApiService from "../ApiService";

const requestTicketType = "REQUEST_TICKET";
const receiveTicketType = "RECEIVE_TICKET";

const initialState = {
  ticket: null,
  errors: null,
  isLoading: false
};

export const actionCreators = {
  requestTicket: id => async dispatch => {
    dispatch({ type: requestTicketType });

    var result;
    try {
      result = await ApiService.getTicket(id);
    } catch (ex) {
      const errors = { message: ex.message };
      dispatch({ type: receiveTicketType, errors });
      return;
    }

    if (result) {
      dispatch({ type: receiveTicketType, ticket: result });
    }
  }
};

export const ticketReducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestTicketType) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === receiveTicketType) {
    return {
      ...state,
      ticket: action.ticket,
      errors: action.errors,
      isLoading: false
    };
  }

  return state;
};
