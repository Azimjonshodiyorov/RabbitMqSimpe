using AutoMapper;
using Event;
using MassTransit;
using MongoDB.Entities;
using SearchAnimalServices.Models;

namespace SearchAnimalServices.Consumers;

public class AnimalCreatedConsumer : IConsumer<AnimalCreated>
{
    private readonly IMapper _mapper;

    public AnimalCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AnimalCreated> context)
    {
        Console.WriteLine("Consuming animal created " + context.Message.Id);
        var animal = _mapper.Map<Animal>(context.Message);

        await animal.SaveAsync();
    }
}