using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Web.Core.Atribute;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Interfaces.UnitOfWork;
using MISA.Web.Infrastructure.Interface;
using MySqlConnector;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace MISA.Web.Infrastructure.MISADatabaseContext
{
    public class MySqlDbcontext : IMISADbContext
    {
        #region Property
        public IDbConnection Connection { get; }
        private IUnitOfWork _unitOfWork { get; }
        #endregion
        #region Constructor
        public MySqlDbcontext(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Method
        public  async Task<IEnumerable<MISAEntity>> GetAll<MISAEntity>()
        {
            var entityName = typeof(MISAEntity).Name;
            string Command = $"SELECT * FROM {entityName}";
            var entity = await _unitOfWork.Connection.QueryAsync<MISAEntity>(sql: Command, transaction: _unitOfWork.Transaction);
            return entity;
        }

        public async Task<MISAEntity> GetByid<MISAEntity>(Guid entityId)
        {
            var entityName = typeof(MISAEntity).Name;
            string Command = $"SELECT * FROM {entityName} WHERE {entityName}Id = @{entityName}Id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{entityName}Id", entityId);
            var entity = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<MISAEntity>(sql: Command, param: parameters, transaction: _unitOfWork.Transaction); ;
            return entity;
        }
      

        public async Task<int> InsertData<MISAEntity>(MISAEntity entity)
        {
            var className = typeof(MISAEntity).Name;
            var sqlColumnsNames = new StringBuilder();
            var sqlColumnsValue = new StringBuilder();
            DynamicParameters parameters = new DynamicParameters();
            string delimiter = "";
            //1.Duyệt tất cả các property của đối tượng:%
            var props = typeof(MISAEntity).GetProperties();
            foreach (var prop in props)
            {
                // 2.lẤY tên property
                var propName = prop.Name;
                var propValue = prop.GetValue(entity);
                var primaryKey = Attribute.IsDefined(prop, typeof(PrimaryKey));
                var notmap = Attribute.IsDefined(prop, typeof(NotMap));
                if (primaryKey == true || propName == $"{className}Id")
                {
                    if (prop.PropertyType == typeof(Guid))
                    {
                        propValue = Guid.NewGuid();
                    }
                }
                if (notmap == true)
                {
                    continue;
                }
                //sqlColumnsName.append(propname);
                var paramName = $"@{propName}";
                sqlColumnsNames.Append($"{delimiter}{propName}");
                sqlColumnsValue.Append($"{delimiter}@{propName}");
                delimiter = ",";
                parameters.Add(paramName, propValue);
            }
            // 3 .Lấy ra giá trị của property:

            //4. Thực hiện build sql
            var sqlCommand = $"INSERT INTO {className} ({sqlColumnsNames}) VALUES ({sqlColumnsValue})";
            var res = await _unitOfWork.Connection.ExecuteAsync(sql: sqlCommand, param: parameters, transaction: _unitOfWork.Transaction);
            return res;
        }
        public async Task<int> InsertMultiData<MISAEntity>(List<MISAEntity> entities)
        {
            var className = typeof(MISAEntity).Name;
            var sqlColumnsNames = new StringBuilder();
            var sqlColumnsValues = new StringBuilder();
            DynamicParameters parameters = new DynamicParameters();

            var props = typeof(MISAEntity).GetProperties();
            var sql = "";
            var index = 1;
            foreach (var entity in entities)
            {
                sqlColumnsValues.Clear(); // Xóa các giá trị của StringBuilder
                string delimiter = "";
                foreach (var prop in props)
                {
                    var propName = prop.Name;
                    var propValue = prop.GetValue(entity);
                    var primaryKey = Attribute.IsDefined(prop, typeof(PrimaryKey));
                    var notmap = Attribute.IsDefined(prop, typeof(NotMap));

                    if (primaryKey || propName == $"{className}Id")
                    {
                        if (prop.PropertyType == typeof(Guid))
                        {
                            propValue = Guid.NewGuid();
                        }
                    }

                    if (notmap)
                    {
                        continue;
                    }

                    var paramName = $"@{propName}{index}";
                    if (index == 1)
                    {
                        sqlColumnsNames.Append($"{propName},");
                    }

                    sqlColumnsValues.Append($"{delimiter}{paramName}");
                    parameters.Add(paramName, propValue);
                    delimiter = ",";
                }

                sql += $"({sqlColumnsValues}),";
                index++;
            }

            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Substring(0, sql.Length - 1); // Xóa dấu phẩy cuối cùng
            }

            var sqlCommand = $"INSERT INTO {className} ({sqlColumnsNames.Remove(sqlColumnsNames.Length-1,1)}) VALUES {sql}";

            var res = await _unitOfWork.Connection.ExecuteAsync(sql: sqlCommand, param: parameters, transaction: _unitOfWork.Transaction);
            return res;
        }

        public async Task<int> Update<MISAEntity>(MISAEntity entity, Guid entityId)
        {
            var className = typeof(MISAEntity).Name;
            var sqlColumnName = new StringBuilder();
            var sqlColumnValue = new StringBuilder();

            DynamicParameters parameters = new DynamicParameters();
            string delimiter = "";
            //duyệt qua các properties của table
            var props = typeof(MISAEntity).GetProperties();
            foreach (var prop in props)
            {
                //lấy tên property
                var propName = prop.Name;
                var propValue = prop.GetValue(entity);
                //check primary key
                var primarykey = Attribute.IsDefined(prop, typeof(PrimaryKey));
                var notmap = Attribute.IsDefined(prop, typeof(NotMap));
                var code = Attribute.IsDefined(prop, typeof(NotEmpty));
                if (primarykey == true)
                {
                    continue;
                }
                //Tạo cấu trúc update 
                var paramName = $"@{propName}";
                sqlColumnName.Append($"{delimiter} {propName} =@{propName}");
                delimiter = ",";
                parameters.Add(paramName, propValue);
            }
            var sqlCommand = $"UPDATE {className} SET {sqlColumnName} WHERE {className}Id = @id";
            parameters.Add("@id", entityId);
            var res = await _unitOfWork.Connection.ExecuteAsync(sql: sqlCommand, param: parameters, transaction: _unitOfWork.Transaction); ;
            return res;

        }

        public async Task<int> Delete<MISAEntity>(Guid entityId)
        {
            var query = "";
            var className = typeof(MISAEntity).Name;
            var parameter = new DynamicParameters();

            query += $"DELETE FROM {className} WHERE {className}Id = @id";
            parameter.Add($"@id", entityId);
            var res = await _unitOfWork.Connection.ExecuteAsync(sql: query, param: parameter, transaction: _unitOfWork.Transaction); ;
            return res;
        }
        public async Task<IEnumerable<MISAEntity>> GetDataBy<MISAEntity>(string? data, string? column)
        {
            var className = typeof(MISAEntity).Name;
            var parameters = new DynamicParameters();
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(column))
            {
                var sqlCommand = $"SELECT * FROM {className} ";
                var res = Connection.Query<MISAEntity>(sql: sqlCommand);
                return res;
            }
            else
            {
                var sqlCommand = $"SELECT * FROM {className} WHERE  {column} LIKE '%@data%'";
                {
                    parameters.Add("@data", data);
                    var res = await _unitOfWork.Connection.QueryAsync<MISAEntity>(sql: sqlCommand, param: parameters, transaction: _unitOfWork.Transaction); ;
                    return res;
                }
            }
        }
        public async Task<int> DeleteMultiple<MISAEntity>(List<Guid> entityId)
        {
            var query = "";
            var arrayId = "";
            var className = typeof(MISAEntity).Name;
            var parameter = new DynamicParameters();

            foreach (var entity in entityId)
            {
                parameter.Add($"@id", entity);
                arrayId += $"'{entity}',";
            }
            var listId = arrayId.Remove(arrayId.Length - 1);
            query += $"DELETE FROM {className} WHERE {className}Id IN ({listId})";
            var res = await _unitOfWork.Connection.ExecuteAsync(sql: query, param: parameter, transaction: _unitOfWork.Transaction); ;

            return res;
        }


        public async Task<Guid> GetDataByName<MISAEntity>(string? name)
        {
            var parameter = new DynamicParameters();
            var entityName = typeof(MISAEntity).Name;
            string Command = $"SELECT {entityName}Id  FROM {entityName} WHERE TRIM({entityName}Name) = '{name}' ";
            var res = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Guid>(sql: Command, param: parameter, transaction: _unitOfWork.Transaction); ;
            return res;
        }


        #endregion

    }
}
