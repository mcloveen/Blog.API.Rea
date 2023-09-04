using System.Security.Claims;
using AutoMapper;
using BlogApp.Business.Dtos.CommentDtos;
using BlogApp.Business.Exceptions.Commons;
using BlogApp.Business.Exceptions.User;
using BlogApp.Business.Services.Interfaces;
using BlogApp.Core.Entities;
using BlogApp.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Business.Services.Implements;

public class CommentService : ICommentService
{
    readonly ICommentRepository _repo;
    readonly IMapper _mapper;
    readonly UserManager<AppUser> _userManager;
    readonly string _userId;
    readonly IBlogRepository _blogRepo;
    readonly IHttpContextAccessor _httpContextAccessor;

    public CommentService(ICommentRepository repo, IMapper mapper, UserManager<AppUser> userManager,  IBlogRepository blogRepo, IHttpContextAccessor httpContextAccessor)
    {
        _repo = repo;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _blogRepo = blogRepo;
    }

    public async Task CreateAsync(int id, CommentCreateDto dto)
    {
        if (String.IsNullOrWhiteSpace(_userId)) throw new ArgumentNullException();
        if (!await _userManager.Users.AnyAsync(u => u.Id == _userId)) throw new UserNotFoundException();
        if (id <= 0) throw new NegativeIdException();
        if (!await _blogRepo.IsExistAsync(b => b.Id == id)) throw new NotFoundException<Blog>();
        var comment = _mapper.Map<Comment>(dto);
        comment.AppUserId = _userId;
        comment.BlogId = id;
        await _repo.CreateAsync(comment);
        await _repo.SaveAsync();
    }

    public Task<IEnumerable<CommentListItemDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}

