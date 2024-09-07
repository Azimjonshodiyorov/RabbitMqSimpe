using AnimalServices.Entities;

namespace AnimalServices.Helpers;

public  class EnumHelper
{
    public static EnumStatus EnumParse(string value, EnumStatus defaultStatus)
    {
        if (!Enum.TryParse(value, out EnumStatus animalStatus))
        {
            return defaultStatus;
        }
        return animalStatus;
    }
}