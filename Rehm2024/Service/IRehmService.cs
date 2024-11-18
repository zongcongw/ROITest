using Rehm.Shared;
using Rehm.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rehm2024.Service
{
    public interface IRehmService
    {
        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ApiResponse> Heartbeat(HeartbeatDto data);

        /// <summary>
        /// 进板
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ApiResponse> PartIn(PartInDto data);

        /// <summary>
        /// 出板
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ApiResponse> PartOut(PartOutDto user);

        /// <summary>
        /// 报警
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ApiResponse> AlarmReport(AlarmDto data);

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ApiResponse> Status(StatusDto data);

        

    }
}
