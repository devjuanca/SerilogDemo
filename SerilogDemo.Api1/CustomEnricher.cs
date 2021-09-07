using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using SerilogDemo.Api1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerilogDemo.Api1
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
            var moduleProperty = propertyFactory.CreateProperty("Module", "Api1 - Weather");
            logEvent.AddPropertyIfAbsent(moduleProperty);
            
            if (_currentUserService.CurrentUser == null)
                return;

            var userName = _currentUserService.CurrentUser;
            var userNameProperty = propertyFactory.CreateProperty("UserName", userName);
            logEvent.AddPropertyIfAbsent(userNameProperty);



        }
    }
}
