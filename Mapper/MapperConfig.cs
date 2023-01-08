using AutoMapper;
using VilllaParks.Model;
using VilllaParks.Model.Dto;

namespace VilllaParks.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<VillaPark, VillaParkDto>().ReverseMap();
            CreateMap<VillaPark, VillaParkCreatedDTO>().ReverseMap();
            CreateMap<VillaPark, VillaParkUpdatedDTO>().ReverseMap();


            CreateMap<VillaParkNumber, VillaParkNumberDto>().ReverseMap();

            // CreateMap<TouristHubDto, TouristHub>();

            CreateMap<VillaParkNumber, VillaParkNoCreatDTO>().ReverseMap();
            CreateMap<VillaParkNumber, VillaParkNoUpdateDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }

    }
}
