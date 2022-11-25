
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

            CreateMap<address, AddressDTO>().ForMember(e => e.type, src => src.MapFrom(act => new TypeDTO { key = act.refTermId.ToString() }))
                       .ForMember(c => c.country, src => src.MapFrom(act => new TypeDTO { key = act.country.ToString() }));
          
            CreateMap<email, EmailDTO>().ForMember(e => e.type, src => src.MapFrom(act => new TypeDTO { key = act.refTermId.ToString() }));

            CreateMap<phone, PhoneDTO>().ForMember(e => e.type, src => src.MapFrom(act => new TypeDTO { key = act.refTermId.ToString() }));


            

            CreateMap<user, UserDTO>();

            CreateMap<AddressDTO, address>().ForMember
                (t => t.refTermId, dst => dst.MapFrom(src => src.type.key))
                .ForMember(c => c.country, dst => dst.MapFrom(src => src.country.key));

            CreateMap<EmailDTO, email>().ForMember
                (t => t.refTermId, dst => dst.MapFrom(src => src.type.key));
            CreateMap<PhoneDTO, phone>().ForMember
                (t => t.refTermId, dst => dst.MapFrom(src => src.type.key));
            CreateMap<UserDTO, user>();


        }
    }
}
