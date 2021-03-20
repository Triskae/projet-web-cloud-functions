using AutoMapper;

namespace ProjetWeb.Models.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Models.User, Models.DTO.UserDto>();
        }
    }
}