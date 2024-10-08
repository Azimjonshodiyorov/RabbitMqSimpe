namespace AnimalServices.Dtos;

public class UpdateAnimalDto
{
    public int Age { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Breed { get; set; }
    public string Sex { get; set; }
    public int Weight { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
    public string CoverImageUrl { get; set; }
    public string EnumStatus { get; set; }

    public string AddressOne { get; set; }
    public string AddressTwo { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
}