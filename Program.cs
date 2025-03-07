//using Grpc.Net.Client;
using Microsoft.OpenApi.Models;
using ProductAPI.Protos;
using ProductAPI.ProtoServiceImplementation;

namespace CustomerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddGrpcClient<ProductService.ProductServiceClient>(options =>
            {
                // Set the address of the ProductApi gRPC server
                options.Address = new Uri("https://localhost:7077"); // My ProductApi URL
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer API", Version = "v1" });

                // to handle the conflict when multiple controllers have actions with the same HTTP method and path.
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
