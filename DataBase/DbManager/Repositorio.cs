using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataBase.DbManager
{
    public class Repositorio<TEntity> : IRepositorio<TEntity> where TEntity : class, new()
    {
        protected DbContext _entities;
        protected DbSet<TEntity> _dbSet;

        public Repositorio(DbContext entities)
        {
            this._entities = entities;
            this._dbSet = entities.Set<TEntity>();
        }


        public IQueryable<TEntity> Listar
        {
            get { return _dbSet; }
        }
        public TEntity BuscarPorId(params object[] id)
        {
            return _dbSet.Find(id);
        }
        public virtual TEntity Crear(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public virtual TEntity Eliminar(TEntity entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }
        public virtual TEntity Modificar(TEntity editedEntity, TEntity originalEntity, out bool changed)
        {
            _entities.Entry(originalEntity).CurrentValues.SetValues(editedEntity);
            changed = _entities.Entry(originalEntity).State == EntityState.Modified;
            return originalEntity;
        }

        public void Confirmar()
        {
            _entities.SaveChanges();
        }

    }
}
