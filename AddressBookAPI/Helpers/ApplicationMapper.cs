
using AddressBookAPI.Entity.Dto;
using AddressBookAPI.Entity.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Helpers
{
    public class ApplicationMapper : Profile
    {
      
        public ApplicationMapper()
        {

            CreateMap<Address, AddressDTO>().ForMember(e => e.Type, src => src.MapFrom(act => new TypeDTO { Key = act.RefTermId.ToString() }))
                       .ForMember(c => c.Country, src => src.MapFrom(act => new TypeDTO { Key = act.Country.ToString() }));

            CreateMap<Email, EmailDTO>().ForMember(e => e.Type, src => src.MapFrom(act => new TypeDTO { Key = act.RefTermId.ToString() }));

            CreateMap<Phone, PhoneDTO>().ForMember(e => e.Type, src => src.MapFrom(act => new TypeDTO { Key =  act.RefTermId.ToString() }));


            CreateMap<User, UserDTO>();

            CreateMap<AddressDTO, Address>().ForMember
                (t => t.RefTermId, dst => dst.MapFrom(src => src.Type.Key))
                .ForMember(c => c.Country, dst => dst.MapFrom(src => src.Country.Key));

            CreateMap<EmailDTO, Email>().ForMember
                (t => t.RefTermId, dst => dst.MapFrom(src => src.Type.Key));
            CreateMap<PhoneDTO, Phone>().ForMember
                (t => t.RefTermId, dst => dst.MapFrom(src => src.Type.Key));
            CreateMap<UserDTO, User>();


        }
    }
}
