namespace HelloWorld.Startup;

public static class DependencyInjectionSetup
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        return builder;
    }
}
