using CursoEFCore.Domain;
using Fundamentos.EFCore.Data;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public class ClienteAplicacao : Persistencia<Cliente>
    {
        #region Outros Metodos

        public List<Cliente> ListarProdutos()
        {
            return Contexto.Clientes.ToList();
        }
        public IQueryable ListarProdutosLinq()
        {
            //Linq puro
            IQueryable lista = from c in base.Contexto.Clientes
                               where c.Id == 8
                               orderby c.Nome
                               select c;

            //Linq Lambda
            IQueryable listaBase = base.Contexto.Clientes.Where(p => p.Id == 8).OrderBy(p => p.Nome);

            return lista;
        }

        #endregion
    }
}
