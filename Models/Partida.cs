using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OGCBackend.Models
{
    public class Partida
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Familia { get; set; }
        
        [MaxLength(200)]
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
        
        public string ArchivoOrigen { get; set; }    
    }
}