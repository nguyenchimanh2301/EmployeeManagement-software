using Dapper;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.UnitOfWork;
using MISA.Web.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Infrastructure.Repositoty
{
    public class PositionRepsitory : BaseRepository<Positions>, IPositionRepsitory
    {
        #region Fields

        IUnitOfWork _unitOfWork;
        #endregion
        #region Constructor

        public PositionRepsitory(IMISADbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion
        #region Method

        public override async Task<object> GetDataByName(string? name)
        {
            
            var parameter = new DynamicParameters();
            string Command = $"SELECT PositionId  FROM Positions WHERE TRIM(PositionName) = '{name}' ";
            var res = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Guid>(sql: Command, param: parameter, transaction: _unitOfWork.Transaction);
            if (res != Guid.Empty)
            {
                // Trả về GUID chính xác
                return res;
            }
            else
            {
                // Trả về null
                return null;
            }

        }
        #endregion
    }


}
