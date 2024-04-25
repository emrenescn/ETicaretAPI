using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Features.Commands.Product.CreateProduct;
using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using ETicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using ETicaretAPI.Application.Features.Commands.ProductImage.RemoveProductImage;
using ETicaretAPI.Application.Features.Commands.ProductImage.UploadProductImage;
using ETicaretAPI.Application.Features.Queries.Product.GetAllProduct;
using ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ProductsController(IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment,
            IFileReadRepository fileReadRepository,
            IFileWriteRepository fileWriteRepository,
            IProductImageFileReadRepository productImageFileReadRepository,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IInvoiceFileReadRepository invoiceFileReadRepository,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration,
            IMediator mediator)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response= await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult>Get([FromRoute]GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response =await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response= await _mediator.Send(createProductCommandRequest);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response=await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest)
        {
            RemoveProductCommandResponse response= await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery]UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product=await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return Ok(product.ProductImageFiles.Select(p => new
            {
                Path = $"{_configuration["BaseStroageUrl"]}/{p.Path}",
               p.FileName,
               p.Id
            })) ;
        }
        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute]RemoveProductImageCommandRequest removeProductImageCommandRequest, [FromQuery]string ImageId)
        {
            removeProductImageCommandRequest.ImageId=ImageId;
            RemoveProductImageCommandResponse response=await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }
    }
}
