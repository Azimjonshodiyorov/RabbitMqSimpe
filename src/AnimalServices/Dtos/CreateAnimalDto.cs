using System.ComponentModel.DataAnnotations;
using AnimalServices.Entities;

namespace AnimalServices.Dtos;

public class CreateAnimalDto
{
    [Required]
    public int Age { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    public string Breed { get; set; }
    [Required]
    public string Sex { get; set; }
    [Required]
    public int Weight { get; set; }
    [Required]
    public string Color { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string CoverImageUrl { get; set; }
    [Required]
    public EnumStatus EnumStatus { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    [Required]
    public string State { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string PostalCode { get; set; }
}