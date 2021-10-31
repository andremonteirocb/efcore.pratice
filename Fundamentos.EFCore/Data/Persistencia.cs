using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentos.EFCore.Data
{
    public class Persistencia<T> : IDisposable, IPersistencia<T> where T : class
    {
        #region Inicializacao 

        private ApplicationContext contexto;
        public ApplicationContext Contexto
        {
            get { return contexto; }
            set { contexto = value; }
        }
        public Persistencia()
        {
            contexto = new ApplicationContext();
        }

        #endregion 

        #region Metodos interface

        public virtual void Salvar(T obj)
        {
            contexto.Set<T>().Add(obj);
            contexto.SaveChanges();
        }
        public virtual void SalvarColecao(List<T> listaItems)
        {
            contexto.Set<T>().AddRange(listaItems);
            contexto.SaveChanges();
        }
        public virtual void Excluir(T obj)
        {
            //contexto.Remove(obj);
            //contexto.Entry<T>(obj).State = EntityState.Deleted;
            contexto.Set<T>().Remove(obj);
            contexto.SaveChanges();
        }
        public virtual void RemoverColecao(List<T> listaItems)
        {
            contexto.Set<T>().RemoveRange(listaItems);
            contexto.SaveChanges();
        }
        public virtual void Editar(T obj)
        {
            contexto.Entry(obj).State = EntityState.Modified;
            contexto.SaveChanges();
        }

        #endregion

        #region Métodos Externos

        public virtual T GetObj(object id)
        {
            return contexto.Set<T>().Find(id);
        }
        public void Dispose()
        {
            contexto.Dispose();
        }
        public List<T> GetAll()
        {
            return contexto.Set<T>().ToList();
        }

        #endregion
    }
}
