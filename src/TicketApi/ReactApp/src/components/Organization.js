import React, { Component } from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { actionCreators } from "../store/Organization";
import { Link } from "react-router-dom";
import _ from "lodash";
import {
  DataGridHeader,
  DataGridHeaderItem,
  DataGridHeaderItemTen,
  DataGridRow,
  DataGridItem,
  DataGridItemTen
} from "./Grid";

class Organization extends Component {
  async componentDidMount() {
    try {
      const id = this.props.match.params.id;
      this.props.requestOrganization(id);
    } catch (err) {}
  }

  render() {
    const org = this.props.organization;
    return (
      <div>
        <h4>Organization Details</h4>
        {org ? (
          <div>
            <DataGridHeader>
              <DataGridHeaderItem>Field</DataGridHeaderItem>
              <DataGridHeaderItemTen>Value</DataGridHeaderItemTen>
            </DataGridHeader>
            <DataGridRow>
              <DataGridItem>_id</DataGridItem>
              <DataGridItemTen>{org.id}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>created_at</DataGridItem>
              <DataGridItemTen>{org.created_at}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>details</DataGridItem>
              <DataGridItemTen>{org.details}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>domain_names</DataGridItem>
              <DataGridItemTen>
                {" "}
                {_.map(org.domain_names, (domain, idx) => (
                  <div key={`dn_${idx}`}>{domain}</div>
                ))}
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>external_id</DataGridItem>
              <DataGridItemTen>{org.external_id}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>name</DataGridItem>
              <DataGridItemTen>{org.name}</DataGridItemTen>
            </DataGridRow>

            <DataGridRow>
              <DataGridItem>shared_tickets</DataGridItem>
              <DataGridItemTen>{String(org.shared_tickets)}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>tags</DataGridItem>
              <DataGridItemTen>
                {_.map(org.tags, (tag, idx) => (
                  <div key={`tag_${idx}`}>{tag}</div>
                ))}
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>url</DataGridItem>
              <DataGridItemTen>
                <a href={org.url} target="_blank" rel="noopener noreferrer">
                  {org.url}
                </a>
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>users</DataGridItem>
              <DataGridItemTen>
                {_.map(org.users, (user, idx) => (
                  <div key={`user_${idx}`}>
                    <Link to={`/user/${user.id}`}>
                      {user.name} {user.email ? `(${user.email})` : null}
                    </Link>
                  </div>
                ))}
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>tickets</DataGridItem>
              <DataGridItemTen>
                {_.map(org.tickets, (ticket, idx) => (
                  <div key={`ticket_${idx}`}>
                    <Link to={`/ticket/${ticket.id}`}>{ticket.subject}</Link> (
                    {ticket.status})
                  </div>
                ))}
              </DataGridItemTen>
            </DataGridRow>
          </div>
        ) : null}
      </div>
    );
  }
}

export default connect(
  state => state.organization,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Organization);
