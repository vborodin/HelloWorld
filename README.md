# HelloWorld
The solution consists of two projects.
#### HelloWorld
A test project with the single endpoint /hello that requires the client to be authenticated with a bearer token.
#### Authorization Service
An authorization microservice that provides functionality for managing users and roles.
## Local developer setup
#### Requirements
1. Docker
2. Visual Studio
3. dotnet-ef
#### Database setup
1. Open solution in VS and set `docker-compose` startup project. All required containers will be built and started automatically.
2. Perform database migration:
```
cd .\AuthenticationService\
dotnet ef database update --connection "server=localhost;port={host port};database=postgres;username=postgres;password={password}"
```
#### Application setup
To launch the application use `docker-compose` startup project and `Application` launch profile
#### After first launch
Create the first user, the User role will be assigned automatically. Access the database with the adminer and set the Administrator role.
#### SSL certificates
See [guide from Microsoft](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide)
