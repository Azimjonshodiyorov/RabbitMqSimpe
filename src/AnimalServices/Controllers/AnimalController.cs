using AnimalServices.Dtos;
using AnimalServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnimalServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    [Route("getAll")]
    public async Task<List<AnimalDto>> GetAll() =>
        await this._animalService.GetAllAnimals();

    [HttpGet]
    [Route("getById")]
    public async Task<AnimalDto> GetById(Guid id) =>
        await this._animalService.GetAnimalById(id);


    [HttpPost]
    [Route("add")]
    public async Task<AnimalDto> AddAnimal(CreateAnimalDto animalDto) =>
        await this._animalService.CreateAnimal(animalDto);

    [HttpPut]
    [Route("update")]
    public async Task UpdateAnimal(Guid id, UpdateAnimalDto animalDto) =>
        await this._animalService.UpdateAnimal(id,animalDto);

    [HttpDelete]
    [Route("deleteById")]
    public async Task DeleteAnimal(Guid id) =>
        await this._animalService.DeleteAnimal(id);
}