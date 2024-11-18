
using Rehm.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rehm2024.Service
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<ApiResponse<TEntity>> AddAsync(TEntity entity);

        Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);

        Task<ApiResponse> DeleteAsync(int id);

        Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(int id);

    }
}
