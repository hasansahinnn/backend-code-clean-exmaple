using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;
using Swashbuckle.AspNetCore.Annotations;
using Core.Constants;
using Core.Extensions;

namespace DirtyToCleanCodeExample.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public PostsController(IPostService postService, ICommentService commentService)
    {
        _postService = postService;
        _commentService = commentService;
    }
    
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(description: ResponseDescriptions.CreatePost)]
    [HttpPost]
    public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
    {
        var result = await _postService.CreatePostAsync(createPostDto);
        return Ok(result);
    }
    
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(description: ResponseDescriptions.DeletePost)]
    [HttpDelete("[action]/{postId}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        var result = await _postService.DeleteAsync(postId);
        return Ok(result);
    }

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(description: ResponseDescriptions.PublishPost)]
    [HttpPost("[action]")]
    public async Task<IActionResult> PublishPostAsync(PublishPostDto publishPostDto)
    {
        var result = await _postService.PublishPostAsync(publishPostDto);
        return Ok(result);
    }
    
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(description: ResponseDescriptions.AddCommentAndArchiveOldPosts)]
    [HttpPost("[action]")]
    public async Task<IActionResult> AddCommentAndArchiveOldPostsAsync(CreateCommentDto createCommentDto)
    {
        var result = await _commentService.AddCommentAsync(createCommentDto);
        await _postService.ArchiveOldPostsAsync();
        
        return Ok(result);
    }
    
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(description: ResponseDescriptions.GetPostsAndUsers)]
    [HttpGet("[action]")]
    public async Task<IActionResult> GetPostsAndUsers()
    {
        var result = await _postService.GetPostsAndUsersAsync();
        return Ok(result);
    }
}