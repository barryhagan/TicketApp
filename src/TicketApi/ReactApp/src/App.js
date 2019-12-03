import React from "react";
import { Route } from "react-router";
import Layout from "./components/Layout";
import Search from "./components/Search";
import Help from "./components/Help";
import User from "./components/User";
import Ticket from "./components/Ticket";
import Organization from "./components/Organization";

export default () => (
  <Layout>
    <Route exact path="/" component={Search} />
    <Route path="/help" component={Help} />
    <Route path="/user/:id" component={User} />
    <Route path="/ticket/:id" component={Ticket} />
    <Route path="/organization/:id" component={Organization} />
  </Layout>
);
