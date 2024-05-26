using Core.Constants;
using Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace DirtyToCleanCodeExample.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }
    
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(description: ResponseDescriptions.CreateBorrow)]
    [HttpPost]
    public async Task<IActionResult> CreateBorrow(CreateBorrowDto createBorrowDto)
    {
        var result = await _bookService.BorrowBooksAsync(createBorrowDto);
        return Ok(result);
    }
        
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(description: ResponseDescriptions.GetRecentBorrowers)]
    [HttpGet("[action]/{daysAgo}")]
    public IAsyncEnumerable<string> GetRecentBorrowers(int daysAgo)
    {
        return _bookService.GetRecentBorrowersAsync(daysAgo);
    }

    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(description: ResponseDescriptions.SendEmailsForRecentBorrowers)]
    [HttpPost("[action]/{daysAgo}")]
    public async Task<IActionResult> SendEmailsForRecentBorrowers(int daysAgo)
    {
        await _bookService.SendEmailsForRecentBorrowersAsync(daysAgo);
        return Ok();
    }
    
}