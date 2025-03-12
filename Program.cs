using MikuSpaceServer.Hubs;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ����Ŀ����򼯣��� TestUnit.dll Ϊ����
var pluginPath = Path.Combine(AppContext.BaseDirectory, "Plugins", "TestUnit.dll");
var assembly = Assembly.LoadFrom(pluginPath);
// ɨ�貢����ע�����
foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract))
{
    // ע���������������޽ӿڵ��ࣩ
    builder.Services.AddScoped(type);

    // ����ע��Ϊʵ�ֵĵ�һ���ӿڣ�����У�
    var interfaces = type.GetInterfaces();
    if (interfaces.Length > 0)
    {
        builder.Services.AddScoped(interfaces[0], type);
    }
}



// ��� SignalR ����
builder.Services.AddSignalR();

// �������
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

builder.WebHost.UseUrls("http://*:5234"); // ��ȷָ����������IP��5234�˿�

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

// ���� SignalR ·��
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

app.Run();
