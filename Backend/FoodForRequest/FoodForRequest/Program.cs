using FluentAssertions.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using FoodForRequest.Data;
using FoodForRequest.Filters;
using FoodForRequest.Hubs;
using FoodForRequest.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Numerics;
using FoodForRequest.Controllers;
using FoodForRequest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ApiExceptionFilter>();
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<IFoodRequestRepository, FoodrequestRepository>();
builder.Services.AddTransient<IOfferRepository, OfferRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<IFoodUserRepository, FoodUserRepository>();
builder.Services.AddTransient<IingridientRepository, IngridientRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://example.com",
                                                          "http://www.contoso.com",
                                                          "http://127.0.0.1:5500",
                                                          "http://127.0.0.1",
                                                          "127.0.0.1",
                                                            "localhost:4200",
                                                            "127.0.0.1:4200",
                                                          "localhost",
                                                           "localhost:5500",
                                                            "127.0.0.1:5500",
                                                             "localhost:5501",
                                                             "*",
                                                               "http://127.0.0.1:5501",
                                                          "http://localhost",
                                                           "http://localhost:5500",
                                                            "http://localhost:5000"
                                                          )
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod()
                                                  .SetIsOriginAllowed((hosr) => true)
                                                    .AllowCredentials();
                                  });
});

builder.Services.AddDefaultIdentity<FoodUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
}
)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = "http://www.security.org",
        ValidIssuer = "http://www.security.org",
        IssuerSigningKey = new SymmetricSecurityKey
      (Encoding.UTF8.GetBytes("FoodForRequestSecurityKey"))
    };
});

builder.Services.AddControllersWithViews();


builder.Services.AddSignalR();


builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(x => x
.AllowCredentials()
.AllowAnyMethod()
.AllowAnyHeader()
.WithOrigins("http://localhost:4200")


);

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<SignalRHub>("/hub").RequireCors(MyAllowSpecificOrigins);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FoodRequest}/{action=Index}/{id?}");
app.MapRazorPages();




app.Run();
