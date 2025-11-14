using Dapper;
using MISA.QLTS.Core.DTOs.Response;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Infrastructure.DapperContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Infrastructure.Repositories
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        private readonly DbContext _context;
        public AssetRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<AssetResponseDTO>> GetPagedAsync(int page, int pageSize, string? departmentID, string assetTypeID, string? search)
        {   
            using var connection =  _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("p_search_text", search);
            parameters.Add("p_department_id", departmentID);
            parameters.Add("p_asset_type_id", assetTypeID);
            parameters.Add("p_page_number", page);
            parameters.Add("p_page_size", pageSize);
            using (var multi = await connection.QueryMultipleAsync(
            "proc_get_all_asset_paging",
            parameters,
            commandType: CommandType.StoredProcedure))
            {
                var assets = (await multi.ReadAsync<AssetResponseDTO>()).ToList();

                var total = await multi.ReadFirstAsync<int>();

                return new PagedResult<AssetResponseDTO>
                {
                    Data = assets,
                    TotalCount = total,
                    Page = page,
                    PageSize = pageSize
                };
            }
        }
    }
}
