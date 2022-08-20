using System;
using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.IO;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Fundamentos.EFCore.Interceptadores;
using System.Reflection;
using Fundamentos.EFCore.Functions;

namespace Fundamentos.EFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString = "Server=localhost, 1433;Database=DbEFCore;User Id=sa;Password=senha;Pooling=false;MultipleActiveResultSets=true;";
        private readonly StreamWriter _writer = new StreamWriter("meu_log_do_ef_core.txt", append: true);
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging() //exibe os parametros sensiveis nos logs console
                .EnableDetailedErrors() //habilitando erros detalhados na aplicação
                //.UseLazyLoadingProxies() //ativa o lazyloading para todas as propriedades virtual
                //.LogTo(Console.WriteLine, LogLevel.Information) //log no console
                .LogTo(_writer.WriteLine, new[]
                    {
                        CoreEventId.ContextInitialized,
                        RelationalEventId.CommandExecuted
                    },
                    LogLevel.Information,
                    DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine
                ) //gera o log no arquivo
                .AddInterceptors(new InterpcetorComandos())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
                .UseSqlServer(
                    _connectionString,
                        o => o
                            .EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null)
                            .MigrationsHistoryTable("curso_ef_core") //alterar o nome da tabela migrations padrão
                            .CommandTimeout(60) //aumenta o timeout das queries para 60 segundos
                            .MaxBatchSize(100) //aumenta o limite de inserts em massa de 42 para o seu desejado
                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery) //ativa o splitquerie para toda a aplicação
                 );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            //adicionando um filtro global, todos os departamentos excluídos não serão buscados
            modelBuilder.Entity<Departamento>().HasQueryFilter(p => !p.Excluido);

            //adicionando um index único
            modelBuilder.Entity<Departamento>().HasIndex(p => p.Descricao).IsUnique();

            //adicionando um index composto
            //modelBuilder.Entity<Departamento>()
            //            .HasIndex(p => new { p.Descricao, p.Ativo })
            //            .HasFilter("Descricao IS NOT NULL")
            //            .HasFillFactor(80)
            //            .HasDatabaseName("idx_meu_indice_composto")
            //            .IsUnique();

            //utilizando uma determinada collection específica globamente
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            //utilizando uma determinada collection específica para uma propriedade de uma classe específica
            modelBuilder.Entity<Departamento>().Property(d => d.Descricao).UseCollation("SQL_Latin1_General_CP1_CI_AS");

            //criando e utilizando uma determinada sequência específica
            modelBuilder.HasSequence<int>("nome_da_sequencia", "schema_sequencia").StartsAt(1).IncrementsBy(2).HasMin(1).HasMax(10).IsCyclic();
            modelBuilder.Entity<Departamento>().Property(d => d.Id).HasDefaultValueSql("NEXT VALUE FOR schema_sequencia.nome_da_sequencia");

            //Segue abaixo todas as conversões definidas
            var conversao = new ValueConverter<StatusPedido, string>(p => p.ToString(), p => (StatusPedido)Enum.Parse(typeof(StatusPedido), p));
            var conversao1 = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.StringToEnumConverter<StatusPedido>();

            //modelBuilder.Entity<Pedido>()
            //    .Property(d => d.Status)
            //    .HasConversion(conversao1);

            //registrando dbfunctions
            //MinhasFuncoes.RegistrarFuncoes(modelBuilder);

            modelBuilder.HasDbFunction(_minhaFuncao)
                    .HasName("LEFT")
                    .IsBuiltIn();

            //adicionando a configuração padrão para colunas que não foram configuradas
            MapearPropriedadesEsquecidas(modelBuilder);
        }

        private static MethodInfo _minhaFuncao = typeof(MinhasFuncoes)
                    .GetRuntimeMethod("Left", new[] { typeof(string), typeof(int) });
        protected void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));
                foreach (var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue)
                        property.SetColumnType("VARCHAR(100)");
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _writer.Dispose();
        }
    }
}