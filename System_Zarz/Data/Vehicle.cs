using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System_Zarz.Data;
using System.ComponentModel.DataAnnotations;

public class Vehicle
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    
    [ValidateNever]
    public Customer Customer { get; set; }

    [ValidateNever] 
    public string Brand { get; set; } = null!;

    [ValidateNever] 
    public string Model { get; set; } = null!;

    [Required(ErrorMessage = "VIN jest wymagany.")]
    public string VIN { get; set; } = null!;

    [Required(ErrorMessage = "Numer rejestracyjny jest wymagany.")]
    public string RegistrationNumber { get; set; } = null!;

    [Required(ErrorMessage = "Rok produkcji jest wymagany.")]
    [Range(1900, 2100, ErrorMessage = "Podaj poprawny rok.")]
    public int Year { get; set; }

    public string? ImagePath { get; set; }

    public List<Order> Orders { get; set; } = new();
}