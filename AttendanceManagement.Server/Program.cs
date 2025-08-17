using System.Text;
using AttendanceManagement.Server.Schema;
using AttendanceManagement.Server.Services;
using Data.DbContexts;
using Data.Entities;
using Library;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiService.Common.Const;

// Shift-JIS使用許可
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ログトレース設定
var loggerPrefix = builder.Configuration["LoggerPrefix"] ?? "AttendanceManagement";

// Logger を初期化
Logger.Initialize(loggerPrefix);

// サービス登録
builder.Services.AddControllers();

// EF Core登録
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// ユーザ認証サービス登録（Identity）
builder.Services.AddIdentity<Users, IdentityRole>().AddEntityFrameworkStores<MyDbContext>();

int tokenLockoutTimeSpanDay = int.Parse(builder.Configuration["TokenLockoutTimeSpanDay"] ?? "1");

// Identity設定
builder.Services.Configure<IdentityOptions>(options =>
{
    // パスワード設定
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 1;

    // ロックアウト設定
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;

    // ユーザー設定
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    options.User.RequireUniqueEmail = false;

    // サインインの設定
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

// 認証トークン取得
var tokenKey = builder.Configuration["JwtSettings:TokenKey"];
if (string.IsNullOrWhiteSpace(tokenKey))
{
    throw new ArgumentNullException(nameof(tokenKey));
}

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

// 認証JWTトークン設定
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(opt =>
               {
                   opt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = key,
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero
                   };
               });

// 認証サービス登録
builder.Services.AddScoped<AuthServiceJWT>();

// CORS登録
var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (corsOrigins != null && corsOrigins.Length > 0)
        {
            policy.WithOrigins(corsOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .WithExposedHeaders(CustomHeaderPrefixes.AuthToken.HeaderName);
        }
        else
        {
            // 設定がなければ全て許可
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .WithExposedHeaders(CustomHeaderPrefixes.AuthToken.HeaderName);
        }
    });
});

// Swaggerサービスを追加
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AttendanceManagement API", Version = "v1", Description = "APIドキュメント" });

    // 属性（アノテーション）ベースの拡張情報表示
    c.EnableAnnotations();

    // XMLコメントファイルの読み込み
    var xmlDirectory = PathHelper.GetPathFromAncestor(AppContext.BaseDirectory, 2, "XML");

    if (Directory.Exists(xmlDirectory))
    {
        foreach (var xmlFile in Directory.GetFiles(xmlDirectory, "*.xml"))
        {
            c.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
        }
    }

    // カスタムスキーマフィルター
    c.OperationFilter<SwaggerResultExampleFilter>();

    // JWT認証の設定も追加可能
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme { Name = "Authorization", Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey, Scheme = "Bearer", BearerFormat = "JWT", In = Microsoft.OpenApi.Models.ParameterLocation.Header, Description = "JWT Authorization header using the Bearer scheme." });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement { { new Microsoft.OpenApi.Models.OpenApiSecurityScheme { Reference = new Microsoft.OpenApi.Models.OpenApiReference { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } });
});

WebApplication app = builder.Build();

// Swaggerミドルウェア有効化
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AttendanceManagement API V1");
    c.RoutePrefix = "swagger";
});

// 認証ミドルウェア有効
app.UseRouting();

// CORS有効
app.UseCors("AllowFrontend");
// app.UseCors("AllowAll");

// 認証・承認ミドルウェアを有効化（JWT認証に必須）
app.UseAuthentication();
app.UseAuthorization();

// 静的ファイルの既定ファイル有効化
app.UseDefaultFiles();
app.MapStaticAssets();

//// HTTP 要求を HTTPS にリダイレクト
// app.UseHttpsRedirection();

// ルーティング登録
app.MapControllers();

// ファイルの提供
app.MapFallbackToFile("/index.html");

app.Run();