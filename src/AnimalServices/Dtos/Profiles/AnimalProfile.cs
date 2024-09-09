using AnimalServices.Entities;
using AutoMapper;
using Event;

namespace AnimalServices.Dtos.Profiles;

public class AnimalProfile : Profile
{
    public AnimalProfile()
    {
        CreateMap<Animal, AnimalDto>()
            .IncludeMembers(x => x.Address)
            .ReverseMap();
        CreateMap<AddressDto, AnimalDto>().ReverseMap();

        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<Address, AnimalDto>().ReverseMap();
            
        // From flat structure into nested
        CreateMap<CreateAnimalDto, Animal>().ForMember(d => d.Address, o => o.MapFrom(s => s)).ReverseMap();
        CreateMap<CreateAnimalDto, Address>().ReverseMap();

        // Publish it in flat structure
        CreateMap<AnimalDto, AnimalCreated>().IncludeMembers(x => x.Address).ReverseMap();;
        CreateMap<AddressDto, AnimalCreated>().ReverseMap();

        CreateMap<Animal, AnimalUpdated>().ReverseMap();
    }
}