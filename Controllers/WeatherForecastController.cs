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
            _loggerTest.Log("测试日志的输出");
            //完全解耦
            // 获取程序集所有非抽象类
            //var types = _pluginAssembly.GetTypes().Where(t => !t.IsAbstract);

            //foreach (var type in types)//type {Name:"类名,FullName:"命名空间.类名"}
            //{
            //    if (type.Name == "Class1") {
            //        var service = _serviceProvider.GetService(type);//DI容器获取服务实例
            //        // 精确获取带两个int参数的方法（避免重载问题）
            //        var method = type.GetMethod("add", new[] { typeof(int), typeof(int) });
            //        if (method != null)
            //        {
            //            object[] parameters = new object[] { 10, 20 };
            //            var result = (int)method.Invoke(service, parameters);

            //            // 如果是Web项目，可以注入ILogger进行记录
            //            // _logger.LogInformation($"计算结果：{result}");
            //        }
            //    }

            //}
            ////半解耦(需要手动引入公共类库(接口))
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
