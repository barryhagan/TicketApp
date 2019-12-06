import React from "react";
import ReactDOM from "react-dom";
import { Provider } from "react-redux";
import { MemoryRouter } from "react-router-dom";
import Help from "../Help";

it("renders without crashing", () => {
  const storeFake = state => ({
    default: () => {},
    subscribe: () => {},
    dispatch: () => {},
    getState: () => ({ ...state })
  });
  const store = storeFake({});

  const div = document.createElement("div");
  ReactDOM.render(
    <Provider store={store}>
      <MemoryRouter>
        <Help />
      </MemoryRouter>
    </Provider>,
    div
  );
});
