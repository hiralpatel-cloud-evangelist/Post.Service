using Microsoft.Extensions.DependencyInjection;
using Post.Service.Base.HelperClasses;
using Post.Service.Base.HelperClasses.IHelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Service.Base.Extensions
{
    public static class APIConfigurationExtensions
    {
        public static void APIBaseContexts(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
