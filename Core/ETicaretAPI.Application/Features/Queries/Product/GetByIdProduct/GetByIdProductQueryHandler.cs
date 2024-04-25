﻿using ETicaretAPI.Application.Repositories;
using MediatR;
using P=ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        public GetByIdProductQueryHandler(IProductReadRepository productReadRepository)
        {

            _productReadRepository = productReadRepository;

        }
        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
            P.Product product=await _productReadRepository.GetByIdAsync(request.Id, false);
            return new() { 
            Name=product.Name,
            Stock=product.Stock,
            Price=product.Price
            };

        }
    }
}
