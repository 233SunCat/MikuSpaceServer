using MikuSpaceServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
