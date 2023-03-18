using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IBusinessLogic<T> where T : class
    {
        IList<T> ListAll();
        int Crear(T model);

        T BuscarPorId(params object[] keyValues);

    }
}
