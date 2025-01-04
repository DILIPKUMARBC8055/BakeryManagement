using BakeryApp.Application.Handlers.OrderHandlers.QueryHandler;
using BakeryApp.Application.Mappers;
using BakeryApp.Core.Repositary;

using Microsoft.IdentityModel.Tokens;
using BakeryApp.Infrastructure.Data;

using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using BakeryApp.Application.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
        };
    });

builder.Services.AddAuthorization();

var assembly = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(GetAllOrdersHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));

builder.Services.AddAutoMapper(typeof(BakeryMappingProfile).Assembly);

// Add services to the container.
builder.Services.AddSingleton<LiteDbContext>();
builder.Services.AddSingleton<IOrderRepo, OrderRepository>();
builder.Services.AddSingleton<IBakeryItemRepo, BakeryItemRepo>();
builder.Services.AddTransient<PasswordService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<IUserRepo, UserRepositary>();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 401)
    {
        response.ContentType = "application/json";
        return response.WriteAsync("{\"error\": \"Unauthorized\"}");
    }
    if (response.StatusCode == 403)
    {
        response.ContentType = "application/json";
        return response.WriteAsync("{\"error\": \"Forbidden\"}");
    }

    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
