namespace AuthenticationService.Startup;

public static class SwaggerSetup
{
    public static WebApplicationBuilder RegisterSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        return builder;
    }
}
