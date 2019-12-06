import React, { Component } from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { actionCreators } from "../store/SearchSchema";
import styled from "styled-components";
import _ from "lodash";

class Help extends Component {
  componentDidMount() {
    this.props.requestSearchSchema();
  }

  render() {
    return (
      <div>
        <h1>Search Help</h1>
        <br />
        <h4>Search Scope</h4>
        <p>
          By default, a global search is performed against all Tickets, Users,
          and Organizations. To limit your search to a specific object type,
          select that type from the drop-down menu on the search page.
        </p>

        <h4>Search Fields</h4>
        <p>
          You may search specific fields in a document by entering the field
          name and search value separated by a colon (e.g.{" "}
          <strong>alias:Miss</strong>).
        </p>
        <p>
          If you do not specify a field when searching, the following fields
          will be searched by default:
        </p>
        <ul>
          <li>alias</li>
          <li>description</li>
          <li>details</li>
          <li>email</li>
          <li>external_id</li>
          <li>name</li>
          <li>signature</li>
          <li>subject</li>
          <li>tags</li>
        </ul>
        <h4>Advanced Syntax</h4>
        <p>
          Searches can be performed using the following syntax features, and you
          may specify multiple search terms using boolean logic.
        </p>
        <p>
          Terms should be quoted if they contain spaces or other special
          characters.
        </p>
        <p>
          Dates should be searched using the <strong>yyyyMMddHHmmss</strong>
          format
        </p>
        <SearchFieldRow>
          <SearchFieldColumnHeader>Feature</SearchFieldColumnHeader>
          <SearchFieldColumnHeader>Example</SearchFieldColumnHeader>
        </SearchFieldRow>
        <SearchFieldRow>
          <SearchFieldColumn>Wildcard</SearchFieldColumn>
          <SearchFieldColumn>name:Mar*</SearchFieldColumn>
        </SearchFieldRow>
        <SearchFieldRow>
          <SearchFieldColumn>Fuzzy Match</SearchFieldColumn>
          <SearchFieldColumn>name:Mary~</SearchFieldColumn>
        </SearchFieldRow>
        <SearchFieldRow>
          <SearchFieldColumn>Boolean And</SearchFieldColumn>
          <SearchFieldColumn>name:Mar* AND suspended:false</SearchFieldColumn>
        </SearchFieldRow>
        <SearchFieldRow>
          <SearchFieldColumn>Boolean Or</SearchFieldColumn>
          <SearchFieldColumn>name:Mar* OR name:Bo*</SearchFieldColumn>
        </SearchFieldRow>
        <SearchFieldRow>
          <SearchFieldColumn>Date Range Search</SearchFieldColumn>
          <SearchFieldColumn>
            last_login_at:[201410 TO 201412]
          </SearchFieldColumn>
        </SearchFieldRow>
        <SearchFieldRow>
          <SearchFieldColumn>Empty Fields</SearchFieldColumn>
          <SearchFieldColumn>
            details:""
            <br />
            details:ISNULL
          </SearchFieldColumn>
        </SearchFieldRow>
        <p></p>
        <h4>Search Field Reference</h4>
        <p>
          The following fields can be used when searching for documents. Click
          on the fields in the search page sidebar to use them.
        </p>
        <SearchFieldRow>
          {this.props.schema && this.props.schema.organization ? (
            <SearchFieldColumn>
              <h5>Organization</h5>
              {_.map(this.props.schema.organization.sort(), field => (
                <div>{field}</div>
              ))}
            </SearchFieldColumn>
          ) : null}

          {this.props.schema && this.props.schema.ticket ? (
            <SearchFieldColumn>
              <h5>Ticket</h5>
              {_.map(this.props.schema.ticket.sort(), field => (
                <div>{field}</div>
              ))}
            </SearchFieldColumn>
          ) : null}

          {this.props.schema && this.props.schema.user ? (
            <SearchFieldColumn>
              <h5>User</h5>
              {_.map(this.props.schema.user.sort(), field => (
                <div>{field}</div>
              ))}
            </SearchFieldColumn>
          ) : null}
        </SearchFieldRow>
        <p></p>
      </div>
    );
  }
}

export default connect(
  state => state.searchSchema,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Help);

const SearchFieldRow = styled.div`
  border-bottom: 1px solid rgba(18, 60, 83, 0.05);
  border-top: 1px solid rgba(18, 60, 83, 0.05);
  padding: 12px 32px;
  &:after {
    content: "";
    display: table;
    clear: both;
  }
`;

const SearchFieldColumn = styled.div`
  float: left;
  width: 33.33%;
`;
const SearchFieldColumnHeader = styled.div`
  float: left;
  width: 33.33%;
  font-weight: bold;
`;
