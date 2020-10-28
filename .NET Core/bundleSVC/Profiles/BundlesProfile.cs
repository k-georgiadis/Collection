using AutoMapper;
using BundleSVC.DTO;
using bundleSVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bundleSVC.DTO;

namespace bundleSVC.Profiles
{
    public class BundlesProfile : Profile
    {
        public BundlesProfile()
        {
            //Source -> Destination
            CreateMap<Bundle, BundleReadDTO>();
            CreateMap<BundleAddDTO, Bundle>();
            CreateMap<BundleUpdateDTO, Bundle>();
            CreateMap<Bundle, BundleUpdateDTO>();
        }
    }
}
