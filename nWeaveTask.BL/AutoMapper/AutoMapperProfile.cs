
using AutoMapper;
using nWeaveTask.BL.DTOs;
using nWeaveTask.DAL;

namespace nWeaveTask.BL;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<Product, ProductReadDTO>();
        CreateMap<ProductAddDTO, Product>();
        CreateMap<ProductUpdateDTO, Product>();

    }
}
