# TvMazeScraper & API

### Tools
- Visual Studio 2019
- SQL Server

### Technologies
- .Net 5
- Asp .Net Core 
- MS Unit test
- EntityFramework Core
- FluentValidation
- Polly


#### Download the source code 
https://github.com/humayun-ahmed/TvMazeScraper.git

#### Run the project
- Open the solution TvMaze.sln and build
- Set Api project as startup
- Change database connection string "TvMazeContext" in all appsettings*.json

#### Swagger
Swagger api documentation are available, endpoint is http://localhost:54438/swagger

#### Background service
2 background service:
1. ScraperHost nightly scheduler, will run every night at 10 PM.
2. ScraperHostInstant immediately run and will migrate database and start scraping the data from TvMaze api

One azure function ScraperFunction is there but it is not fully functional