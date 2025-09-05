// Services/ExcelService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using AutoMapper;
using Microsoft.Extensions.Logging;
using OGCBackend.DTOs;
using OGCBackend.Models;
using OGCBackend.Repositories;
using OGCBackend.Helpers;

namespace OGCBackend.Services
{
    public class ExcelService : IExcelService
    {
        private readonly IPartidaRepository _partidaRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ExcelService> _logger;

        public ExcelService(
            IPartidaRepository partidaRepository,
            IMapper mapper,
            ILogger<ExcelService> logger)
        {
            _partidaRepository = partidaRepository;
            _mapper = mapper;
            _logger = logger;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<ExcelUploadDto> ProcessExcelFileAsync(IFormFile file)
        {
            var result = new ExcelUploadDto
            {
                FileName = file.FileName,
                ProcessedAt = DateTime.UtcNow,
                Errors = new List<string>()
            };

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                
                if (worksheet == null)
                {
                    result.Errors.Add("No se encontró ninguna hoja de cálculo en el archivo.");
                    return result;
                }

                var rowCount = worksheet.Dimension?.Rows ?? 0;
                var colCount = worksheet.Dimension?.Columns ?? 0;
                
                if (rowCount < 2)
                {
                    result.Errors.Add("El archivo no contiene datos para procesar.");
                    return result;
                }

                var partidas = new List<Partida>();
                
                // Start from row 3 (assuming row 1 is headers, row 2 might be sub-headers)
                for (int row = 3; row <= rowCount; row++)
                {
                    try
                    {
                        // Skip empty rows or header rows
                        var partidaValue = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                        if (string.IsNullOrWhiteSpace(partidaValue) || 
                            partidaValue.Equals("PARTIDA", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }                

                        var excelRow = new ExcelRowDto
                        {
                            RowNumber = row,
                            Partida = partidaValue,
                            Familia = worksheet.Cells[row, 2].Value?.ToString()?.Trim(),
                            SubPartida = worksheet.Cells[row, 3].Value?.ToString()?.Trim(),
                            Cantidad = ParseDecimal(worksheet.Cells[row, 4].Value),
                            PrecioUnitario = ParseDecimal(worksheet.Cells[row, 5].Value),
                            Subtotal = ParseDecimal(worksheet.Cells[row, 6].Value),
                            Iva = ParseDecimal(worksheet.Cells[row, 7].Value),
                            Total = ParseDecimal(worksheet.Cells[row, 8].Value),
                            Aprobado = ParseDecimal(worksheet.Cells[row, 9].Value),
                            Pagado = ParseDecimal(worksheet.Cells[row, 10].Value),
                            PorLiquidar = ParseDecimal(worksheet.Cells[row, 11].Value),
                            Actual = ParseDecimal(worksheet.Cells[row, 12].Value)
                        };

                        // Validate required fields
                        if (!string.IsNullOrWhiteSpace(excelRow.Partida) && 
                            !string.IsNullOrWhiteSpace(excelRow.Familia))
                        {
                            var partida = _mapper.Map<Partida>(excelRow);
                            partida.ArchivoOrigen = file.FileName;
                            
                            // Check if already exists
                            var exists = await _partidaRepository.ExistsAsync(
                                partida.Nombre, 
                                partida.Familia, 
                                partida.SubPartida);
                            
                            if (!exists)
                            {
                                partidas.Add(partida);
                                result.SuccessfulRecords++;
                            }
                            else
                            {
                                _logger.LogWarning($"Registro duplicado en fila {row}: {partida.Nombre} - {partida.Familia}");
                            }
                        }
                        else
                        {
                            result.FailedRecords++;
                            if (!string.IsNullOrWhiteSpace(excelRow.Partida) || 
                                !string.IsNullOrWhiteSpace(excelRow.Familia))
                            {
                                result.Errors.Add($"Fila {row}: Datos incompletos - Partida o Familia faltante");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.FailedRecords++;
                        result.Errors.Add($"Error en fila {row}: {ex.Message}");
                        _logger.LogError(ex, $"Error procesando fila {row}");
                    }
                }

                result.TotalRecords = result.SuccessfulRecords + result.FailedRecords;

                if (partidas.Any())
                {
                    await _partidaRepository.AddRangeAsync(partidas);
                    await _partidaRepository.SaveChangesAsync();
                    _logger.LogInformation($"Se guardaron {partidas.Count} registros exitosamente.");
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error general: {ex.Message}");
                _logger.LogError(ex, "Error procesando archivo Excel");
            }

            return result;
        }

        public async Task<byte[]> ExportToExcelAsync()
        {
            var partidas = await _partidaRepository.GetAllAsync();
            
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Partidas");
            
            // Headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "PARTIDA";
            worksheet.Cells[1, 3].Value = "FAMILIA";
            worksheet.Cells[1, 4].Value = "SUBPARTIDA";
            worksheet.Cells[1, 5].Value = "CANTIDAD";
            worksheet.Cells[1, 6].Value = "P.U";
            worksheet.Cells[1, 7].Value = "SUBTOTAL";
            worksheet.Cells[1, 8].Value = "IVA";
            worksheet.Cells[1, 9].Value = "TOTAL";
            worksheet.Cells[1, 10].Value = "APROBADO";
            worksheet.Cells[1, 11].Value = "PAGADO";
            worksheet.Cells[1, 12].Value = "POR LIQUIDAR";
            worksheet.Cells[1, 13].Value = "ACTUAL";
            worksheet.Cells[1, 14].Value = "FECHA CARGA";
            
            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 10])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
            }
            
            // Data
            int row = 2;
            foreach (var partida in partidas)
            {
                worksheet.Cells[row, 1].Value = partida.Id;
                worksheet.Cells[row, 2].Value = partida.Nombre;
                worksheet.Cells[row, 3].Value = partida.Familia;
                worksheet.Cells[row, 4].Value = partida.SubPartida;
                worksheet.Cells[row, 5].Value = partida.Total;
                worksheet.Cells[row, 6].Value = partida.Aprobado;
                worksheet.Cells[row, 7].Value = partida.Pagado;
                worksheet.Cells[row, 8].Value = partida.PorLiquidar;
                worksheet.Cells[row, 9].Value = partida.Actual;
                worksheet.Cells[row, 10].Value = partida.FechaCarga.ToString("yyyy-MM-dd HH:mm:ss");
                
                // Format currency columns
                worksheet.Cells[row, 5, row, 9].Style.Numberformat.Format = "$#,##0.00";
                
                row++;
            }
            
            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            
            return package.GetAsByteArray();
        }

        private decimal? ParseDecimal(object value)
        {
            if (value == null)
                return null;

            var stringValue = value.ToString().Trim();
            
            // Remove currency symbols and formatting
            stringValue = stringValue.Replace("$", "")
                                   .Replace(",", "")
                                   .Replace(" ", "")
                                   .Trim();
            
            // Handle negative numbers in parentheses
            bool isNegative = false;
            if (stringValue.StartsWith("(") && stringValue.EndsWith(")"))
            {
                isNegative = true;
                stringValue = stringValue.Trim('(', ')');
            }
            
            // Handle dash or empty as zero
            if (stringValue == "-" || string.IsNullOrWhiteSpace(stringValue))
                return 0;
            
            if (decimal.TryParse(stringValue, out decimal result))
            {
                return isNegative ? -result : result;
            }
            
            return null;
        }
    }
}