using MISA.Web.Core.Interfaces.UnitOfWork;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Infrastructure.Interface;
namespace MISA.Web.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Lớp Unit of work
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly DbConnection _connection;
        IMISADbContext _context;
        private DbTransaction? _transaction = null;
        #endregion

        #region Constructor
        public UnitOfWork(string conection)
        {
            _connection = new MySqlConnection(conection);
        }
        #endregion
        #region Propertys
        public DbConnection Connection => _connection;
        public DbTransaction? Transaction => _transaction;
        public IEmployeeRepository EmployeeRepository { get; }

        #endregion
        #region Method

        public void BeginTransaction()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _transaction = _connection.BeginTransaction();


            }
            else
            {
                 Connection.Open();
                _transaction = _connection.BeginTransaction();

            }
        }

        public async Task BeginTransactionAsync()
        {
            if(_connection.State == ConnectionState.Open)
            {
                _transaction = await _connection.BeginTransactionAsync();

            }
            else
            {
                await Connection.OpenAsync();
                _transaction = await _connection.BeginTransactionAsync();
            }
        }

        public void Commit()
        {
            _transaction?.Commit();
            Dispose();
        }

        public async Task CommitAsync()
        {
            if( _transaction != null )
            {
            await _transaction.CommitAsync();
            }
            await DisposeAsync();
        }

        public void Dispose()
        {
           _transaction?.Dispose();
            _transaction = null;
            _connection.Close();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            _transaction = null;
            await _connection.CloseAsync();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
             Dispose();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
            await DisposeAsync();
        }
        #endregion

    }
}
