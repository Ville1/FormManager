using FormManager.Data;
using FormManager.Services;
using FormManager.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();Swagger removed

builder.Services.AddMvc(setupAction => { setupAction.Filters.Add(new RequireHttpsAttribute()); });

//Set config
Config.Configuration = builder.Configuration;

//Add database
builder.Services.AddDbContext<Database>(options => { options.UseSqlServer(Config.ConnectionString); });

//Add database service
builder.Services.AddScoped<DatabaseService>();

//Add cookie based authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
    options.LoginPath = "/Login";
});

//Add authorization policys
builder.Services.AddAuthorization(options => {
    options.AddPolicy(FormManager.Controllers.AuthPolicy.Basic, policy => policy.RequireClaim("UserId"));
});

builder.WebHost.UseWebRoot("wwwroot");

var app = builder.Build();

//Swagger removed
// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
