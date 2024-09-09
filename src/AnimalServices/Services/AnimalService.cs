using AnimalServices.DataContext;
using AnimalServices.Dtos;
using AnimalServices.Entities;
using AnimalServices.Helpers;
using AnimalServices.Services.Interfaces;
using AutoMapper;
using Event;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AnimalServices.Services;

public class AnimalService : IAnimalService
{
    private readonly IMapper _mapper;
    private readonly RabbitDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public AnimalService(IMapper mapper,RabbitDbContext dbContext ,IPublishEndpoint publishEndpoint)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<List<AnimalDto>> GetAllAnimals()
    {
        var result = await this._dbContext.Animals
            .Include(x => x.Address)
            .OrderBy(x => x.UpdatedAt)
            .ToListAsync();
        var animal = _mapper.Map<List<AnimalDto>>(result);
        return animal;
    }

    public async Task<AnimalDto> GetAnimalById(Guid id)
    {
        var animal = await this._dbContext.Animals
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Id == id);
        var result = _mapper.Map<AnimalDto>(animal);
        if (animal is null)
        {
            throw new Exception("Animal  not found");
        }
        return result;
    }

    public async Task<AnimalDto> CreateAnimal(CreateAnimalDto createAnimalDto)
    {
        try
        {
            var animal = _mapper.Map<Animal>(createAnimalDto);
            await _dbContext.Animals.AddAsync(animal);
            var newAnimal = _mapper.Map<AnimalDto>(animal);
            var createAnimal = _mapper.Map<CreateAnimalDto>(animal);
            await _publishEndpoint.Publish(createAnimal);
            await _dbContext.SaveChangesAsync();
            return newAnimal;
        }
        catch (Exception e)
        {
            throw new Exception($"{e.InnerException} {e.StackTrace} {e.Data}");
        }
        return null;
    }   

    public async Task UpdateAnimal(Guid id, UpdateAnimalDto updateAnimalDto)
    {
        try
        {
            var animal = await _dbContext.Animals.Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (animal == null)
            {
                throw new Exception("Animmal not found");
            };

            animal.Description = updateAnimalDto.Description ?? animal.Description;
            animal.Name = updateAnimalDto.Name ?? animal.Name;
            animal.EnumStatus = updateAnimalDto.EnumStatus != null ? EnumHelper.EnumParse(updateAnimalDto.EnumStatus, animal.EnumStatus) : animal.EnumStatus;
            animal.Breed = updateAnimalDto.Breed ?? animal.Breed;
            animal.CoverImageUrl = updateAnimalDto.CoverImageUrl ?? animal.CoverImageUrl;
            animal.Color = updateAnimalDto.Color ?? animal.Color;
            animal.Type = updateAnimalDto.Type ?? animal.Type;
            animal.CoverImageUrl = updateAnimalDto.CoverImageUrl ?? animal.CoverImageUrl;
            animal.Weight = updateAnimalDto.Weight == 0 ? animal.Weight : updateAnimalDto.Weight;
            animal.Age = updateAnimalDto.Age == 0 ? animal.Age : updateAnimalDto.Age;
            animal.Address.City = updateAnimalDto.City ?? animal.Address.City;
            animal.Address.Address1 = updateAnimalDto.AddressOne ?? animal.Address.Address1;
            animal.Address.Address2 = updateAnimalDto.AddressTwo ?? animal.Address.Address2;
            animal.Address.Country = updateAnimalDto.Country ?? animal.Address.Country;
            animal.Address.PostalCode = updateAnimalDto.PostalCode ?? animal.Address.PostalCode;
            animal.Address.State = updateAnimalDto.State ?? animal.Address.State;
            animal.UpdatedAt = DateTime.UtcNow;
            var updateAnimal = _mapper.Map<AnimalUpdated>(animal);
            await _publishEndpoint.Publish(updateAnimal);
            await this._dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Update qilishda muamo yuzaga keldi");
        }
    }

    public async Task DeleteAnimal(Guid id)
    {
        var animal = await this._dbContext.Animals
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (animal is null)
            throw new Exception("Animal not found");
        this._dbContext.Animals.Remove(animal);
        await this._publishEndpoint.Publish<AnimalDeleted>(new { Id = animal.Id.ToString() });
        await this._dbContext.SaveChangesAsync();
    }
}