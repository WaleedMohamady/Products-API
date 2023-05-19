
using AutoMapper;
using nWeaveTask.BL.DTOs;
using nWeaveTask.DAL;
using nWeaveTask.DAL.Repositories.Products_Repo;

namespace nWeaveTask.BL;

public class ProductsManager : IProductsManager
{
    #region Fields
    private readonly IProductsRepo _productsRepo;
    private readonly IMapper _mapper;
    #endregion

    #region Ctor
    public ProductsManager(IProductsRepo productsRepo, IMapper mapper)
    {
        _productsRepo = productsRepo;
        _mapper = mapper;
    }
    #endregion

    #region Methods
    public List<ProductReadDTO> GetAll()
    {
        var dbProducts = _productsRepo.GetAll();
        return _mapper.Map<List<ProductReadDTO>>(dbProducts);
    }

    public ProductReadDTO? GetById(Guid id)
    {
        var dbProduct = _productsRepo.GetById(id);
        if (dbProduct == null) { return null; }
        return _mapper.Map<ProductReadDTO>(dbProduct);
    }

    public ProductReadDTO Add(ProductAddDTO product)
    {
        var AddedProduct = _mapper.Map<Product>(product);
        AddedProduct.Id = Guid.NewGuid();
        _productsRepo.Add(AddedProduct);
        _productsRepo.SaveChanges();
        return _mapper.Map<ProductReadDTO>(AddedProduct);
    }

    public bool Update(ProductUpdateDTO product)
    {
        var UpdatedProduct = _productsRepo.GetById(product.Id);
        if (UpdatedProduct == null)
            return false;
        _mapper.Map(product, UpdatedProduct);
        _productsRepo.Update(UpdatedProduct);
        _productsRepo.SaveChanges();
        return true;
    }
    public void Delete(Guid id)
    {
        _productsRepo.DeleteById(id);
        _productsRepo.SaveChanges();
    }
    #endregion
}
