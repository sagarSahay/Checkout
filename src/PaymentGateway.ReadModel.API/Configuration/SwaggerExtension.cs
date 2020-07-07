namespace PaymentGateway.ReadModel.API.Configuration
{
    using Microsoft.AspNetCore.Builder;

    public static class SwaggerExtension
    {
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Query API V1");
            });

            return app;
        }
    }
}