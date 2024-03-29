using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MISA.Web.Core.Authentication;
using MISA.Web.Core.AutoMapper;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Interfaces.UnitOfWork;
using MISA.Web.Core.Services;
using MISA.Web.Infrastructure.Interface;
using MISA.Web.Infrastructure.MISADatabaseContext;
using MISA.Web.Infrastructure.Repositoty;
using MISA.Web.Infrastructure.UnitOfWork;
using OfficeOpenXml;
using System.Text;
Console.InputEncoding = Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
 string connectionString = configuration.GetConnectionString("DefaultConection");
var conection = builder.Configuration.GetConnectionString("DefaultConection");
// Thêm dịch vụ DbContext với MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(conection)));
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
// Thêm cấu hình CORS vào đây
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
}  );


// xử lý DI 
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>)); 
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IMISADbContext), typeof(MySqlDbcontext));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepsitory>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerGroupRepsitory, CustomerGroupRepsitory>();
builder.Services.AddScoped<ICustomerGroupRepsitory, CustomerGroupRepsitory>();
builder.Services.AddScoped<IDepartmentRepsitory, DepartmentRepsitory>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IPositionRepsitory, PositionRepsitory>();
builder.Services.AddScoped<IAuthenticationRepsitory, AuthenticationRepsitory>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAcountRepsitory, AccountRepsitory>();

builder.Services.AddScoped<IMISADbContext, MySqlDbcontext>();

builder.Services.AddScoped<IUnitOfWork>(provider=> new UnitOfWork(conection));

builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<HandeExceptionMiddleware>();
app.UseHttpsRedirection();
// Sử dụng cấu hình CORS ở đây
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");

app.MapControllers();
app.Run();
