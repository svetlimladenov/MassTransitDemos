using AutoMapper;
using Sample.Api.ViewModels;
using Sample.Contracts.UtilizeCredit;

namespace Sample.Api.Automapper
{
    public class ApiMapperProfile : Profile
    {
        public ApiMapperProfile()
        {
            CreateMap<CreateCreditInputModel, CreateCreditDTO>();

            CreateMap<BonusPointInputModel, BonusPointDTO>();
        }
    }
}
