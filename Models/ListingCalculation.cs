using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceMarketplace.Models
{
    public class ListingCalculation
    {
        [Key]
        public int Id { get; set; }

        public int ListingId { get; set; }

        public double RequiredPaint { get; set; }
        public double RequiredPrimer { get; set; }
        public double RequiredCeramic { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedMaterialMin { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedMaterialMax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedLaborMin { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedLaborMax { get; set; }

        public int EstimatedDaysMin { get; set; }
        public int EstimatedDaysMax { get; set; }
    }
}
