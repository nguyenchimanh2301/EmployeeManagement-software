using AutoMapper;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DTOImport, Employee>();
            CreateMap<Employee, DTOImport>();
            CreateMap<Employee, DTOEmployee>();
            CreateMap<DTOImport, DTOEmployee>();
            CreateMap<DTOImport, object>();
            CreateMap<IEnumerable<DTOImport>, IEnumerable<object>>();
            CreateMap<object, DTOImport>();
            CreateMap<DTOEmployee, DTOImport>();
            CreateMap<DTOAccount, Account>();
            CreateMap<Account, DTOAccount>();
        }

    }
}
