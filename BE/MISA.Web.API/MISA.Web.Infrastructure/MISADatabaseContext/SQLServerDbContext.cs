using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Web.Core.Atribute;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Infrastructure.MISADatabaseContext
{
    public class SQLServerDbContext
    {
        #region Property
        public IDbConnection Connection { get; }
        #endregion
        #region Constructor
        public SQLServerDbContext(IConfiguration configuration)
        {
            Connection = new MySqlConnection(configuration.GetConnectionString("DefaultConection"));
        }
        #endregion

        #region Method

        public IEnumerable<MISAEntity> GetAll<MISAEntity>()
        {
            var entityName = typeof(MISAEntity).Name;
            var entites = Connection.Query<MISAEntity>($"SELECT * FROM {entityName} ");
            return entites;
        }

        public MISAEntity GetByid<MISAEntity>(Guid entityId)
        {
            var entityName = typeof(MISAEntity).Name;
            string Command = $"SELECT * FROM {entityName} WHERE {entityName}Id = @{entityName}Id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{entityName}Id", entityId);
            var entity = Connection.QueryFirstOrDefault<MISAEntity>(sql: Command, param: parameters);
            return entity;
        }

        public int InsertData<MISAEntity>(MISAEntity entity)
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
                //Kiểm tra property là khóa chính
                var primaryKey = Attribute.IsDefined(prop, typeof(PrimaryKey));
                var notmap = Attribute.IsDefined(prop, typeof(NotMap));
                var date = Attribute.IsDefined(prop, typeof(DateValue));
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
                if (date == true)
                {
                    propValue = DateTime.ParseExact(propValue.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
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
            var res = Connection.Execute(sql: sqlCommand, param: parameters);
            return res;
        }

        public int Update<MISAEntity>(MISAEntity entity, Guid entityId)
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
                var date = Attribute.IsDefined(prop, typeof(DateValue));
                if (primarykey == true)
                {
                    continue;
                }
                if (date == true)
                {
                    propValue = DateTime.ParseExact(propValue.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                }
                //Tạo cấu trúc update 
                var paramName = $"@{propName}";
                sqlColumnName.Append($"{delimiter} {propName} =@{propName}");
                delimiter = ",";
                parameters.Add(paramName, propValue);
            }
            var sqlCommand = $"UPDATE {className} SET {sqlColumnName} WHERE {className}Id = '{entityId}'";
            var res = Connection.Execute(sql: sqlCommand, param: parameters);
            return res;

        }

        public int Delete<MISAEntity>(Guid entityId)
        {
            var query = "";
            var className = typeof(MISAEntity).Name;
            var parameter = new DynamicParameters();

            query += $"DELETE FROM {className} WHERE {className}Id = @id";
            parameter.Add($"@id", entityId);
            var res = Connection.Execute(sql: query, param: parameter);
            return res;
        }

        public IEnumerable<MISAEntity> Paginate<MISAEntity>(int? pageSize, int? numberPage)
        {
            var className = typeof(MISAEntity).Name;

            if (pageSize == 0 || numberPage == 0)
            {
                var sqlCommand = $"SELECT * FROM {className}  ";
                var res = Connection.Query<MISAEntity>(sql: sqlCommand);
                return res;
            }
            else
            {
                var start_index = (numberPage - 1) * pageSize;
                var sqlCommand = $"SELECT * FROM {className}  LIMIT {pageSize} OFFSET {start_index} ";
                {
                    var res = Connection.Query<MISAEntity>(sql: sqlCommand);
                    return res;
                }
            }
        }

        public IEnumerable<MISAEntity> GetDataBy<MISAEntity>(string? data, string? column)
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
                    var res = Connection.Query<MISAEntity>(sql: sqlCommand, param: parameters);
                    return res;
                }
            }
        }
        #endregion
    }
}
