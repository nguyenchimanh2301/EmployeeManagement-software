using MISA.Web.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Interfaces.UnitOfWork
{
    /// <summary>
    /// Interface Unit of work
    /// CreatedBy NCManh(15/1/2024)
    /// </summary>
    public interface IUnitOfWork:IDisposable ,IAsyncDisposable
    {
        #region Property

        /// <summary>
        /// Mở chuỗi kết nối
        /// </summary>
        DbConnection Connection { get; }
        /// <summary>
        /// Mở giao dịch
        /// </summary>
        DbTransaction? Transaction { get; }
        #endregion
        #region Constructor
        /// <summary>
        /// Hàm Bắt đầu giao dịch
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Hàm Bắt đầu giao dịch bất đồng bộ
        /// </summary>
        Task BeginTransactionAsync();
        /// <summary>
        /// Hàm Commit dữ liệu
        /// </summary>
        void Commit();
        /// <summary>
        /// Hàm Commit bất đồng bộ
        /// </summary>
        Task CommitAsync();
        /// <summary>
        /// Hàm rollback
        /// </summary>
        void Rollback();
        /// <summary>
        ///  Hàm rollback bất đồng bộ
        /// </summary>
        /// <returns></returns>
        Task RollbackAsync();
        #endregion

    }
}
