// Services/IExcelService.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OGCBackend.DTOs;
using OGCBackend.Models;

namespace OGCBackend.Services
{
    public interface IExcelService
    {
        Task<ExcelUploadDto> ProcessExcelFileAsync(IFormFile file);
        Task<byte[]> ExportToExcelAsync();        
    }
}
