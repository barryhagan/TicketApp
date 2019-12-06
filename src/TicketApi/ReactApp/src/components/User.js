import React, { Component } from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { actionCreators } from "../store/User";
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
import emailIcon from "./email.png";

class User extends Component {
  async componentDidMount() {
    try {
      const id = this.props.match.params.id;
      this.props.requestUser(id);
    } catch (err) {}
  }

  render() {
    const { user } = this.props;
    return (
      <div>
        <h4>User Details</h4>
        {user ? (
          <div>
            <DataGridHeader>
              <DataGridHeaderItem>Field</DataGridHeaderItem>
              <DataGridHeaderItemTen>Value</DataGridHeaderItemTen>
            </DataGridHeader>
            <DataGridRow>
              <DataGridItem>_id</DataGridItem>
              <DataGridItemTen>{user.id}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>active</DataGridItem>
              <DataGridItemTen>{String(user.active)}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>alias</DataGridItem>
              <DataGridItemTen>{user.alias}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>created_at</DataGridItem>
              <DataGridItemTen>{user.created_at}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>email</DataGridItem>
              <DataGridItemTen>
                <span>
                  {user.email}{" "}
                  <a
                    href={`mailto:${user.email}`}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    <img src={emailIcon} alt="Send Email" />
                  </a>
                </span>
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>external_id</DataGridItem>
              <DataGridItemTen>{user.external_id}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>last_login_at</DataGridItem>
              <DataGridItemTen>{user.last_login_at}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>locale</DataGridItem>
              <DataGridItemTen>{user.locale}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>name</DataGridItem>
              <DataGridItemTen>{user.name}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>
                {user.organization ? (
                  <Link to={`/organization/${user.organization.id}`}>
                    organization
                  </Link>
                ) : (
                  "organization"
                )}
              </DataGridItem>
              <DataGridItemTen>
                {user.organization ? user.organization.name : "--"}
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>phone</DataGridItem>
              <DataGridItemTen>{user.phone}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>role</DataGridItem>
              <DataGridItemTen>{user.role}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>shared</DataGridItem>
              <DataGridItemTen>{String(user.shared)}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>signature</DataGridItem>
              <DataGridItemTen>{user.signature}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>suspended</DataGridItem>
              <DataGridItemTen>{String(user.suspended)}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>tags</DataGridItem>
              <DataGridItemTen>
                {" "}
                {_.map(user.tags, (tag, idx) => (
                  <div key={`tag_${idx}`}>{tag}</div>
                ))}
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>timezone</DataGridItem>
              <DataGridItemTen>{user.timezone}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>url</DataGridItem>
              <DataGridItemTen>
                <a href={user.url} target="_blank" rel="noopener noreferrer">
                  {user.url}
                </a>
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>verified</DataGridItem>
              <DataGridItemTen>{String(user.verified)}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>assigned tickets</DataGridItem>
              <DataGridItemTen>
                {_.map(user.assignedTickets, (ticket, idx) => (
                  <div key={`ticket_${idx}`}>
                    <Link to={`/ticket/${ticket.id}`}>{ticket.subject} </Link> (
                    {ticket.status})
                  </div>
                ))}
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>submitted tickets</DataGridItem>
              <DataGridItemTen>
                {_.map(user.submittedTickets, (ticket, idx) => (
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
  state => state.user,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(User);
