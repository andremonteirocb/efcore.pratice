using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace CursoEFCore.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public List<Funcionario> Funcionarios { get; set; }

        //public Departamento() { }

        #region LazyLoading 1
        //private ILazyLoader _lazyLoader;
        //public Departamento(ILazyLoader lazyLoader)
        //{
        //    _lazyLoader = lazyLoader;
        //}

        //private List<Funcionario> _funcionarios;
        //public List<Funcionario> Funcionarios
        //{
        //    get => _lazyLoader.Load(this, ref _funcionarios);
        //    set => _funcionarios = value;
        //}
        #endregion

        #region LazyLoading 2
        //private Action<object, string> _lazyLoader;
        //public Departamento(Action<object, string> lazyLoader)
        //{
        //    _lazyLoader = lazyLoader;
        //}

        //private List<Funcionario> _funcionarios;
        //public List<Funcionario> Funcionarios
        //{
        //    get
        //    {
        //        _lazyLoader?.Invoke(this, nameof(Funcionarios));
        //        return _funcionarios;
        //    }
        //    set => _funcionarios = value;
        //}
        #endregion
    }
}