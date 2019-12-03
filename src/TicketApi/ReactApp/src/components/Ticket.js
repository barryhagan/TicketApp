import React, { Component } from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import _ from "lodash";
import ApiService from "../ApiService";
import {
  DataGridHeader,
  DataGridHeaderItem,
  DataGridHeaderItemTen,
  DataGridRow,
  DataGridItem,
  DataGridItemTen
} from "./Grid";

class Ticket extends Component {
  constructor(props) {
    super(props);
    this.state = { ticket: null };
  }

  async componentDidMount() {
    const ticket = await ApiService.getTicket(this.props.match.params.id);
    this.setState({ ticket: ticket });
  }

  render() {
    const ticket = this.state.ticket;
    return (
      <div>
        <h4>Ticket Details</h4>
        {ticket ? (
          <div>
            <DataGridHeader>
              <DataGridHeaderItem>Field</DataGridHeaderItem>
              <DataGridHeaderItemTen>Value</DataGridHeaderItemTen>
            </DataGridHeader>
            <DataGridRow>
              <DataGridItem>_id</DataGridItem>
              <DataGridItemTen>{ticket.id}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>
                <Link to={`/user/${ticket.assignee.id}`}>assignee</Link>
              </DataGridItem>
              <DataGridItemTen>
                {" "}
                {ticket.assignee.name}{" "}
                {ticket.assignee.email ? `(${ticket.assignee.email})` : null}
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>created_at</DataGridItem>
              <DataGridItemTen>{ticket.created_at}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>description</DataGridItem>
              <DataGridItemTen>{ticket.description}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>due_at</DataGridItem>
              <DataGridItemTen>{ticket.due_at}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>external_id</DataGridItem>
              <DataGridItemTen>{ticket.external_id}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>has_incidents</DataGridItem>
              <DataGridItemTen>{String(ticket.has_incidents)}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>
                <Link to={`/organization/${ticket.organization.id}`}>
                  organization
                </Link>
              </DataGridItem>
              <DataGridItemTen>{ticket.organization.name}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>priority</DataGridItem>
              <DataGridItemTen>{ticket.priority}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>status</DataGridItem>
              <DataGridItemTen>{ticket.status}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>subject</DataGridItem>
              <DataGridItemTen>{ticket.subject}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>
                {" "}
                <Link to={`/user/${ticket.submitter.id}`}>submitter</Link>
              </DataGridItem>
              <DataGridItemTen>
                {ticket.submitter.name}{" "}
                {ticket.submitter.email ? `(${ticket.submitter.email})` : null}
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>tags</DataGridItem>
              <DataGridItemTen>
                {" "}
                {_.map(ticket.tags, (tag, idx) => (
                  <div key={`tag_${idx}`}>{tag}</div>
                ))}
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>type</DataGridItem>
              <DataGridItemTen>{ticket.type}</DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>url</DataGridItem>
              <DataGridItemTen>
                <a href={ticket.url}>{ticket.url}</a>
              </DataGridItemTen>
            </DataGridRow>
            <DataGridRow>
              <DataGridItem>via</DataGridItem>
              <DataGridItemTen>{ticket.via}</DataGridItemTen>
            </DataGridRow>
          </div>
        ) : null}
      </div>
    );
  }
}

export default connect()(Ticket);
