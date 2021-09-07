using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using SerilogDemo.Api2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerilogDemo.Api2
{
    public class CustomEnricher : ILogEventEnricher
    {
        readonly ICurrentUserService _currentUserService;
        
        public CustomEnricher(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
           
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var moduleProperty = propertyFactory.CreateProperty("Module", "Api2 - Traffic");
            logEvent.AddPropertyIfAbsent(moduleProperty);

            //var moduleIPProperty = propertyFactory.CreateProperty("IP", _currentUserService.Accessor.HttpContext?.Connection.RemoteIpAddress);
            //logEvent.AddPropertyIfAbsent(moduleIPProperty);


            if (_currentUserService.CurrentUser == null)
                return;

            var userName = _currentUserService.CurrentUser;
            var userNameProperty = propertyFactory.CreateProperty("UserName", userName);
            logEvent.AddPropertyIfAbsent(userNameProperty);



        }
    }
}
