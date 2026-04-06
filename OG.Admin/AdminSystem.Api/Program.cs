using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SqlSugar;
using AdminSystem.Core.DTOs;
using AdminSystem.Core.Entities;
using AdminSystem.Core.Interfaces;
using AdminSystem.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdminSystem API", Version = "v1" });
});

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=adminsystem.db";
var dbTypeStr = builder.Configuration.GetValue<string>("DatabaseType") ?? "sqlite";

DbType dbType = dbTypeStr.ToLower() switch
{
    "mysql" => DbType.MySql,
    "postgresql" => DbType.PostgreSQL,
    "sqlserver" => DbType.SqlServer,
    _ => DbType.Sqlite
};

var sqlSugar = new SqlSugarClient(new ConnectionConfig
{
    ConnectionString = connectionString,
    DbType = dbType,
    IsAutoCloseConnection = true,
    InitKeyType = InitKeyType.Attribute
});

builder.Services.AddSingleton<ISqlSugarClient>(sqlSugar);

// JWT Settings
var jwtSettings = new JwtSettings
{
    SecretKey = builder.Configuration["Jwt:SecretKey"] ?? "YourSecretKeyForJwtTokenMustBeAtLeast32CharactersLong!",
    Issuer = builder.Configuration["Jwt:Issuer"] ?? "AdminSystem",
    Audience = builder.Configuration["Jwt:Audience"] ?? "AdminSystem",
    ExpireDays = builder.Configuration.GetValue<int>("Jwt:ExpireDays", 7)
};
builder.Services.AddSingleton(jwtSettings);

// Authentication
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

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrgService, OrgService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IRoleService, RoleService>();

var app = builder.Build();

// Initialize database
InitDatabase(sqlSugar);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

void InitDatabase(ISqlSugarClient db)
{
    // Create tables using CodeFirst
    db.CodeFirst.InitTables<SysUser>();
    db.CodeFirst.InitTables<SysOrg>();
    db.CodeFirst.InitTables<SysMenu>();
    db.CodeFirst.InitTables<SysRole>();
    db.CodeFirst.InitTables<SysUserRole>();
    db.CodeFirst.InitTables<SysRoleMenu>();

    // Seed data if empty
    var userCount = db.Queryable<SysUser>().Count();
    if (userCount == 0)
    {
        // Insert admin user
        var adminUser = new SysUser
        {
            Username = "admin",
            Password = BCrypt.Net.BCrypt.HashPassword("123456"),
            Nickname = "超级管理员",
            Email = "admin@example.com",
            Status = 1,
            CreateTime = DateTime.Now
        };
        db.Insertable(adminUser).ExecuteCommand();

        // Insert orgs
        var orgs = new List<SysOrg>
        {
            new() { OrgName = "总公司", OrgCode = "HQ", ParentId = 0, Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { OrgName = "技术部", OrgCode = "TECH", ParentId = 1, Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { OrgName = "运营部", OrgCode = "OPS", ParentId = 1, Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { OrgName = "财务部", OrgCode = "FIN", ParentId = 1, Sort = 3, Status = 1, CreateTime = DateTime.Now }
        };
        db.Insertable(orgs).ExecuteCommand();

        // Insert menus
        var menus = new List<SysMenu>
        {
            new() { MenuName = "工作台", MenuCode = "dashboard", MenuType = 1, Path = "/dashboard", Component = "/dashboard/index", Icon = "HomeFilled", ParentId = 0, Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "系统管理", MenuCode = "system", MenuType = 0, Path = "/system", Icon = "Setting", ParentId = 0, Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "用户管理", MenuCode = "user", MenuType = 1, Path = "/system/user", Component = "/system/user/index", Icon = "User", ParentId = 2, Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "角色管理", MenuCode = "role", MenuType = 1, Path = "/system/role", Component = "/system/role/index", Icon = "UserFilled", ParentId = 2, Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "菜单管理", MenuCode = "menu", MenuType = 1, Path = "/system/menu", Component = "/system/menu/index", Icon = "Menu", ParentId = 2, Sort = 3, Status = 1, CreateTime = DateTime.Now },
            new() { MenuName = "组织管理", MenuCode = "org", MenuType = 1, Path = "/system/org", Component = "/system/org/index", Icon = "OfficeBuilding", ParentId = 2, Sort = 4, Status = 1, CreateTime = DateTime.Now }
        };
        db.Insertable(menus).ExecuteCommand();

        // Insert roles
        var roles = new List<SysRole>
        {
            new() { RoleName = "超级管理员", RoleCode = "super_admin", Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { RoleName = "普通用户", RoleCode = "user", Sort = 2, Status = 1, CreateTime = DateTime.Now }
        };
        db.Insertable(roles).ExecuteCommand();

        // Insert role-menus
        var roleMenus = new List<SysRoleMenu>
        {
            new() { RoleId = 1, MenuId = 1 },
            new() { RoleId = 1, MenuId = 2 },
            new() { RoleId = 1, MenuId = 3 },
            new() { RoleId = 1, MenuId = 4 },
            new() { RoleId = 1, MenuId = 5 },
            new() { RoleId = 1, MenuId = 6 }
        };
        db.Insertable(roleMenus).ExecuteCommand();

        // Assign admin to super_admin role
        var userRoles = new List<SysUserRole>
        {
            new() { UserId = 1, RoleId = 1 }
        };
        db.Insertable(userRoles).ExecuteCommand();
    }
}
