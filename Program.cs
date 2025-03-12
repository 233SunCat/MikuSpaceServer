using MikuSpaceServer.Hubs;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 加载目标程序集（以 TestUnit.dll 为例）
var pluginPath = Path.Combine(AppContext.BaseDirectory, "Plugins", "TestUnit.dll");
var assembly = Assembly.LoadFrom(pluginPath);
// 扫描并批量注册服务
foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract))
{
    // 注册类自身（适用于无接口的类）
    builder.Services.AddScoped(type);

    // 或者注册为实现的第一个接口（如果有）
    var interfaces = type.GetInterfaces();
    if (interfaces.Length > 0)
    {
        builder.Services.AddScoped(interfaces[0], type);
    }
}



// 添加 SignalR 服务
builder.Services.AddSignalR();

// 允许跨域
string[] allowedOrigins = new[] { "http://localhost:5173", "https://localhost:5173" };
builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();

    });
});

builder.WebHost.UseUrls("http://*:5234"); // 明确指定监听所有IP的5234端口

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("CorsPolicy");

// 配置 SignalR 路由
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

app.Run();
