using AzureTangyFunc;
using AzureTangyFunc.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly:WebJobsStartup(typeof(Startup))]
namespace AzureTangyFunc
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            string ConnectionString = Environment.GetEnvironmentVariable("LocalDatabase");
            builder.Services.AddDbContext<AzureTangyDbContext>(option => option.UseSqlServer(ConnectionString));

            builder.Services.BuildServiceProvider();
        }
    }
}
