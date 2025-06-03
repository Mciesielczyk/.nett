namespace System_Zarz.DTOs
{
    public class VehicleDto
    {
        public int CustomerId { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string VIN { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public int Year { get; set; }
        public IFormFile? UploadPhoto { get; set; }
    }
}
