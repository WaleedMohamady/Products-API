
using nWeaveTask.DAL.Repositories.Generic_Repo;

namespace nWeaveTask.DAL;

public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : class
{
    #region Fields
    private readonly nWeaveContext _context;
    #endregion

    #region Ctor
    public GenericRepo(nWeaveContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public List<TEntity> GetAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    public TEntity? GetById(Guid id)
    {
        return _context.Set<TEntity>().Find(id);
    }
    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {

    }

    public void DeleteById(Guid id)
    {
        var deletedEntity = GetById(id);
        if (deletedEntity != null)
        {
            _context.Set<TEntity>().Remove(deletedEntity);
        }
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    #endregion
}
