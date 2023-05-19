
namespace nWeaveTask.DAL.Repositories.Products_Repo;

public class ProductsRepo : GenericRepo<Product> , IProductsRepo
{
    #region Fields
    private readonly nWeaveContext _context;
    #endregion

    #region Ctor
    public ProductsRepo(nWeaveContext context) : base(context)
    {
        _context = context;
    }
    #endregion

    #region Methods

    #endregion
}
