using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentos.EFCore.Data
{
    interface IPersistencia<T> 
    {
        void Salvar(T obj);
        void Editar(T obj);
        void Excluir(T obj);
        T GetObj(object id);
        List<T> GetAll();
    }
}
