IF OBJECT_ID(N'[curso_ef_core]') IS NULL
BEGIN
    CREATE TABLE [curso_ef_core] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK_curso_ef_core] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'schema_sequencia') IS NULL EXEC(N'CREATE SCHEMA [schema_sequencia];');
GO

CREATE SEQUENCE [schema_sequencia].[nome_da_sequencia] AS int START WITH 1 INCREMENT BY 2 MINVALUE 1 MAXVALUE 10 CYCLE;
GO

CREATE TABLE [Clientes] (
    [Id] int NOT NULL IDENTITY,
    [Nome] VARCHAR(80) NOT NULL,
    [Phone] CHAR(11) NULL,
    [CEP] CHAR(8) NOT NULL,
    [Estado] CHAR(2) NOT NULL,
    [Cidade] nvarchar(60) NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Departamentos] (
    [Id] int NOT NULL DEFAULT (NEXT VALUE FOR schema_sequencia.nome_da_sequencia),
    [Descricao] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CS_AI NULL,
    [Ativo] bit NOT NULL,
    [Excluido] bit NOT NULL,
    CONSTRAINT [PK_Departamentos] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Produtos] (
    [Id] int NOT NULL IDENTITY,
    [CodigoBarras] VARCHAR(14) NOT NULL,
    [Descricao] VARCHAR(60) NULL,
    [Valor] decimal(18,2) NOT NULL,
    [TipoProduto] nvarchar(max) NOT NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Pedidos] (
    [Id] int NOT NULL IDENTITY,
    [ClienteId] int NOT NULL,
    [IniciadoEm] datetime2 NOT NULL DEFAULT (GETDATE()),
    [FinalizadoEm] datetime2 NOT NULL,
    [TipoFrete] int NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Observacao] VARCHAR(512) NULL,
    CONSTRAINT [PK_Pedidos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Pedidos_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Funcionarios] (
    [Id] int NOT NULL IDENTITY,
    [Nome] VARCHAR(100) NULL,
    [CPF] VARCHAR(100) NULL,
    [RG] VARCHAR(100) NULL,
    [Excluido] bit NOT NULL,
    [DepartamentoId] int NOT NULL,
    CONSTRAINT [PK_Funcionarios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Funcionarios_Departamentos_DepartamentoId] FOREIGN KEY ([DepartamentoId]) REFERENCES [Departamentos] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PedidoItens] (
    [Id] int NOT NULL IDENTITY,
    [PedidoId] int NOT NULL,
    [ProdutoId] int NOT NULL,
    [Quantidade] int NOT NULL DEFAULT 0,
    [Valor] decimal(18,2) NOT NULL DEFAULT 0.0,
    [Desconto] decimal(18,2) NOT NULL DEFAULT 0.0,
    CONSTRAINT [PK_PedidoItens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PedidoItens_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PedidoItens_Produtos_ProdutoId] FOREIGN KEY ([ProdutoId]) REFERENCES [Produtos] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [idx_cliente_telefone] ON [Clientes] ([Phone]);
GO

CREATE UNIQUE INDEX [IX_Departamentos_Descricao] ON [Departamentos] ([Descricao]) WHERE [Descricao] IS NOT NULL;
GO

CREATE INDEX [IX_Funcionarios_DepartamentoId] ON [Funcionarios] ([DepartamentoId]);
GO

CREATE INDEX [IX_PedidoItens_PedidoId] ON [PedidoItens] ([PedidoId]);
GO

CREATE INDEX [IX_PedidoItens_ProdutoId] ON [PedidoItens] ([ProdutoId]);
GO

CREATE INDEX [IX_Pedidos_ClienteId] ON [Pedidos] ([ClienteId]);
GO

INSERT INTO [curso_ef_core] ([MigrationId], [ProductVersion])
VALUES (N'20210802225248_Initial', N'5.0.1');
GO

COMMIT;
GO

