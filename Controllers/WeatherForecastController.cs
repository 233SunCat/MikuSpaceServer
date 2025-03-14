using Microsoft.AspNetCore.Mvc;
using Miku.Log4Net;
using Plugin;
using System.Configuration;
using System.Reflection;

namespace MikuSpaceServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly Assembly _pluginAssembly;
        private readonly ILoggerTest _loggerTest;
        public WeatherForecastController(ILoggerTest loggerTest,ILogger<WeatherForecastController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _pluginAssembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Plugins/TestUnit.dll"));
            _loggerTest = loggerTest;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _loggerTest.Log("������־�����");
            //��ȫ����
            // ��ȡ�������зǳ�����
            //var types = _pluginAssembly.GetTypes().Where(t => !t.IsAbstract);

            //foreach (var type in types)//type {Name:"����,FullName:"�����ռ�.����"}
            //{
            //    if (type.Name == "Class1") {
            //        var service = _serviceProvider.GetService(type);//DI������ȡ����ʵ��
            //        // ��ȷ��ȡ������int�����ķ����������������⣩
            //        var method = type.GetMethod("add", new[] { typeof(int), typeof(int) });
            //        if (method != null)
            //        {
            //            object[] parameters = new object[] { 10, 20 };
            //            var result = (int)method.Invoke(service, parameters);

            //            // �����Web��Ŀ������ע��ILogger���м�¼
            //            // _logger.LogInformation($"��������{result}");
            //        }
            //    }

            //}
            ////�����(��Ҫ�ֶ����빫�����(�ӿ�))
            //Assembly assembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Plugins/UserCustom.dll"));
            //var pluginType = assembly.GetTypes().FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            //if (pluginType != null)
            //{
            //    IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginType);

            //    plugin.EllisTest();
            //}



            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
