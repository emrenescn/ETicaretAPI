using ETicaretAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _writeRepository;
        private readonly IProductReadRepository _readRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;

        public ProductsController(IProductWriteRepository writeRepository, IProductReadRepository readRepository, IOrderWriteRepository orderWriteRepository, ICustomerWriteRepository customerWriteRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _orderWriteRepository = orderWriteRepository;
            _customerWriteRepository = customerWriteRepository;
        }
        
        [HttpGet]
        public async Task Get()
        {
            var customerId=Guid.NewGuid();
            await _customerWriteRepository.AddAsync(new() { Id = customerId,Name="Memati" });
            await _orderWriteRepository.AddRangeAsync(new() {
                new()
                { Description = "bombapbarabumbumpop",
                    Address = "Mersin",CustomerId=customerId},
            new(){Description="barabarabarabereberebere",Address="Kayseri",CustomerId=customerId}});
            await _orderWriteRepository.SaveAsync();
        }


    }
}
