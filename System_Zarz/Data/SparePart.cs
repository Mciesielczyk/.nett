using System.ComponentModel.DataAnnotations;

namespace System_Zarz.Data
{
    public class SparePart
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000)]
        public decimal UnitPrice { get; set; }
    }
}