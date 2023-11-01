using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PIMTool.Entities;
using PIMTool.Repositories;
using PIMTool.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddDI(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

void AddDI(IServiceCollection services)
{
    services.AddScoped<GroupRepository>();
    services.AddScoped<IGroupService, GroupService>();
    services.AddScoped<EmployeeRepository>();
    services.AddScoped<IEmployeeService, EmployeeService>();
    services.AddScoped<ProjectRepository>();
    services.AddScoped<IProjectService, ProjectService>();
    services.AddScoped<AppDbContext>();
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddAutoMapper(typeof(Program).Assembly);

}