// Helpers/ExcelMapper.cs
using System;
using System.Globalization;
using OGCBackend.DTOs;

namespace OGCBackend.Helpers
{
    public static class ExcelMapper
    {
        public static ExcelRowDto MapExcelRow(dynamic row, int rowNumber)
        {
            var dto = new ExcelRowDto
            {
                RowNumber = rowNumber,
                Partida = GetStringValue(row, "A"),
                Familia = GetStringValue(row, "B"),
                SubPartida = GetStringValue(row, "C"),
                Cantidad = GetDecimalValue(row, "D"),
                PrecioUnitario = GetDecimalValue(row, "E"),
                Subtotal = GetDecimalValue(row, "F"),
                Iva = GetDecimalValue(row, "G"),
                Total = GetDecimalValue(row, "H"),
                Aprobado = GetDecimalValue(row, "I"),
                Pagado = GetDecimalValue(row, "J"),
                PorLiquidar = GetDecimalValue(row, "K"),
                Actual = GetDecimalValue(row, "L")
            };

            return dto;
        }

        private static string GetStringValue(dynamic row, string column)
        {
            try
            {
                var value = row[column]?.ToString()?.Trim();
                return string.IsNullOrWhiteSpace(value) ? null : value;
            }
            catch
            {
                return null;
            }
        }

        private static decimal? GetDecimalValue(dynamic row, string column)
        {
            try
            {
                var value = row[column]?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(value))
                    return null;

                // Remove currency symbols and spaces
                value = value.Replace("$", "").Replace(",", "").Replace(" ", "").Trim();
                
                // Handle parentheses for negative numbers
                bool isNegative = false;
                if (value.StartsWith("(") && value.EndsWith(")"))
                {
                    isNegative = true;
                    value = value.Trim('(', ')');
                }

                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                {
                    return isNegative ? -result : result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}