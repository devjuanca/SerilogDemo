using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerilogDemo.Api2.Services
{
    public interface ICurrentUserService
    {
        string CurrentUser { get; set; }
        //IHttpContextAccessor Accessor { get; set; }
    }

    public class CurrentUserService : ICurrentUserService
    {
        //IHttpContextAccessor _accesor;


        //public CurrentUserService(IHttpContextAccessor accesor)
        //{
        //    _accesor = accesor;
        //}

        public string CurrentUser { get; set; } = "test_user_Traffic";
        //public IHttpContextAccessor Accessor
        //{
        //    get
        //    {
        //        return _accesor;
        //    }
        //    set { _accesor = value; }
        //}


    }

}
