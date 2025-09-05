using System;
using System.Collections.Generic;

namespace OGCBackend.DTOs
{
    public class PartidaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Familia { get; set; }
        public string SubPartida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public decimal Aprobado { get; set; }
        public decimal Pagado { get; set; }
        public decimal PorLiquidar { get; set; }
        public decimal Actual { get; set; }
        public DateTime FechaCarga { get; set; }                 
    }

    public class CreatePartidaDto
    {
        public string Nombre { get; set; }
        public string Familia { get; set; }
        public string SubPartida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public decimal Aprobado { get; set; }
        public decimal Pagado { get; set; }
        public decimal PorLiquidar { get; set; }
        public decimal Actual { get; set; }
    }

    public class ExcelUploadDto
    {
        public string FileName { get; set; }
        public int TotalRecords { get; set; }
        public int SuccessfulRecords { get; set; }
        public int FailedRecords { get; set; }
        public List<string> Errors { get; set; }
        public DateTime ProcessedAt { get; set; }
    }

    public class ExcelRowDto
    {
        public int RowNumber { get; set; }
        public string Partida { get; set; }
        public string Familia { get; set; }
        public string SubPartida { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Iva { get; set; }
        public decimal? Total { get; set; }
        public decimal? Aprobado { get; set; }
        public decimal? Pagado { get; set; }
        public decimal? PorLiquidar { get; set; }
        public decimal? Actual { get; set; }    
    }
}