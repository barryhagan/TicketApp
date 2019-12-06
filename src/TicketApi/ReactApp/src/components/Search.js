import React, { Component } from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import { actionCreators as searchCreators } from "../store/Search";
import { actionCreators as searchSchemaCreators } from "../store/SearchSchema";
import styled from "styled-components";
import _ from "lodash";
import {
  DataGridHeader,
  DataGridHeaderItem,
  DataGridHeaderItemDouble,
  DataGridRow,
  DataGridItem,
  DataGridItemDouble
} from "./Grid";

class Search extends Component {
  constructor(props) {
    super(props);
    this.state = { search: "", scope: "", lastSearch: null };

    this.searchBox = React.createRef();
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleClick = this.handleClick.bind(this);
    this.handleTermClick = this.handleTermClick.bind(this);
  }

  componentDidMount() {
    this.props.requestSearchSchema();
    this.setState({ search: (this.props.search || {}).searchTerm || "" });
  }

  handleInputChange(event) {
    var name = event.target.name;
    this.setState({ [name]: event.target.value });
  }

  handleSubmit(event) {
    const { search, scope } = this.state;
    const { saveSearchTerm, requestSearchResults } = this.props;
    saveSearchTerm(search);
    requestSearchResults({
      search: search,
      scope: scope
    });
    this.setState({ lastSearch: search });

    event.preventDefault();
  }

  handleClick(link) {
    this.props.history.push(link);
  }

  handleTermClick(term) {
    const currentSearch = this.state.search;
    const newSearch = currentSearch
      ? `${currentSearch} AND ${term}:`
      : `${term}:`;
    this.setState({ search: newSearch });
    this.searchBox.current.focus();
  }

  render() {
    const { scope, search, lastSearch } = this.state;
    const { searchResults, errors, isLoading } = this.props.search || {};
    const { schema } = this.props.searchSchema || {};

    const hasResults = searchResults && searchResults.length > 0;

    return (
      <SearchLayout>
        <SearchWrapper>
          <form onSubmit={this.handleSubmit}>
            <label>
              Scope:&nbsp;
              <select
                name="scope"
                value={scope}
                onChange={this.handleInputChange}
              >
                <option value="">Global</option>
                <option value="Organization">Organizations</option>
                <option value="Ticket">Tickets</option>
                <option value="User">Users</option>
              </select>
            </label>
            <SearchInput
              name="search"
              value={search}
              type="textbox"
              ref={this.searchBox}
              onChange={this.handleInputChange}
            />
            <SearchButton type="submit" value="Search" />
          </form>

          {isLoading ? null : hasResults ? (
            <div>
              <ResultText>{searchResults.length} results</ResultText>
              <DataGridHeader>
                <DataGridHeaderItem>Id</DataGridHeaderItem>
                {!scope ? <DataGridHeaderItem>Type</DataGridHeaderItem> : null}
                <DataGridHeaderItemDouble>Details</DataGridHeaderItemDouble>
                <DataGridHeaderItem>Tags</DataGridHeaderItem>
              </DataGridHeader>
              {_.map(searchResults, (result, idx) => (
                <DataGridRow
                  key={`result_${idx}`}
                  onClick={() =>
                    this.handleClick(`/${result.docType}/${result.id}`)
                  }
                >
                  <DataGridItem>
                    <Link to={`/${result.docType}/${result.id}`}>
                      {result.id}
                    </Link>
                  </DataGridItem>
                  {!scope ? (
                    <DataGridItem>{result.docType}</DataGridItem>
                  ) : null}
                  <DataGridItemDouble>{GetDetails(result)}</DataGridItemDouble>
                  <DataGridItem>
                    {_.map(result.tags, (tag, idx) => (
                      <span key={`tag_${idx}`}>{tag}&nbsp;</span>
                    ))}
                  </DataGridItem>
                </DataGridRow>
              ))}
            </div>
          ) : errors ? (
            <WarningText>
              Unable to complete your search. Please check your search syntax.
              You may need to quote or escape complex terms.
            </WarningText>
          ) : lastSearch !== null ? (
            <ResultText>Sorry, there are no matching records.</ResultText>
          ) : null}
        </SearchWrapper>
        <SearchHelp>
          <SearchTerms
            scope={scope}
            schema={schema}
            handleTermClick={this.handleTermClick}
          ></SearchTerms>
        </SearchHelp>
      </SearchLayout>
    );
  }
}

const GetDetails = result => {
  switch (result.docType) {
    case "Organization":
      return (
        <div>
          {result.name}
          <small>{result.details}</small>
        </div>
      );
    case "User":
      return (
        <div>
          {result.name}
          <small>{result.alias}</small>
          <small>{result.email}</small>
        </div>
      );
    case "Ticket":
      return (
        <div>
          {result.subject}
          {result.organization ? <div>{result.organization.name}</div> : null}
          <small>PRIORITY: {result.priority}</small>
          <small>STATUS: {result.status}</small>
          <small>TYPE: {result.type}</small>
          <small>VIA: {result.via}</small>
        </div>
      );
    default:
      return null;
  }
};

const SearchTerms = ({ scope, schema, handleTermClick }) => {
  if (!schema || !schema.user) {
    return null;
  }
  var terms;
  switch (scope) {
    case "Organization":
      terms = schema.organization;
      break;
    case "Ticket":
      terms = schema.ticket;
      break;
    case "User":
      terms = schema.user;
      break;
    default:
      terms = _.uniq([
        ...schema.ticket,
        ...schema.user,
        ...schema.organization
      ]).sort();
      break;
  }
  return (
    <div>
      <h5>{scope ? scope : "Global"} Search Terms</h5>
      <p>(click to use)</p>
      <ul>
        {_.map(terms.sort(), (term, idx) => (
          <li key={`term_${idx}`} onClick={() => handleTermClick(term)}>
            {term}
          </li>
        ))}
      </ul>
      <Link to={`/help`}>more help</Link>
    </div>
  );
};

const mapStateToProps = function(state) {
  return {
    search: state.search,
    searchSchema: state.searchSchema
  };
};

const mapDispatchToProps = dispatch =>
  bindActionCreators({ ...searchCreators, ...searchSchemaCreators }, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(Search);

const WarningText = styled.div`
  color: red;
  font-style: italic;
`;

const ResultText = styled.div`
  font-style: italic;
`;

const SearchInput = styled.input`
  border: 0,
  outline: 'none',
  height: 20px;
  width: 400px;
  background: transparent;
  font-size: 1rem;
  margin-left:10px;   
  padding: 0;
  transition: margin 250ms ease-out, padding 250ms ease-out;
`;

const SearchButton = styled.input`
  align-items: center;
  background: #fff;
  border: 1px solid;
  border-radius: 4px;
  color: #444;
  cursor: pointer;
  font-size: inherit;
  height: auto;
  justify-content: center;
  outline: none;
  margin-left: 10px;
  margin-right: 10px;
  padding: 0 1rem;
  position: relative;
  transition: all 0.2s linear;
  user-select: none;
  white-space: nowrap;
  &:hover {
    box-shadow: 0 1px 0 rgba(0, 0, 0, 0.06);
  }
`;

const SearchLayout = styled.div`
  &:after {
    content: "";
    display: table;
    clear: both;
  }
`;

const SearchWrapper = styled.div`
  border-radius: 4px;
  padding: 40px;
  h2 {
    font-weight: 500;
    margin: 0 0 24px;
  }
  float: left;
  width: 75%;
`;

const SearchHelp = styled.div`
  float: left;
  width: 25%;
  padding: 10px;
  background: #f6f8f9;
`;
