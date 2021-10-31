/*gerando script*/
dotnet ef migrations script -p .\Fundamentos.EFCore\Fundamentos.EFCore.csproj -o .\Fundamentos.EFCore\Data\Sql\Initial.sql

/*gerando script idempotente*/
dotnet ef migrations script -p .\Fundamentos.EFCore\Fundamentos.EFCore.csproj -o .\Fundamentos.EFCore\Data\Sql\Initial.sql -i

/*adicionando a migrations*/
dotnet ef migrations add Initial --p .\Fundamentos.EFCore\Fundamentos.EFCore

/*revertendo a migrations*/
dotnet ef migrations remove -p .\Fundamentos.EFCore\Fundamentos.EFCore.csproj

/*aplindo migrations*/
dotnet ef migrations update -p .\Fundamentos.EFCore\Fundamentos.EFCore.csproj -v

/*instalando pacotes nuget*/
 dotnet add .\Fundamentos.EFCore\Fundamentos.EFCore.csproj package nome_pacote_nuget --version 3.1.5




 
Para utilizar o Code First, execute as etapas abaixo:
RESUMO:
1 - Fa�a Build do projeto
2 - Abra o Package Manager Console
3 - Execute o comando 
	add-migration <identificacaoDaMudancaNoBanco>
4 - Crie um arquivo .sql na pasta Scripts com o mesmo nome da classe gerada no passo anterior.
	<dataHoraEidentificacaoDaMudancaNoBanco>.sql
5 - Execute o comando
	Update-Database -Script -SourceMigration <MigrationOrigem> -TargetMigration <MigrationDestino>
6 - Copie o script gerado e insira-o no arquivo gerado no passo 5.
7 - Quando a altera��o for publicada em produ��o, execute os scripts gerados, em ordem.
	Para checar qual foi o �ltimo script gerado, fa�a uma consulta na tabela __MigrationHistory

EXPLICA��ES:

---------------------------------------------------------------------------------
---------------------------------------------------------------------------------
---------------------------------------------------------------------------------

1 - Fa�a Build do projeto

---------------------------------------------------------------------------------
---------------------------------------------------------------------------------
---------------------------------------------------------------------------------

2 - Abra o Package Manager Console
Caminho: Tools/Nuget Package Manager/Package Manager Console

---------------------------------------------------------------------------------
---------------------------------------------------------------------------------
---------------------------------------------------------------------------------

3 - Execute o comando abaixo
add-migration <identificacao da mudanca no banco>

Exemplos:
add-migration inclusaoProfessores
add-migration alteracaoHorarios
add-migration criacaoViewAlunos

Esse comando produzir� uma classe dentro da pasta Migrations no seguinte padr�o:
<data e hora><identificacao da mudanca no banco>.cs

Essa classe registra todas as altera��es do banco de dados realizadas no dom�nio e determina a sequ�ncia de opera��es a serem executadas pelo Entity.
Caso necess�rio, apesar de incomum, � poss�vel alterar o comportamento de algumas instru��es nessa classe, personalizando algumas a��es e alterando o comportamento padr�o do Entity.

---------------------------------------------------------------------------------
---------------------------------------------------------------------------------
---------------------------------------------------------------------------------

4 - Para efeito de rastreio, crie um arquivo .sql na pasta Scripts com o mesmo nome da classe gerada no passo anterior.
<data e hora><identificacao da mudanca no banco>.sql

---------------------------------------------------------------------------------
---------------------------------------------------------------------------------
---------------------------------------------------------------------------------

5 - Identifique as seguintes classes da pasta Migrations:

<Classe_1> : A classe de Migrations anterior � altera��o que voc� quer aplicar (sem a extens�o.cs)
<Classe_2> : A classe de Migrations que voc� quer aplicar (no caso, a classe <data e hora><identificacao da mudanca no banco>.cs ) (sem a extens�o.cs)

Update-Database -Script -SourceMigration <Classe_1> -TargetMigration <Classe_2>

Exemplo:
Update-Database -Script -SourceMigration: 201906122112332_agendamentosBase -TargetMigration: 201907111440070_criterioavaliativo

Esse comando gerar� as instru��es SQL baseadas nas altera��es que voc� aplicou no dom�nio tendo como base a �ltima atualiza��o aplicada no banco de dados destino.
� importante usar o par�metro "-Script" para que o Entity n�o aplique as mudan�as diretamente no banco configurado no Web.config. Fazendo isso, conseguimos aplicar
as altera��es no banco de produ��o mesmo sem ter acesso direto ao servidor do mesmo.

---------------------------------------------------------------------------------
---------------------------------------------------------------------------------
---------------------------------------------------------------------------------

6 - Copie o script gerado e insira-o no arquivo gerado no passo 5.

---------------------------------------------------------------------------------
---------------------------------------------------------------------------------
---------------------------------------------------------------------------------

7 - Quando a altera��o for publicada em produ��o, execute os scripts gerados, em ordem.
Para checar qual foi o �ltimo script gerado, fa�a uma consulta na tabela __MigrationHistory

select * from __MigrationHistory order by 1 desc


