using System.Linq;

namespace DataBase.DbManager
{
    public interface IRepositorio<T>
    {
        IQueryable<T> Listar { get; }
        T BuscarPorId(params object[] id);
        T Eliminar(T entity);
        T Crear(T entity);
        T Modificar(T entity, T oldEntity, out bool modified);
        void Confirmar();
    }
}
