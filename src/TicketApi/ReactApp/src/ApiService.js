class ApiService {
  constructor() {
    this.apiUrl = "/graphql";
  }

  async getGraphQlData(query) {
    const res = await fetch(this.apiUrl, {
      method: "POST",
      mode: "cors",
      headers: new Headers({
        "Content-Type": "application/json",
        Accept: "application/json"
      }),
      body: JSON.stringify({ query })
    });
    if (res.ok) {
      const body = await res.json();

      if (body.errors) {
        throw new Error(body.errors[0].message);
      }

      return body.data;
    } else {
      throw new Error(res.status);
    }
  }

  async getSearchSchema() {
    const data = await this.getGraphQlData(
      "{ searchSchema { organization ticket user } } "
    );
    return data.searchSchema;
  }

  async getSearchResults(searchInput) {
    var input = "{ search:" + JSON.stringify(searchInput.search);
    if (searchInput.scope) {
      input += " docType:" + JSON.stringify(searchInput.scope);
    }
    input += " }";
    const data = await this.getGraphQlData(
      "{ globalSearch(input:" +
        input +
        ") { organizations {score item{id details external_id name tags}} tickets {score item{id subject via status external_id tags via priority type organization {name} }}  users {score item { id external_id name alias email created_at tags} } } } "
    );
    return data.globalSearch;
  }

  async getUser(id) {
    const data = await this.getGraphQlData(
      "{ user(id:" +
        id +
        ") { id active alias assignedTickets { id subject status } created_at email external_id last_login_at locale name organization { id name } phone role shared signature submittedTickets { id subject status } suspended tags timezone url verified } } "
    );
    return data.user;
  }

  async getOrganization(id) {
    const data = await this.getGraphQlData(
      "{ organization(id:" +
        id +
        ") { id created_at details domain_names external_id name, shared_tickets tags tickets { id subject status } url users { id name email } } } "
    );
    return data.organization;
  }

  async getTicket(id) {
    const data = await this.getGraphQlData(
      '{ ticket(id:"' +
        id +
        '") { id  assignee { id name email } created_at description due_at external_id has_incidents organization { id name } priority status subject submitter { id name email } tags type url via } } '
    );
    return data.ticket;
  }
}

export default new ApiService();
