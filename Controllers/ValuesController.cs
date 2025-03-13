using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MikuSpaceServer.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MikuSpaceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IServiceProvider provider;
        public ValuesController(IServiceProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet("login")]
        public async Task<object> getLogin(string name, string password)
        {

            Debugger.Log(0, null, "测试调式");
            //Console.Read();

            await Task.Delay(1000);
            //验证用户名和密码
            if (name == "admin" && password == "123456")
            {

                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, "ClaimType"),
                new Claim("UserId", "abcd"),
                new Claim("RoleValue","admin")
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MikuSpaceServer"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    "MikuSpaceServer",
                    "MikuSpaceServer",//traceSource : new TraceSource("MikuSpaceServer"),
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return tokenString;
            }
            else
            {
                return "用户名或密码错误";
            }

        }
        [Authorize (Roles = "admin")]
        [HttpPost("test")]
        public async Task<object> postTest(string name, string password) { 
            await Task.Delay(1000);
            return "success";
        }

        [HttpPost("test_traceSource")]
        public async Task<object> postTestTraceSource() { 
            await Task.Delay(1000);
            //创建跟踪实例  
            var source  = new TraceSource("test", SourceLevels.Warning);//最低warning级别
            //获得事件所有类型
            var types = (TraceEventType[])Enum.GetValues(typeof(TraceEventType));//TraceEventType是枚举类型
            var eventId = 1;
            //把事件写入日志
            foreach (var type in types) { 
                source.TraceEvent(type, eventId++, "test traceSource");
            }
            //vs控制台输出
            //test Critical: 1 : test traceSource
            //test Error: 2 : test traceSource
            //test Warning: 3 : test traceSource

            IDictionary<string, ILogger> dict = new Dictionary<string, ILogger>();
            IList<LogInfo> LogInfos = new List<LogInfo>();

            foreach (LogInfo info in LogInfos.OrderBy(m => m.CreatedTime))
            {
                if (!dict.TryGetValue(info.LogName, out ILogger logger))
                {
                    ILoggerFactory factory = provider.GetRequiredService<ILoggerFactory>();
                    logger = factory.CreateLogger(info.LogName);
                    dict[info.LogName] = logger;
                }

                switch (info.LogLevel)
                {
                    case LogLevel.Trace:
                        logger.LogTrace(info.Message);
                        break;
                    case LogLevel.Debug:
                        logger.LogDebug(info.Message);
                        break;
                    case LogLevel.Information:
                        logger.LogInformation(info.Message);
                        break;
                    case LogLevel.Warning:
                        logger.LogWarning(info.Message);
                        break;
                    case LogLevel.Error:
                        logger.LogError(info.Exception, info.Message);
                        break;
                    case LogLevel.Critical:
                        logger.LogCritical(info.Exception, info.Message);
                        break;
                }
            }



            return "success";
        }



        public class LogInfo
        {
            public LogLevel LogLevel { get; set; }

            public string Message { get; set; }

            public Exception Exception { get; set; }

            public string LogName { get; set; }

            public DateTime CreatedTime { get; set; }
        }
    }


}
