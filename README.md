# HelloWorld
The solution consists of two projects.
#### HelloWorld
A test project with the single endpoint /hello that requires the client to be authenticated with a bearer token.
#### Authorization Service
An authorization microservice that provides bearer tokens for registered users.
Contains two endpoints:
- api/account/login to log in with username and password
- api/account/register to create a user
## Local developer setup
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
#### SSL certificates
See [guide from Microsoft](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide)
