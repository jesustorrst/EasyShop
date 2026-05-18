using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Features.Categories.Queries.GetCategoryList;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EasyShop.Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var query = new GetCategoryListQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }


}