using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TESTCRUDNET6.Data;
using TESTCRUDNET6.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true; // Nếu bạn muốn JSON được định dạng
}); ;*/

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
/*builder.Services.AddSwaggerGen();*/

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Book API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddDbContext<MyDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
});

builder.Services.AddScoped<IcategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAccountRepository, Accountrepository>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.WithOrigins("*")
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.SaveToken = true;
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

/*
 
- AddAuthentication: Thêm dịch vụ xác thực vào container DI (Dependency Injection) của ứng dụng.

- DefaultAuthenticateScheme: Xác định phương thức xác thực mặc định, trong trường hợp này là xác thực thông qua JWT Bearer.

- DefaultChallengeScheme: Xác định phương thức được sử dụng để thách thức (challenge) người dùng nếu không được xác thực.

- DefaultScheme: Xác định phương thức xác thực mặc định cho tất cả các yêu cầu. 

- AddJwtBearer: Thêm dịch vụ xác thực JWT Bearer.

- SaveToken: Khi được đặt là true, nó sẽ lưu token vào AuthenticationProperties, cho phép bạn sử dụng token trong các yêu cầu sau.

- RequireHttpsMetadata: Nếu được đặt là false, ứng dụng có thể nhận token qua HTTP không an toàn (HTTP). 
Thông thường, bạn nên đặt true cho môi trường sản xuất.

- TokenValidationParameters: Chứa các tham số để xác thực token:

- ValidateIssuer: Xác nhận rằng token được phát hành bởi một nhà phát hành đáng tin cậy.

- ValidateAudience: Xác nhận rằng token là cho một khán giả (audience) nhất định.

- ValidAudience: Xác định giá trị khán giả hợp lệ, được lấy từ cấu hình.

- ValidIssuer: Xác định giá trị nhà phát hành hợp lệ, cũng được lấy từ cấu hình.

- IssuerSigningKey: Khóa dùng để ký token, được tạo từ chuỗi bí mật trong cấu hình.

 */



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

/*app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads" // Cấu hình đường dẫn yêu cầu
});*/

app.UseStaticFiles();

app.MapControllers();

app.Run();
