using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(FYstudentMgr.Startup))]

namespace FYstudentMgr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           //var  cors=new CorsOptions()
           // {
           //      CorsEngine= "http://localhost:4200"
           // }
           // app.UseCors(cors);
            ConfigureAuth(app);
        }
    }
}
