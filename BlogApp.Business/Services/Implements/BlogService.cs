using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using BlogApp.Business.Dtos.BlogDtos;
using BlogApp.Business.Dtos.CategoryDtos;
using BlogApp.Business.Exceptions.Category;
using BlogApp.Business.Exceptions.Commons;
using BlogApp.Business.Exceptions.User;
using BlogApp.Business.Services.Interfaces;
using BlogApp.Core.Entities;
using BlogApp.Core.Enums;
using BlogApp.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Business.Services.Implements;

public class BlogService : IBlogService
{
    readonly IBlogRepository _repo;
    readonly IHttpContextAccessor _context;
    readonly IMapper _mapper;
    readonly IBlogLikeRepository _blogLikeRepo;
    readonly ICategoryRepository _categoryRepository;
    readonly UserManager<AppUser> _userManager;
    readonly string? userId;

    public BlogService(IBlogRepository repo, IHttpContextAccessor context, IMapper mapper, ICategoryRepository categoryRepository, UserManager<AppUser> userManager, IBlogLikeRepository blogLikeRepo)
    {
        _repo = repo;
        _context = context;
        userId = _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
        _userManager = userManager;
        _blogLikeRepo = blogLikeRepo;
    }

    public async Task CreateAsync(BlogCreateDto dto)
    {
        if (String.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException();
        if (!await _userManager.Users.AnyAsync(u => u.Id == userId)) throw new UserNotFoundException();
        List<BlogCategory> blogCats = new();
        foreach (var id in dto.CategoryIds)
        {
            var cat = await _categoryRepository.FindByIdAsync(id);
            if (cat == null) throw new CategoryNotFoundException();
            blogCats.Add(new BlogCategory { Category = cat});
        }
        Blog blog = _mapper.Map<Blog>(dto);
        blog.AppUserId = userId;
        blog.BlogCategories = blogCats;
        await _repo.CreateAsync(blog);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<BlogListItemDto>> GetAllAsync()
    {
        //1ci usul
        var dto = new List<BlogListItemDto>();
        var entity = _repo.GetAll("AppUser", "BlogCategories", "BlogCategories.Category",
            "Comments", "Comments.Children","Comments.AppUser","BlogLikes");
        List<Category> categories = new();
        foreach (var item in entity)
        {
            categories.Clear();
            foreach (var category in item.BlogCategories)
            {
                categories.Add(category.Category);
            }
            var dtoItem = _mapper.Map<BlogListItemDto>(item);
            dtoItem.Categories = _mapper.Map<IEnumerable<CategoryListItemDto>>(categories);
            dtoItem.ReactCount = item.BlogLikes.Count;
            dto.Add(dtoItem);
        }
        return dto;


        //2 ci usul
        //var entity = _repo.GetAll("AppUser", "BlogCategories", "BlogCategories.Category");
        //return _mapper.Map<IEnumerable<BlogListItemDto>>(entity);

    }

    public async Task<BlogDetailDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException();
        var entity = await _repo.FindByIdAsync(id,"AppUser","BlogCategories","BlogCategories.Category",
            "Comments","Comments.Children","Comments.AppUser","BlogLikes","BlogLikes.AppUser");
        if (entity == null) throw new NotFoundException<Blog>();
        entity.ViewerCount++;
        await _repo.SaveAsync();
        return _mapper.Map<BlogDetailDto>(entity);
    }


    public async Task ReactAsync(int id, Reactions reaction)
    {
        await _checkValidate(id);
        var blog = await _repo.FindByIdAsync(id, "BlogLikes");
        if(!blog.BlogLikes.Any(bl => bl.BlogId == id && bl.AppUserId == userId))
        {
            blog.BlogLikes.Add(new BlogLike
            {
                BlogId=id,
                AppUserId=userId,
                Reaction=reaction
            });
        }
        else {
            var currentReaction = blog.BlogLikes.FirstOrDefault(bl => bl.BlogId == id && bl.AppUserId == userId);
            if (currentReaction == null) throw new NotFoundException<BlogLike>();
            currentReaction.Reaction = reaction;
        }
        await _repo.SaveAsync();
    }

    public async Task RemoveReactAsync(int id)
    {
        await _checkValidate(id);
        var entity =await _blogLikeRepo.GetSingleAsync(bl => bl.AppUserId == userId && bl.BlogId == id);
        if (entity is null) throw new NotFoundException<BlogLike>();
        _blogLikeRepo.Delete(entity);
        await _repo.SaveAsync();
    }


    public async Task RemoveAsync(int id)
    {
        await _checkValidate(id);
        var entity = await _repo.FindByIdAsync(id);
        if (entity is null) throw new NotFoundException<Blog>();
        if (entity.AppUserId != userId) throw new UserHasNotAccessException();
        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, BlogUpdateDto dto)
    {
        await _checkValidate(id);
        var entity = await _repo.GetAll("BlogCategories", "BlogCategories.Category")
                                 .SingleOrDefaultAsync(blog => blog.Id == id);
        if (entity == null) throw new NotFoundException<Blog>();
        if (entity.AppUserId != userId) throw new UserHasNotAccessException();

        entity.BlogCategories?.Clear();

        foreach (var itemId in dto.CategoryIds)
        {
            var cat = await _categoryRepository.FindByIdAsync(itemId);
            if (cat == null) throw new CategoryNotFoundException();
            entity.BlogCategories?.Add(new BlogCategory { Category = cat });
        }

        entity.AppUserId = userId;
        _mapper.Map(dto, entity);
        await _repo.SaveAsync();
    }

    async Task _checkValidate(int id)
    {
        if (id <= 0) throw new NegativeIdException();
        if (String.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException();
        if (!await _userManager.Users.AnyAsync(u => u.Id == userId)) throw new UserNotFoundException();
    }

}


