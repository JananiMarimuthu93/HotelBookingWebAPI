using HotelBookingAPI.Context;
using HotelBookingAPI.Interfaces;
using HotelBookingAPI.Repositories.Implementations;
using HotelBookingAPI.Repositories.Interfaces;
using HotelBookingAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Main method: the entry point of the application.

//Create a new builder for the web application using the command-line arguments provided.
var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container.
// Here, MVC controllers are being added, which handle web API requests.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Registering Swagger services to help generate API documentation and provide an API explorer.

// Register the necessary services to enable API exploration capabilities
// which Swashbuckle utilizes to generate Swagger documentation
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

// Add Swagger generator services to the services container.
// This will be used to produce the Swagger document (OpenAPI spec) and the Swagger UI
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<HotelBookingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Register Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// Register RoomType Repository & Service
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<RoomTypeService>();
// Room specific
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<RoomService>();
//Guest specific
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<GuestService>();
//User specific
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
//Role Specific
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<RoleService>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<BookingService>();

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddScoped<IToken, TokenService>();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//// Build the web application using the configurations defined above.
var app = builder.Build();

//3.Middleware Configuration
// Configure the HTTP request pipeline, which handles incoming HTTP requests.
// Register Swagger middleware components if in development environment
if (app.Environment.IsDevelopment())
{
    //UseSwagger middleware to serve the generated Swagger as a JSON endpoint
    app.UseSwagger();
    // UseSwaggerUI middleware to serve the Swagger UI.
    // Swagger UI fetches the Swagger JSON to generate a visual documentation of the API.
    app.UseSwaggerUI();
}

// // Middleware to redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();
app.UseAuthentication();
// Middleware to enforce authorization policies on requests.
app.UseAuthorization();
// Map controller routes. This makes the application aware of the routes defined in the controllers
app.MapControllers();
// Run the application and start listening for incoming HTTP requests.
app.Run();
