using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Features.Products.Queries.GetProductList;
using EasyShop.Catalog.Application.Features.Products.Commands.CreateProduct;
using EasyShop.Catalog.Application.Features.Products.Queries.GetProductById;
using EasyShop.Catalog.Application.Features.Products.Commands.UpdateProduct;
using EasyShop.Catalog.Application.Features.Products.Commands.DeleteProduct;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EasyShop.Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // [HttpGet(Name = "GetProducts")]
    // [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    // public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
    // {
    //     var query = new GetProductListQuery();
    //     var products = await _mediator.Send(query);
    //     return Ok(products);
    // }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var query = new GetProductListQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidarModeloFilter))]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        var command = new CreateProductCommand(createProductDto);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProducts), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, [FromBody] CreateProductDto updateProductDto)
    {
        var command = new UpdateProductCommand(id, updateProductDto);
        var result = await _mediator.Send(command);

        if (result == null)
            return NotFound();

        return Ok(result);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteProduct(Guid id)
    {
        var command = new DeleteProductCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return Ok(result);
    }



}