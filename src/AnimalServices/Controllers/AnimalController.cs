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
    
    
}