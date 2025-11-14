using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Services
{
    public class AssetTypeService : BaseService<AssetType>, IAssetTypeService
    {
        public AssetTypeService(IBaseRepository<AssetType> baseRepository) : base(baseRepository)
        {
        }
    }
}
