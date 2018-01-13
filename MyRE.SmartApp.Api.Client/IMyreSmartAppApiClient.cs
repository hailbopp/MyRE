using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyRE.SmartApp.Api.Client.Models;

namespace MyRE.SmartApp.Api.Client
{
    public interface IMyreSmartAppApiClient: IDisposable
    {
        Task<ApiResponse<InstanceStatus>> GetInstanceStatusAsync();
    }
}
