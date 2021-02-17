using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooMag.ViewModels;

namespace ZooMag.Services.Interfaces
{
    public interface ICartService
    {
        Task<Response> Add();
        Task<Response> Delete();
    }
}
