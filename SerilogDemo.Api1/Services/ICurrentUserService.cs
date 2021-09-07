using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerilogDemo.Api1.Services
{
    public interface ICurrentUserService
    {
        string CurrentUser { get; set; }
    }

    public class CurrentUserService : ICurrentUserService
    {
        public string CurrentUser { get; set; } = "test_user_Weather";
    }

}
