using System.Text;
using AdminSystem.Core.Entities;
using AdminSystem.Core.Interfaces;
using AdminSystem.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=adminsystem.db";

var dbType = builder.Configuration.GetValue<string>("DatabaseType") ?? "sqlite";

builder.Services.AddSingleton<JwtSettings>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new JwtSettings
    {
        SecretKey = config["Jwt:SecretKey"] ?? "YourSecretKeyForJwtTokenMustBeAtLeast32CharactersLong!",
        Issuer = config["Jwt:Issuer"] ?? "AdminSystem",
        Audience = config["Jwt:Audience"] ?? "AdminSystem",
        ExpireDays = config.GetValue<int>("Jwt:ExpireDays", 7)
    };
});

ISqlSugarClient sqlSugar;
switch (dbType.ToLower())
{
    case "mysql":
        sqlSugar = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });
        break;
    case "postgresql":
        sqlSugar = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = DbType.PostgreSQL,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });
        break;
    case "sqlserver":
        sqlSugar = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = DbType.SqlServer,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });
        break;
    default:
        sqlSugar = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = DbType.Sqlite,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });
        break;
}

builder.Services.AddSingleton<ISqlSugarClient>(sqlSugar);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrgService, OrgService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IRoleService, RoleService>();

var jwtSettings = builder.Services.BuildServiceProvider().GetRequiredService<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdminSystem API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

await InitDatabase(sqlSugar);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminSystem API V1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

async Task InitDatabase(ISqlSugarClient db)
{
    db.CreateTableAsync<SysUser>().Wait();
    db.CreateTableAsync<SysOrg>().Wait();
    db.CreateTableAsync<SysMenu>().Wait();
    db.CreateTableAsync<SysRole>().Wait();
    db.CreateTableAsync<SysUserRole>().Wait();
    db.CreateTableAsync<SysRoleMenu>().Wait();

    var userCount = await db.Queryable<SysUser>().CountAsync();
    if (userCount == 0)
    {
        var adminUser = new SysUser
        {
            Username = "admin",
            Password = HashPassword("123456"),
            Nickname = "超级管理员",
            Status = 1,
            CreateTime = DateTime.Now
        };
        await db.Insertable(adminUser).ExecuteCommandAsync();

        var orgs = new List<SysOrg>
        {
            new() { OrgName = "总公司", OrgCode = "HQ", ParentId = 0, Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { OrgName = "技术部", OrgCode = "TECH", ParentId = 1, Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { OrgName = "运营部", OrgCode = "OPS", ParentId = 1, Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { OrgName = "财务部", OrgCode = "FIN", ParentId = 1, Sort = 3, Status = 1, CreateTime = DateTime.Now }
        };
        await db.Insertable(orgs).ExecuteCommandAsync();

        var menus = new List<SysMenu>
        {
            new() { MenuName = "工作台", MenuCode = "dashboard", MenuType = 1, Path = "/dashboard", Component = "/dashboard/index", Icon = "HomeFilled", ParentId = 0, Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "系统管理", MenuCode = "system", MenuType = 0, Path = "/system", Icon = "Setting", ParentId = 0, Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "用户管理", MenuCode = "user", MenuType = 1, Path = "/system/user", Component = "/system/user/index", Icon = "User", ParentId = 2, Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "角色管理", MenuCode = "role", MenuType = 1, Path = "/system/role", Component = "/system/role/index", Icon = "UserFilled", ParentId = 2, Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "菜单管理", MenuCode = "menu", MenuType = 1, Path = "/system/menu", Component = "/system/menu/index", Icon = "Menu", ParentId = 2, Sort = 3, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "组织管理", MenuCode = "org", MenuType = 1, Path = "/system/org", Component = "/system/org/index", Icon = "OfficeBuilding", ParentId = 2, Sort = 4, Status = 1, CreateTime = DateTime.Now }
        };
        await db.Insertable(menus).ExecuteCommandAsync();

        var roles = new List<SysRole>
        {
            new() { RoleName = "超级管理员", RoleCode = "super_admin", Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { RoleName = "普通用户", RoleCode = "user", Sort = 2, Status = 1, CreateTime = DateTime.Now }
        };
        await db.Insertable(roles).ExecuteCommandAsync();

        var roleMenus = new List<SysRoleMenu>
        {
            new() { RoleId = 1, MenuId = 1 },
            new() { RoleId = 1, MenuId = 2 },
            new() { RoleId = 1, MenuId = 3 },
            new() { RoleId = 1, MenuId = 4 },
            new() { RoleId = 1, MenuId = 5 },
            new() { RoleId = 1, MenuId = 6 }
        };
        await db.Insertable(roleMenus).ExecuteCommandAsync();

        var userRoles = new List<SysUserRole>
        {
            new() { UserId = 1, RoleId = 1 }
        };
        await db.Insertable(userRoles).ExecuteCommandAsync();
    }
}

string HashPassword(string password)
{
    using var sha256 = System.Security.Cryptography.SHA256.Create();
    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    return Convert.ToBase64String(hashedBytes);
}
