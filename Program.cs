using MikuSpaceServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
