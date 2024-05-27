using AutoMapper;
using Core.Data.Repositories;
using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.DTOs;
using Service.Rules;
using static Service.Constants.Messages;

namespace Service.Services;

public interface IPostService
{
    /// <summary>
    /// Creates a new post.
    /// Throws an exception if the user does not exist.
    /// Returns a success message if the post is created successfully.
    /// </summary>
    /// <param name="createPostDto"></param>
    Task<string> CreatePostAsync(CreatePostDto createPostDto);
    /// <summary>
    /// Deletes the post with the given id.
    /// Throws an exception if the post does not exist.
    /// Returns a success message if the post is deleted successfully.
    /// </summary>
    /// <param name="postId"></param>
    Task<string> DeleteAsync(int postId);
    /// <summary>
    /// Publishes the post with the given id.
    /// Throws an exception if the post does not exist.
    /// Returns a success message if the post is published successfully.
    /// </summary>
    /// <param name="publishPostDto"></param>
    Task<string> PublishPostAsync(PublishPostDto publishPostDto);
    /// <summary>
    /// Gets the list of posts and their users.
    /// </summary>
    Task<List<PostsListDto>> GetPostsAndUsersAsync();
    /// <summary>
    /// Archives the posts that are older than 30 days.
    /// </summary>
    Task ArchiveOldPostsAsync();
    /// <summary>
    /// Deletes the posts of the user with the given id.
    /// Throws an exception if the user does not exist.
    /// </summary>
    /// <param name="userId"></param>
    Task DeletePostAsync(int userId);
}

public class PostService : IPostService
{
    private readonly IRepository<Post, DataContext> _postRepository;
    private readonly IRepository<User, DataContext> _userRepository;
    private readonly IMapper _mapper;
    private readonly UserBusinessRule _userBusinessRule;
    private readonly PostBusinessRule _postBusinessRule;

    public PostService(
            IRepository<Post, DataContext> postRepository,
            IRepository<User, DataContext> userRepository,
            IMapper mapper,
            UserBusinessRule userBusinessRule,
            PostBusinessRule postBusinessRule)
    {
        _postRepository = postRepository;
        _mapper = mapper;
        _userBusinessRule = userBusinessRule;
        _postBusinessRule = postBusinessRule;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Gets the list of posts and their users.
    /// </summary>
    public async Task<List<PostsListDto>> GetPostsAndUsersAsync()
    {
        var posts = await _postRepository.GetListAsync(
            predicate: null,
            orderBy: null,
            include: x => x.Include(p => p.User));

        var mappedPostsListDto = _mapper.Map<List<PostsListDto>>(posts);
        return mappedPostsListDto;
    }

    /// <summary>
    /// Creates a new post.
    /// Throws an exception if the user does not exist.
    /// Returns a success message if the post is created successfully.
    /// </summary>
    /// <param name="createPostDto"></param>
    public async Task<string> CreatePostAsync(CreatePostDto createPostDto)
    {
        await _postBusinessRule.ValidateId(createPostDto.UserId);
        await _postBusinessRule.ValidateContent(createPostDto.Content);

        var user = await _userBusinessRule.CanUserCreatePost(createPostDto.UserId);
        var createPost = _mapper.Map<Post>(createPostDto);
        createPost.Status = PostStatus.Draft;
        createPost.CreatedDate = DateTime.UtcNow;

        await _postRepository.AddAsync(createPost);
        await IncrementPostCountAsync(user);

        return PostCreatedSuccessfully;
    }

    /// <summary>
    /// Increments the post count of the user.
    /// </summary>
    /// <param name="user"></param>
    private async Task IncrementPostCountAsync(User user)
    {
        user.PostCount += 1;
        await _userRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the post with the given id.
    /// Throws an exception if the post does not exist.
    /// Returns a success message if the post is deleted successfully.
    /// </summary>
    /// <param name="postId"></param>
    public async Task<string> DeleteAsync(int postId)
    {
        await _postBusinessRule.ValidateId(postId);

        var post = await _postRepository.GetSingleOrDefaultAsync(x => x.Id == postId);
        await _postBusinessRule.PostCannotBeNull(post);

        post!.Status = PostStatus.Deleted;
        post.UpdatedDate = DateTime.UtcNow;

        await _postRepository.SaveChangesAsync();
        return PostDeleted;
    }

    /// <summary>
    /// Publishes the post with the given id.
    /// Throws an exception if the post does not exist.
    /// Returns a success message if the post is published successfully.
    /// </summary>
    /// <param name="publishPostDto"></param>
    public async Task<string> PublishPostAsync(PublishPostDto publishPostDto)
    {
        await _postBusinessRule.ValidateId(publishPostDto.PostId);
        await _postBusinessRule.ValidateStatus(publishPostDto.Status);

        var post = await _postRepository.GetSingleOrDefaultAsync(x => x.Id == publishPostDto.PostId);
        await _postBusinessRule.PostCannotBeNull(post);

        post!.Status = (PostStatus)publishPostDto.Status;
        post.UpdatedDate = DateTime.UtcNow;

        await _postRepository.SaveChangesAsync();
        return PostPublishedSuccessfully;
    }

    /// <summary>
    /// Archives the posts that are older than 30 days.
    /// </summary>
    public async Task ArchiveOldPostsAsync()
    {
        var posts = await _postRepository.GetListAsync();
        await _postBusinessRule.ThereMustBeAnElementInThePostList(posts);

        foreach (var post in posts)
        {
            if (post.CreatedDate < DateTime.UtcNow.AddDays(-30))
                post.Status = PostStatus.Archived;
        }
        await _postRepository.UpdateRangeAsync(posts);
    }

    /// <summary>
    /// Deletes the posts of the user with the given id.
    /// Throws an exception if the user does not exist.
    /// </summary>
    /// <param name="userId"></param>
    public async Task DeletePostAsync(int userId)
    {
        await _userBusinessRule.IdShouldBeGreaterThanZero(userId);
        await _userBusinessRule.UserIdShouldExistWhenSelected(userId);
        var userPosts = await _postRepository.GetListAsync(x => x.UserId == userId);
        await _postBusinessRule.ThereMustBeAnElementInThePostList(userPosts);
        await _postRepository.DeleteRangeAsync(userPosts);
    }
}