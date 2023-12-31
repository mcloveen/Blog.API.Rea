﻿using AutoMapper;
using BlogApp.Business.Dtos.BlogDtos;
using BlogApp.Business.Dtos.BlogLikeDtos;
using BlogApp.Core.Entities;

namespace BlogApp.Business.Profiles;

public class BlogMappingProfile:Profile
{
    public BlogMappingProfile()
    {
        CreateMap<Blog,BlogListItemDto>();
        CreateMap<Blog,BlogDetailDto>();
        CreateMap<BlogCreateDto, Blog>();
        CreateMap<BlogUpdateDto, Blog>();
        CreateMap<BlogCategory, BlogCategoryDto>().ReverseMap();
        CreateMap<BlogLike,BlogLikeListItemDto>().ReverseMap();
    }
}

