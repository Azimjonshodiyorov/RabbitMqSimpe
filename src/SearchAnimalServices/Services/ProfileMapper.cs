using AutoMapper;
using Event;
using SearchAnimalServices.Models;

namespace SearchAnimalServices.Services;

public class ProfileMapper : Profile
{
    public ProfileMapper()
    {

        CreateMap<AnimalCreated, Animal>().ForMember(d => d.Address, o => o.MapFrom(s => s));
        CreateMap<AnimalCreated, Address>();
        CreateMap<AnimalUpdated, Animal>().ForMember(d => d.Address, o => o.MapFrom(s => s));
    }
}