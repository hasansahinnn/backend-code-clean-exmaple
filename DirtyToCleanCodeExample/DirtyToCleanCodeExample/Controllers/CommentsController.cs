using Core.Constants;
using Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace DirtyToCleanCodeExample.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(description: ResponseDescriptions.CreateComment)]
    [HttpPost]
    public async Task<IActionResult> CreatePost(CreateCommentDto createCommentDto)
    {
        var result = await _commentService.AddCommentAsync(createCommentDto);
        return Ok(result);
    }
}