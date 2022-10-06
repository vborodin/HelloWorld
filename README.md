# HelloWorld
## Developer setup
#### Requirements
1. Docker
2. Visual Studio
3. dotnet-ef
#### Database setup
1. Build and launch database containers with `docker-compose` startup project and `Database` launch profile.
2. Perform database migration:
```
cd .\AuthenticationService\
dotnet ef database update --connection "server=localhost;port={host port};database=postgres;username=postgres;password={password}"
```
Host port can be found in `Containers` window in VS.
#### Application setup
To launch the application use `docker-compose` startup project and `Application` launch profile
