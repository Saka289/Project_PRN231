using AutoMapper;
using CallService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Web.Services.InventoryAPI;
using Web.Services.InventoryAPI.Data;
using Web.Services.InventoryAPI.Extensions;
using Web.Services.InventoryAPI.Repository;
using Web.Services.InventoryAPI.Repository.IRepository;
using Web.Services.InventoryAPI.Service;
using Web.Services.InventoryAPI.Service.IService;
using Web.Services.OrderAPI.Service;
using Web.Services.OrderAPI.Service.IService;
using Web.Services.InventoryAPI.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddHttpClient<ISendService, SendService>();
builder.Services.AddHttpClient<IProductService, ProductService>().AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IIventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISendService, SendService>();

//Add services Auto Mapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= JwtBearerDefaults.AuthenticationScheme
                }
            },
            new List<string>()
        }
    });
});

builder.AddAppAuthencation();

builder.Services.AddAuthorization();

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

//ApplyMigration();
app.UseStaticFiles();

app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_context.Database.GetPendingMigrations().Count() > 0)
        {
            _context.Database.Migrate();
        }
    }
}