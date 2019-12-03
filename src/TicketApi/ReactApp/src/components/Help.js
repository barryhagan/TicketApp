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
        <table>
          <thead>
            <tr>
              <th>Feature</th>
              <th>Example</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>Wildcard</td>
              <td>name:Mar*</td>
            </tr>
            <tr>
              <td>Fuzzy Match</td>
              <td>name:Mary~</td>
            </tr>
            <tr>
              <td>Boolean And</td>
              <td>name:Mar* AND suspended:false</td>
            </tr>
            <tr>
              <td>Boolean Or</td>
              <td>name:Mar* OR name:Bo*</td>
            </tr>
            <tr>
              <td>Empty Fields</td>
              <td>
                details:""
                <br />
                details:ISNULL
              </td>
            </tr>
          </tbody>
        </table>
        <p></p>
        <h4>Search Field Reference</h4>
        <p>The following fields can be used when searching for documents:</p>
        <SearchFieldRow>
          {this.props.schema && this.props.schema.organization ? (
            <SearchFieldColumn>
              <h5>Organization</h5>
              <table>
                <tbody>
                  {_.map(this.props.schema.organization.sort(), field => (
                    <tr key={field}>
                      <td>{field}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </SearchFieldColumn>
          ) : null}

          {this.props.schema && this.props.schema.ticket ? (
            <SearchFieldColumn>
              <h5>Ticket</h5>
              <table>
                <tbody>
                  {_.map(this.props.schema.ticket.sort(), field => (
                    <tr key={field}>
                      <td>{field}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </SearchFieldColumn>
          ) : null}

          {this.props.schema && this.props.schema.user ? (
            <SearchFieldColumn>
              <h5>User</h5>
              <table>
                <tbody>
                  {_.map(this.props.schema.user.sort(), field => (
                    <tr key={field}>
                      <td>{field}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </SearchFieldColumn>
          ) : null}
        </SearchFieldRow>
      </div>
    );
  }
}

export default connect(
  state => state.searchSchema,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Help);

const SearchFieldRow = styled.div`
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
