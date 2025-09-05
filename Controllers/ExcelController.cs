// Controllers/ExcelController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OGCBackend.Services;
using OGCBackend.DTOs;

namespace OGCBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;
        private readonly ILogger<ExcelController> _logger;

        public ExcelController(IExcelService excelService, ILogger<ExcelController> logger)
        {
            _excelService = excelService;
            _logger = logger;
        }

        /// <summary>
        /// Upload and process an Excel file
        /// </summary>
        /// <param name="file">Excel file to process</param>
        /// <returns>Processing result</returns>
        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExcelUploadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "Por favor, proporcione un archivo válido." });
                }

                var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
                if (fileExtension != ".xlsx" && fileExtension != ".xls")
                {
                    return BadRequest(new { message = "Solo se permiten archivos Excel (.xlsx, .xls)." });
                }

                if (file.Length > 10 * 1024 * 1024) // 10MB limit
                {
                    return BadRequest(new { message = "El archivo excede el tamaño máximo permitido (10MB)." });
                }

                var result = await _excelService.ProcessExcelFileAsync(file);

                if (result.Errors.Count > 0 && result.SuccessfulRecords == 0)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el archivo Excel");
                return StatusCode(500, new { message = "Error al procesar el archivo Excel." });
            }
        }
    }
}
