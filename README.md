# TicketApp

Demonstration of a basic help desk search application.

This application includes:
- A GraphQL API written in C# that runs on dotnet core 2.2
  - An in-memory data persistence layer
  - An in-memory Lucene search engine
  - An embedded JSON data source to load demonstration data
- A React.js + Redux sample front end web application

# Option 1: Run in a Docker container
- Prerequisites
  - a recent version of [Docker](https://www.docker.com/products/docker-desktop) and [Docker Compose](https://docs.docker.com/compose/install/)
- Clone this repository
- From the root of the repository run `docker-compose up`
- Browse to http://localhost:10443 after Docker Compose has built and started the container.  If port 10443 is already in use on your local machine, edit the docker-compose.yml file and change the exposed container port.

# Option 2: Full developer setup
- Prerequisites
  - a recent version of [npm](https://www.npmjs.com/get-npm)
  - [.Net Core 2.2 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- Clone this repository
- Use your favorite IDE to start or debug the application.
  - Visual Studio 2019 or 2017
    - Open TicketApp.sln and hit F5 or Ctrl+F5
  - Visual Studio Code and other IDEs
    - Open a terminal in /src/TicketApi/ReactApp and run `npm i`
    - Open a terminal in the repository root and run `dotnet run`
- Browse to http://localhost:34401 after the application has started.  If port 34401 is already in use on your local machine, edit the appsettings.Development.json file and change the configured Kestrel HTTP port.
    
# Continuous integration and testing
- A GitHub [Action](https://github.com/barryhagan/TicketApp/actions) is configured to run unit and integration tests on any push to the master branch
- You can run tests locally by running `dotnet test` from the repository root


