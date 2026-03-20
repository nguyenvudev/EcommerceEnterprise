using EcommerceEnterprise.Application.Features.Products.Commands.CreateProduct;
using EcommerceEnterprise.Application.Features.Products.Queries.GetProductDetail;
using EcommerceEnterprise.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceEnterprise.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    /// <summary>Lấy danh sách sản phẩm có filter + phân trang</summary>
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] GetProductsQuery query,
        CancellationToken ct)
        => Ok(await mediator.Send(query, ct));

    /// <summary>Lấy chi tiết sản phẩm theo slug</summary>
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductDetail(
        string slug, CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetProductDetailQuery(slug), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }

    /// <summary>Tạo sản phẩm mới — chỉ Admin/Manager</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager,SuperAdmin")]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetProductDetail),
                new { slug = command.Slug },
                new { id = result.Value })
            : BadRequest(new { error = result.Error });
    }
}