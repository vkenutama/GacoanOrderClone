using AutoMapper;
using Gacoan.Backend.Common.Models;
using Gacoan.Backend.Dtos;
using Gacoan.Backend.Models;
using Gacoan.Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gacoan.Backend.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<ProductType> _productTypeRepository;
    private readonly IMapper _mapper;

    public ProductService(
        IRepository<Product> productRepository, 
        IRepository<ProductType> productTypeRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _productTypeRepository = productTypeRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<ProductDto>>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetQueryable()
            .Include(p => p.Type)
            .ToListAsync(cancellationToken);

        var dtoList = _mapper.Map<List<ProductDto>>(products);
        return ServiceResult<List<ProductDto>>.Ok(dtoList);
    }

    public async Task<ServiceResult<ProductDto>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetQueryable()
            .Include(p => p.Type)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (product == null)
        {
            return ServiceResult<ProductDto>.Fail("Produk tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        var dto = _mapper.Map<ProductDto>(product);
        return ServiceResult<ProductDto>.Ok(dto);
    }

    public async Task<ServiceResult<ProductDto>> CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        // Validasi keberadaan Kategori (ProductType)
        var type = await _productTypeRepository.GetByIdAsync(dto.TypeId, cancellationToken);
        if (type == null)
        {
            return ServiceResult<ProductDto>.Fail("Kategori produk (TypeId) tidak ditemukan.", StatusCodes.Status400BadRequest);
        }

        var product = _mapper.Map<Product>(dto);
        product.Id = Guid.NewGuid();

        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        // Pasang referensi Type agar response memuat TypeName
        product.Type = type;
        var responseDto = _mapper.Map<ProductDto>(product);

        return ServiceResult<ProductDto>.Ok(responseDto, "Produk berhasil ditambahkan.", StatusCodes.Status201Created);
    }

    public async Task<ServiceResult<ProductDto>> UpdateProductAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetQueryable()
            .Include(p => p.Type)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (product == null)
        {
            return ServiceResult<ProductDto>.Fail("Produk tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        // Cek kategori jika diubah
        if (product.TypeId != dto.TypeId)
        {
            var type = await _productTypeRepository.GetByIdAsync(dto.TypeId, cancellationToken);
            if (type == null)
            {
                return ServiceResult<ProductDto>.Fail("Kategori produk (TypeId) tidak ditemukan.", StatusCodes.Status400BadRequest);
            }
            product.Type = type;
        }

        // Update properties
        _mapper.Map(dto, product);

        await _productRepository.SaveChangesAsync(cancellationToken);

        var responseDto = _mapper.Map<ProductDto>(product);
        return ServiceResult<ProductDto>.Ok(responseDto, "Produk berhasil diperbarui.");
    }

    public async Task<ServiceResult<bool>> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            return ServiceResult<bool>.Fail("Produk tidak ditemukan.", StatusCodes.Status404NotFound);
        }

        await _productRepository.RemoveAsync(id, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.Ok(true, "Produk berhasil dihapus.");
    }
}
