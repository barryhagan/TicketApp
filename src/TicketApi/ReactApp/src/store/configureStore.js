import { applyMiddleware, combineReducers, compose, createStore } from "redux";
import thunk from "redux-thunk";
import { routerReducer, routerMiddleware } from "react-router-redux";
import * as Search from "./Search";
import * as SearchSchema from "./SearchSchema";
import * as Ticket from "./Ticket";
import * as Organization from "./Organization";
import * as User from "./User";

export default function configureStore(history, initialState) {
  const reducers = {
    organization: Organization.organizationReducer,
    search: Search.searchReducer,
    searchSchema: SearchSchema.searchSchemaReducer,
    ticket: Ticket.ticketReducer,
    user: User.userReducer
  };

  const middleware = [thunk, routerMiddleware(history)];

  const enhancers = [];
  const isDevelopment = process.env.NODE_ENV === "development";
  if (
    isDevelopment &&
    typeof window !== "undefined" &&
    window.devToolsExtension
  ) {
    enhancers.push(window.devToolsExtension());
  }

  const rootReducer = combineReducers({
    ...reducers,
    routing: routerReducer
  });

  return createStore(
    rootReducer,
    initialState,
    compose(applyMiddleware(...middleware), ...enhancers)
  );
}
