using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Web.Administration;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                Configuration config = serverManager.GetApplicationHostConfiguration();

                ConfigurationSection dynamicIpSecuritySection = config.GetSection("system.webServer/security/dynamicIpSecurity");
                dynamicIpSecuritySection["denyAction"] = @"Forbidden";
                dynamicIpSecuritySection["enableProxyMode"] = true;
                dynamicIpSecuritySection["enableLoggingOnlyMode"] = true;

                ConfigurationElement denyByConcurrentRequestsElement = dynamicIpSecuritySection.GetChildElement("denyByConcurrentRequests");
                denyByConcurrentRequestsElement["enabled"] = true;
                denyByConcurrentRequestsElement["maxConcurrentRequests"] = 10;

                ConfigurationElement denyByRequestRateElement = dynamicIpSecuritySection.GetChildElement("denyByRequestRate");
                denyByRequestRateElement["enabled"] = true;
                denyByRequestRateElement["maxRequests"] = 10;
                denyByRequestRateElement["requestIntervalInMilliseconds"] = 10;

                serverManager.CommitChanges();

                CreateHostBuilder(args).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
