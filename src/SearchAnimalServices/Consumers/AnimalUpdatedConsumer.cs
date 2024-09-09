using AutoMapper;
using Event;
using MassTransit;
using MongoDB.Entities;
using SearchAnimalServices.Models;

namespace SearchAnimalServices.Consumers;

public class AnimalUpdatedConsumer : IConsumer<AnimalUpdated>
{
    private readonly IMapper _mapper;

    public AnimalUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AnimalUpdated> context)
    {
        Console.WriteLine("Consuming animal update " + context.Message.Id);

        var animal = _mapper.Map<Animal>(context.Message);

        var result = await DB.Update<Animal>().Match(animal => animal.ID == context.Message.Id).ModifyOnly(
            animal => new
            {
                animal.Name,
                animal.Age,
                animal.Description,
                animal.Breed,
                animal.Sex,
                animal.Weight,
                animal.Color,
                animal.Type,
                animal.CoverImageUrl,
                animal.UpdatedAt,

            }, animal).ExecuteAsync();
        if (!result.IsAcknowledged)
            throw new MessageException(typeof(AnimalUpdated), "Problem updating mongodb");
    }
}