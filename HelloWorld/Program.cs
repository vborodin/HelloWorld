using HelloWorld.Startup;

WebApplication
    .CreateBuilder(args)
    .RegisterServices()
    .RegisterSwagger()
    .RegisterAuthentication()
    .Build()
    .ConfigurePipeline()
    .Run();