using AnimalServices.Dtos;

namespace AnimalServices.Services.Interfaces;

public interface IAnimalService
{
    Task<List<AnimalDto>> GetAllAnimals();
    Task<AnimalDto> GetAnimalById(Guid id);
    Task<AnimalDto> CreateAnimal(CreateAnimalDto createAnimalDto);
    Task UpdateAnimal(Guid id, UpdateAnimalDto updateAnimalDto);
    Task DeleteAnimal(Guid id);
}