using AuthenticationService.Startup;

WebApplication
    .CreateBuilder(args)
    .RegisterServices()
    .RegisterDatabase()
    .RegisterSwagger()
    .RegisterAuthentication()
    .Build()
    .ConfigurePipeline()
    .Run();
