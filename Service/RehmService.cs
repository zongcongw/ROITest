using Rehm.Shared.Dtos;
using Rehm.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rehm2024.Service
{
    public class RehmService : IRehmService
    {

        private readonly HttpRestClient client;
        private readonly string serviceName = "rehmservice";

        public RehmService(HttpRestClient client)
        {
           this.client = client;
        }

        public Task<ApiResponse> AlarmReport(AlarmDto data)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> Heartbeat(HeartbeatDto data)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"m2m/cmpservice/services/api/v4/heartbeatRequest";
            request.Parameter = data;
            var res = await client.PostRequstAsync(request);
            return await client.PostRequstAsync(request);
        }

        public Task<ApiResponse> PartIn(PartInDto data)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> PartOut(PartOutDto user)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Status(StatusDto data)
        {
            throw new NotImplementedException();
        }
    }
}
