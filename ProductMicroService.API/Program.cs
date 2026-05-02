using DataAccessLayer;
using BusinessLogicLayer;
using ProductMicroService.API.Middleware;
using ProductMicroService.API.APIEndpoints;

var builder = WebApplication.CreateBuilder(args);

// Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

// Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapProductAPIEndpoints();

app.Run();

