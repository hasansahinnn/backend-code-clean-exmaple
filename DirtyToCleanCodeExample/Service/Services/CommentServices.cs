using AutoMapper;
using Core.Data.Repositories;
using Data.Contexts;
using Data.Models;
using Service.DTOs;
using Service.Rules;
using static Service.Constants.Messages;

namespace Service.Services;

public interface ICommentService
{
    /// <summary>
    /// Adds a comment to the database.
    /// </summary>
    /// <param name="createCommentDto"></param>
    Task<string> AddCommentAsync(CreateCommentDto createCommentDto);
}

public class CommentService : ICommentService
{
    private readonly IRepository<Comment, DataContext> _repository;
    private readonly IMapper _mapper;
    private readonly PostBusinessRule _postBusinessRule;
    private readonly UserBusinessRule _userBusinessRule;

    public CommentService
    (
        IRepository<Comment, DataContext> repository,
        IMapper mapper,
        UserBusinessRule userBusinessRule,
        PostBusinessRule postBusinessRule
        )
    {
        _repository = repository;
        _mapper = mapper;
        _userBusinessRule = userBusinessRule;
        _postBusinessRule = postBusinessRule;
    }

    /// <summary>
    /// Adds a comment to the database.
    /// </summary>
    /// <param name="createCommentDto"></param>
    public async Task<string> AddCommentAsync(CreateCommentDto createCommentDto)
    {
        await _userBusinessRule.IdShouldBeGreaterThanZero(createCommentDto.UserId);
        await _postBusinessRule.ValidateId(createCommentDto.PostId);
        await _userBusinessRule.UserIdShouldExistWhenSelected(createCommentDto.UserId);
        await _postBusinessRule.PostIdShouldExistWhenSelected(createCommentDto.PostId);

        var mappedPost = _mapper.Map<Comment>(createCommentDto);

        mappedPost.CreatedDate = DateTime.UtcNow;
        await _repository.AddAsync(mappedPost);

        return CommentAddSuccessfully;
    }
}