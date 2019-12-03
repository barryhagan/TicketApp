import ApiService from "../ApiService";
const requestSearchSchemaType = "REQUEST_SEARCH_SCHEMA";
const receiveSearchSchemaType = "RECEIVE_SEARCH_SCHEMA";
const initialState = {
  schema: {},
  isLoading: false
};

export const actionCreators = {
  requestSearchSchema: () => async (dispatch, getState) => {
    const loaded = getState().searchSchema.schema;
    if (loaded && loaded.user) {
      return;
    }

    dispatch({ type: requestSearchSchemaType });

    var schema = await ApiService.getSearchSchema();

    if (schema) {
      dispatch({ type: receiveSearchSchemaType, schema });
    }
  }
};

export const searchSchemaReducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestSearchSchemaType) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === receiveSearchSchemaType) {
    return {
      ...state,
      schema: action.schema,
      isLoading: false
    };
  }

  return state;
};
