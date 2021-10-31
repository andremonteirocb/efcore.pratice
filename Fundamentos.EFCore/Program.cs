using System;
using Fundamentos.EFCore.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Aplicacao;
using System.Transactions;
using Fundamentos.EFCore.Functions;

namespace Fundamentos.EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ApplicationContext())
            {
                CriarMenu(db);
            }
        }

        private static void CriarMenu(ApplicationContext db)
        {
            Console.WriteLine("Escolhaa opção que deseja:");
            Console.WriteLine("1 - Inserir dados");
            Console.WriteLine("2 - Inserir dados em massa");
            Console.WriteLine("3 - Consultar dados");
            Console.WriteLine("4 - Consultar pedido");
            Console.WriteLine("5 - Consultar pedido carregando lazy loading");
            Console.WriteLine("6 - Atualizar dados");
            Console.WriteLine("7 - Remover Registro");
            Console.WriteLine("8 - Melhorar perfomance");
            Console.WriteLine("9 - Consultar conexão com banco");
            Console.WriteLine("10 - Criando/Deletando banco");
            Console.WriteLine("11 - Todas as Migrações");
            Console.WriteLine("12 - Migrações Pendentes");
            Console.WriteLine("13 - Migrações Aplicadas");
            Console.WriteLine("14 - Aplicar Migrações");
            Console.WriteLine("15 - Obter script do banco");
            Console.WriteLine("16 - Rodando Scripts Manualmente");
            Console.WriteLine("17 - Sql Injection");
            Console.WriteLine("18 - Utilizando Filtros");
            Console.WriteLine("19 - Utilizando Consultas");
            Console.WriteLine("20 - Utilizando Procedures");
            Console.WriteLine("21 - Habilitando Batch Size (Insert's em massa)");
            Console.WriteLine("22 - Utilizando Estratégia (Transação)");            
            Console.WriteLine("23 - Utilizando EF Functions com TransactionScope");
            Console.WriteLine("24 - Utilizando Interceptadores");
            Console.WriteLine("25 - Utilizando DbFunctions");
            var comando = Console.ReadLine();

            switch (comando)
            {
                case "1":
                    InserirDados(db);
                    break;

                case "2":
                    InserirDadosEmMassa(db);
                    break;

                case "3":
                    ConsultarDados(db);
                    break;

                case "4":
                    CadastrarPedido(db);
                    break;

                case "5":
                    ConsultarPedidoCarregamentos(db);
                    break;

                case "6":
                    AtualizarDados(db);
                    break;

                case "7":
                    RemoverRegistro(db);
                    break;

                case "8":
                    _count = 0;
                    GerenciarEstadoDaConexao(db, false);
                    _count = 0;
                    GerenciarEstadoDaConexao(db, true);
                    break;

                case "9":
                    HealthCheckBancoDeDados(db);
                    break;

                case "10":
                    EnsureCreatedAndDeleted(db);
                    break;

                case "11":
                    TodasMigracoes(db);
                    break;

                case "12":
                    MigracoesPendentes(db);
                    break;

                case "13":
                    MigracoesJaAplicadas(db);
                    break;

                case "14":
                    AplicarMigracaoEmTempodeExecucao(db);
                    break;

                case "15":
                    ScriptGeralDoBancoDeDados(db);
                    break;

                case "16":
                    ExecuteSQL(db);
                    break;

                case "17":
                    SqlInjection(db);
                    break;

                case "18":
                    ExecuteFilters(db);
                    break;

                case "19":
                    ExcuteConsultas(db);
                    break;

                case "20":
                    ExecuteProcedures(db);
                    break;

                case "21":
                    ExecuteBatchSize(db);
                    break;

                case "22":
                    ExecutarEstrategiaResiliencia(db);
                    break;

                case "23":
                    ExecutarEFFunctionsWithTransactionScope(db);
                    break;

                case "24":
                    ExecutarInterceptadores(db);
                    break;

                case "25":
                    ExecutarDbFunctions(db);
                    break;
            }

            CriarMenu(db);
        }

        private static void ExecutarDbFunctions(ApplicationContext db)
        {
            var pedido = db.Clientes
                .AsNoTrackingWithIdentityResolution()
                .Select(c => MinhasFuncoes.Left(c.Nome, 10))
                .FirstOrDefault();
        }

        private static void ExecutarInterceptadores(ApplicationContext db)
        {
            var pedido = db.Pedidos
                           .TagWith("NOLOCK")
                           .AsNoTrackingWithIdentityResolution()
                           .FirstOrDefault();

            Console.WriteLine(pedido.ClienteId);
        }

        private static void ExecuteBatchSize(ApplicationContext db)
        {
            TempoComandoGeral(db);
            HabilitandoBatchSize(db);
        }

        private static void ExecuteProcedures(ApplicationContext db)
        {
            CriarStoredProcedure(db);
            InserirDadosViaProcedure(db);
            CriarStoredProcedureDeConsulta(db);
            ConsultaViaProcedure(db);
        }

        private static void ExcuteConsultas(ApplicationContext db)
        {
            ConsultaProjetada(db);
            ConsultaParametrizada(db);
            ConsultaInterpolada(db);
            ConsultaComTAG(db);
        }

        private static void ExecuteFilters(ApplicationContext db)
        {
            IgnoreFiltroGlobal(db);
            FiltroGlobal(db);
        }

        private static void RemoverRegistro(ApplicationContext db)
        {
            var appCliente = new ClienteAplicacao();
            appCliente.Excluir(new Cliente { Id = 3 });
        }

        private static void AtualizarDados(ApplicationContext db)
        {
            var cliente = db.Clientes.Find(1);
            //cliente.Nome = $"{new Random().Next(1, 100)} Cliente Desconectado Passo 3";
            
            var clienteDesconectado = new 
            {
                Nome = $"{new Random().Next(1, 100)} Cliente Desconectado Passo 3"
            };

            //atualiza apenas as propriedades alteradas
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);
            
            //rastreamento de updates
            //db.Entry(cliente).State = EntityState.Modified;

            //atualiza todas as colunas novamente
            //db.Clientes.Update(cliente);

            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentos(ApplicationContext db)
        {
            CarregamentoAdiantado(db);
            CarregamentoExplicito(db);

            //configurando lazyloading proxies para um determinado método
            //db.ChangeTracker.LazyLoadingEnabled = false;
            //adiciona .UseLazyLoadingProxies() no optionsbuilder
            //adiciona virtual nas propriedades das entidades
        }

        private static void CadastrarPedido(ApplicationContext db)
        {
            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                 {
                     new PedidoItem
                     {
                         ProdutoId = produto.Id,
                         Desconto = 0,
                         Quantidade = 1,
                         Valor = 10,
                     }
                 }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }

        private static void ConsultarDados(ApplicationContext db)
        {
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var consultaPorMetodo = db.Clientes
                .AsNoTracking()
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Id)
                .ToList();

            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");
                //db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void InserirDadosEmMassa(ApplicationContext db)
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Rafael Almeida",
                CEP = "99999000",
                Cidade = "Itabaiana",
                Estado = "SE",
                Telefone = "99000001111",
            };

            var listaClientes = new[]
            {
                new Cliente
                {
                    Nome = "Teste 1",
                    CEP = "99999000",
                    Cidade = "Itabaiana",
                    Estado = "SE",
                    Telefone = "99000001115",
                },
                new Cliente
                {
                    Nome = "Teste 2",
                    CEP = "99999000",
                    Cidade = "Itabaiana",
                    Estado = "SE",
                    Telefone = "99000001116",
                },
            };

            //db.AddRange(produto, cliente);
            db.Set<Cliente>().AddRange(listaClientes);
            //db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void InserirDados(ApplicationContext db)
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            db.Produtos.Add(produto);
            db.Set<Produto>().Add(produto);
            db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        static void ExecutarEstrategiaResiliencia(ApplicationContext db)
        {
            var strategy = db.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = db.Database.BeginTransaction();

                try
                {
                    db.Departamentos.Add(new Departamento { Descricao = "Departamento Transacao" });

                    //transaction.CreateSavepoint("Departamento01");

                    db.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    //transaction.RollbackToSavepoint("Departamento01");
                    transaction.Rollback();
                }
            });
        }

        static void ExecutarEFFunctionsWithTransactionScope(ApplicationContext db)
        {
            var transactionOptions = new TransactionOptions
            {
                Timeout = TimeSpan.FromSeconds(1000),
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                var pedidos = db.Pedidos
                    .AsNoTrackingWithIdentityResolution()
                    .Select(p => new 
                    { 
                        Dias = EF.Functions.DateDiffDay(p.IniciadoEm, p.FinalizadoEm),
                        Data = EF.Functions.DateFromParts(DateTime.Now.Year, DateTime.Now.Month, 01),
                        DataValida = EF.Functions.IsDate(p.IniciadoEm.ToString("yyyy-MM-dd")),
                        TotalBytes = EF.Functions.DataLength(p.IniciadoEm)
                    })
                    .ToList();

                var clientes = db.Clientes
                    .AsNoTrackingWithIdentityResolution()
                    .Where(c => EF.Functions.Like(c.Nome, "%B[ao]%"))
                    .ToList();

                transaction.Complete();
            }
        }

        static void TempoComandoGeral(ApplicationContext db)
        {
            db.Database.SetCommandTimeout(10);
            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07';SELECT 1");
        }

        static void HabilitandoBatchSize(ApplicationContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            for (var i = 0; i < 50; i++)
            {
                db.Departamentos.Add(
                    new Departamento
                    {
                        Descricao = "Departamento " + i
                    });
            }

            db.SaveChanges();
        }

        static void ConsultaViaProcedure(ApplicationContext db)
        {
            var dep = new SqlParameter("@dep", "Departamento");

            var departamentos = db.Departamentos
                .FromSqlRaw("EXECUTE GetDepartamentos @dep", dep)
                //.FromSqlInterpolated($"EXECUTE GetDepartamentos {dep}")
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine(departamento.Descricao);
            }
        }

        static void CriarStoredProcedureDeConsulta(ApplicationContext db)
        {
            var criarDepartamento = @"
            CREATE OR ALTER PROCEDURE GetDepartamentos
                @Descricao VARCHAR(50)
            AS
            BEGIN
                SELECT * FROM Departamentos Where Descricao Like @Descricao + '%'
            END        
            ";

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }


        static void InserirDadosViaProcedure(ApplicationContext db)
        {
            db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", "Departamento Via Procedure", true);
        }

        static void CriarStoredProcedure(ApplicationContext db)
        {
            var criarDepartamento = @"
            CREATE OR ALTER PROCEDURE CriarDepartamento
                @Descricao VARCHAR(50),
                @Ativo bit
            AS
            BEGIN
                INSERT INTO 
                    Departamentos(Descricao, Ativo, Excluido) 
                VALUES (@Descricao, @Ativo, 0)
            END        
            ";

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }

        static void DivisaoDeConsulta()
        {
            using var db = new ApplicationContext();
            Setup(db);

            //caso esteja ligado o splitquerie global, utilize singlequery para ignora-lo
            //.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            var departamentos = db.Departamentos
                .Include(p => p.Funcionarios)
                .Where(p => p.Id < 3)
                //.AsSplitQuery()
                .AsSingleQuery()
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\tNome: {funcionario.Nome}");
                }
            }
        }

        static void EntendendoConsulta1NN1()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var funcionarios = db.Funcionarios
                .Include(p => p.Departamento)
                .ToList();

            foreach (var funcionario in funcionarios)
            {
                Console.WriteLine($"Nome: {funcionario.Nome} / Descricap Dep: {funcionario.Departamento.Descricao}");
            }

            /*var departamentos = db.Departamentos
                .Include(p=>p.Funcionarios)
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\tNome: {funcionario.Nome}");
                }
            }*/
        }

        static void ConsultaComTAG(ApplicationContext db)
        {
            Setup(db);

            var departamentos = db.Departamentos
                .TagWith(@"Estou enviando um comentario para o servidor                
                           Segundo comentario
                           Terceiro comentario")
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        static void ConsultaInterpolada(ApplicationContext db)
        {
            Setup(db);

            var id = 1;
            var departamentos = db.Departamentos
                .FromSqlInterpolated($"SELECT * FROM Departamentos WHERE Id>{id}")
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        static void ConsultaParametrizada(ApplicationContext db)
        {
            Setup(db);

            var id = new SqlParameter
            {
                Value = 1,
                SqlDbType = System.Data.SqlDbType.Int
            };
            var departamentos = db.Departamentos
                .FromSqlRaw("SELECT * FROM Departamentos WHERE Id>{0}", id)
                .Where(p => !p.Excluido)
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        static void ConsultaProjetada(ApplicationContext db)
        {
            Setup(db);

            var departamentos = db.Departamentos
                .Where(p => p.Id > 0)
                .Select(p => new { p.Descricao, Funcionarios = p.Funcionarios.Select(f => f.Nome) })
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\t Nome: {funcionario}");
                }
            }
        }

        static void IgnoreFiltroGlobal(ApplicationContext db)
        {
            Setup(db);

            var departamentos = db.Departamentos.IgnoreQueryFilters().Where(p => p.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }

        static void FiltroGlobal(ApplicationContext db)
        {
            Setup(db);

            var departamentos = db.Departamentos.Where(p => p.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }

        static void Setup(ApplicationContext db)
        {
            if (db.Database.EnsureCreated())
            {
                db.Departamentos.AddRange(
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 01",
                        Funcionarios = new System.Collections.Generic.List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Rafael Almeida",
                                CPF = "99999999911",
                                RG= "2100062"
                            }
                        },
                        Excluido = true
                    },
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 02",
                        Funcionarios = new System.Collections.Generic.List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Bruno Brito",
                                CPF = "88888888811",
                                RG= "3100062"
                            },
                            new Funcionario
                            {
                                Nome = "Eduardo Pires",
                                CPF = "77777777711",
                                RG= "1100062"
                            }
                        }
                    });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }

        static void CarregamentoAdiantado(ApplicationContext db)
        {
            //adiantado
            var pedidos = db
                .Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);

            //adiantado
            var departamentos = db
                .Departamentos
                  .Include(d => d.Funcionarios)
                    .AsNoTrackingWithIdentityResolution()
                .ToList();

            Console.WriteLine(departamentos.Count);
        }

        static void CarregamentoExplicito(ApplicationContext db)
        {
            SetupTiposCarregamentos(db);

            var departamentos = db
                .Departamentos
                .ToList();

            foreach (var departamento in departamentos)
            {
                if (departamento.Id == 4)
                {
                    //db.Entry(departamento).Collection(p=>p.Funcionarios).Load();
                    db.Entry(departamento).Collection(p => p.Funcionarios).Query().Where(p => p.Id > 2).ToList();
                }

                Console.WriteLine("---------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionario encontrado!");
                }
            }
        }

        static void SetupTiposCarregamentos(ApplicationContext db)
        {
            if (!db.Departamentos.Any())
            {
                db.Departamentos.AddRange(
                    new Departamento
                    {
                        Descricao = "Departamento 01",
                        Funcionarios = new System.Collections.Generic.List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Rafael Almeida",
                                CPF = "99999999911",
                                RG= "2100062"
                            }
                        }
                    },
                    new Departamento
                    {
                        Descricao = "Departamento 02",
                        Funcionarios = new System.Collections.Generic.List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Bruno Brito",
                                CPF = "88888888811",
                                RG= "3100062"
                            },
                            new Funcionario
                            {
                                Nome = "Eduardo Pires",
                                CPF = "77777777711",
                                RG= "1100062"
                            }
                        }
                    });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }

        static void ScriptGeralDoBancoDeDados(ApplicationContext db)
        {
            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        static void MigracoesJaAplicadas(ApplicationContext db)
        {
            var migracoes = db.Database.GetAppliedMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }

        static void TodasMigracoes(ApplicationContext db)
        {
            var migracoes = db.Database.GetMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }

        static void AplicarMigracaoEmTempodeExecucao(ApplicationContext db)
        {
            db.Database.Migrate();
        }

        static void MigracoesPendentes(ApplicationContext db)
        {
            var migracoesPendentes = db.Database.GetPendingMigrations();

            Console.WriteLine($"Total: {migracoesPendentes.Count()}");

            foreach (var migracao in migracoesPendentes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }

        static void SqlInjection(ApplicationContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.AddRange(
                new Departamento
                {
                    Descricao = "Departamento 01"
                },
                new Departamento
                {
                    Descricao = "Departamento 02"
                });
            db.SaveChanges();

            //var descricao = "Teste ' or 1='1";
            //db.Database.ExecuteSqlRaw("update departamentos set descricao='AtaqueSqlInjection' where descricao={0}",descricao);
            //db.Database.ExecuteSqlRaw($"update departamentos set descricao='AtaqueSqlInjection' where descricao='{descricao}'");
            foreach (var departamento in db.Departamentos.AsNoTracking())
            {
                Console.WriteLine($"Id: {departamento.Id}, Descricao: {departamento.Descricao}");
            }
        }

        static void ExecuteSQL(ApplicationContext db)
        {
            // Primeira Opcao
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                cmd.ExecuteNonQuery();
            }

            // Segunda Opcao
            var descricao = "TESTE";
            db.Database.ExecuteSqlRaw("update departamentos set descricao={0} where id=1", descricao);

            //Terceira Opcao
            db.Database.ExecuteSqlInterpolated($"update departamentos set descricao={descricao} where id=1");
        }

        static int _count;
        static void GerenciarEstadoDaConexao(ApplicationContext db, bool gerenciarEstadoConexao)
        {
            var time = System.Diagnostics.Stopwatch.StartNew();

            var conexao = db.Database.GetDbConnection();

            conexao.StateChange += (_, __) => ++_count;

            if (gerenciarEstadoConexao)
            {
                conexao.Open();
            }

            for (var i = 0; i < 200; i++)
            {
                db.Departamentos.AsNoTracking().Any();
            }

            time.Stop();
            var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, Contador: {_count}";

            Console.WriteLine(mensagem);
        }

        static void HealthCheckBancoDeDados(ApplicationContext db)
        {
            var canConnect = db.Database.CanConnect();
            //try { db.Database.GetDbConnection(); } catch { }

            if (canConnect)
            {
                Console.WriteLine("Posso me conectar");
            }
            else
            {
                Console.WriteLine("Não posso me conectar");
            }
        }

        static void EnsureCreatedAndDeleted(ApplicationContext db)
        {
            //db.Database.EnsureCreated(); //cria o banco de dados
            db.Database.EnsureDeleted(); //apaga o banco de dados
        }
    }
}
