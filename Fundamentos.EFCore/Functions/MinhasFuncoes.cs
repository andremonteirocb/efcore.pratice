using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Fundamentos.EFCore.Functions
{
    public static class MinhasFuncoes
    {
        [DbFunction(name: "LEFT", IsBuiltIn = true)]
        public static string Left(string dados, int quantidade)
        {
            throw new NotImplementedException();
        }

        public static void RegistrarFuncoes(ModelBuilder modelBuilder)
        {
            var funcoes = typeof(MinhasFuncoes).GetMethods().Where(p => Attribute.IsDefined(p, typeof(DbFunctionAttribute)));

            foreach (var funcao in funcoes)
                modelBuilder.HasDbFunction(funcao);
        }
    }
}
