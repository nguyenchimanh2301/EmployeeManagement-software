
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using System.Security.Claims;
using System.Text;

namespace MISA.Web.API.Controllers
{/// <summary>
 /// / Các API thực hiện công việc thao tác với Database sử dụng ở tầng Data Access gồm:
 /// <item>Các hàm thực hiện lấy bản ghi,lọc bản ghi</item>
 /// <item>Các hàm thực hiện thêm/sửa/xóa bản ghi,lọc bản ghi</item>
 /// </summary>
 /// <typeparam name="MISAEntity">Đối tượng lớp API</typeparam>
 /// CreatedBy NC Mạnh(25/12/2023)
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MISABaseController<MISAEntity> : Controller
    {
        #region Fields
        IBaseService<MISAEntity> _baseService;
        IBaseRepository<MISAEntity> _baseRepository;
        private readonly IMapper mapper;
        #endregion
        #region Constructer
        public MISABaseController(IBaseService<MISAEntity> baseService, IBaseRepository<MISAEntity> baseRepository)
        {
            _baseService = baseService;
            _baseRepository = baseRepository;
        }

        public MISABaseController(IMapper mapper)
        {
            this.mapper = mapper;
        }
        #endregion

        #region Method
        /// <summary>
        /// API lấy tất cả bản ghi
        /// <returns>
        /// Danh sách bản ghi
        /// 200 - Lấy về thành công
        /// 400 - Lỗi nghiệp vụ
        /// 500 - Có Exception
        /// </returns>
        /// </summary>
        /// CreatedBy NC Mạnh(25/12/2023)
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {

            var data = await _baseRepository.GetAllAsync();
            return Ok(data);

        }
        /// API lấy bản ghi phân trang
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="numberSize">Số trang</param>
        /// <returns>
        /// Danh sách bản ghi phân trang
        /// 200 - Lấy về thành công
        /// 400 - Lỗi nghiệp vụ
        /// 500 - Có Exception
        /// </returns>
        /// </summary>
        /// CreatedBy NC Mạnh(25/12/2023)


        /// <summary>
        /// API về thông tin theo ID
        /// <param name="entityId">Id đối tượng</param>
        /// <returns>
        /// Thông tin đối tượng
        /// 200 - Lấy về thành công
        /// 400 - Lỗi nghiệp vụ
        /// 500 - Có Exception
        /// </returns>
        /// </summary>
        /// CreatedBy NC Mạnh(25/12/2023)
        [HttpGet("{entityId}")]
        public async Task<IActionResult> GetById(Guid entityId)
        {

            var data = await _baseRepository.GetByid(entityId);
            return Ok(data);

        }
        /// <summary>
        /// API thêm mới
        /// <param name="entity">
        /// Đối tượng thêm mới
        /// </param>
        /// <returns>
        /// Số bản ghi
        /// 201 - Thêm mới thành công
        /// 400 - Lỗi nghiệp vụ
        /// 500 - Có Exception
        /// </returns>
        /// </summary>
        /// CreatedBy NC Mạnh(25/12/2023)
        [HttpPost]
        public async Task<IActionResult> Insert(MISAEntity entity)
        {
            var data = await _baseService.Insert(entity);
            return StatusCode(201, data);

        }
        /// <summary>
        /// API thêm mới
        /// <param name="entity">
        /// Đối tượng thêm mới
        /// </param>
        /// <returns>
        /// Số bản ghi
        /// 201 - Thêm mới thành công
        /// 400 - Lỗi nghiệp vụ
        /// 500 - Có Exception
        /// </returns>
        /// </summary>
        /// CreatedBy NC Mạnh(25/12/2023)
        [HttpPost("Multiple")]
        public async Task<IActionResult> InsertMulti(List<MISAEntity> entity)
        {
            
            var data = await _baseRepository.InsertMultiData(entity);
            return StatusCode(201, data);
        }

        /// <summary>
        /// API cập nhật bản ghi
        /// <param name="entity">Đối tượng cập nhật</param>
        /// <param name="Id">Id đối tượng cần cập nhật</param>
        /// <returns>
        /// Số bản ghi
        /// 200 - Cập nhật thành công
        /// 400 - Lỗi nghiệp vụ
        /// 500 - Có Exception
        /// </returns>
        /// </summary>
        /// CreatedBy NC Mạnh(25/12/2023)
        [HttpPut("{entityId}")]
        public async Task<IActionResult> Update(MISAEntity entity, Guid entityId)
        {
            var data = await _baseService.UpdateService(entity, entityId);
            return StatusCode(200, data);
        }
        /// <summary>
        /// API xóa bản ghi
        /// <param name="Id">Id đối tượng cần xóa</param>
        /// <returns>
        /// Số bản ghi
        /// 200 - Xóa thành công
        /// 400 - Lỗi nghiệp vụ
        /// 500 - Có Exception
        /// </returns>
        /// </summary>
        /// CreatedBy NC Mạnh(25/12/2023)
        [HttpDelete("{entityId}")]
        public async Task<IActionResult> Delete(Guid entityId)
        {

            var data = await _baseRepository.Delete(entityId);
            return StatusCode(201, data);

        }
        /// <summary>
        /// API xóa nhiều bản ghi
        /// </summary>
        /// <typeparam name="MISAEntity">Lớp dối tượng muốn xóa </typeparam>
        /// <param name="entityId"> Danh sách các id cần xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy NC Mạnh (04/01/2024)
        [HttpDelete]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<Guid> arrayEntityId)
        {
            var data = await _baseService.DeleteMultiple(arrayEntityId);
            return StatusCode(201, data);
        }
        /// <summary>
        /// API lọc bản ghi
        /// </summary>
        /// <param name="data">Dữ liệu cần lọc</param>
        /// <param name="column">Trường muốn lọc</param>
        /// <returns> Trả về bản ghi đã lọc</returns>
       /* [HttpGet("{data},{column}")]
        public IActionResult GetDataBy(string? data, string? column)
        {
            var res = _baseRepository.GetDataBy(data, column);
            return StatusCode(200, res);
        }*/
        #endregion

    }
}
