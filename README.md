# Chu S.A. Api

## Sobre

Projeto tem como objetivo a criacao de uma API para uma empresa ficcional relacionada a financas.

Foram solicitados a disponibilizacao de 3 endpoints para chamada:

 - Cadastro de contas
 - Realizacao de transferencias ([apenas em dias uteis](https://brasilapi.com.br/api/feriados/v1/2024))
 - Gerar extrato por periodo

Como requisitos para a conclusao foram exigidas as tecnologias:

 - ASP.NET Core 6
 - Testes unitários
 - Containerizacao (Docker)
 - Cache
 - Git
 - Manual para executar o projeto no README.md

Possuindo a incersao de algumas tecnologias como bonus para a criacao, como:

 - Versionamento de API
 - Interação com banco dados (Qualquer DB)
 - Implementar Autenticação e autorização
 - Utilização swagger
 - Utilização Fluent Validation

## Documentacao

### ASP .NET 6

Foram instalados a partir do gerenciador de pacotes oficial do Arch Linux
> sudo pacman -Sy aspnet-runtime-6.0 dotnet-sdk-6.0

(tambem podendo ser instalado no [site da microsoft](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0))

### Banco de Dados

Foi utilizado MySql para a criacao do projeto por conta de melhor familizariazao com o mesmo, 
tendo sido instalado por docker a partir do comando:
> docker run --ulimit nofile=1048576 -e MYSQL_ROOT_PASSWORD=**<SUA-SENHA-AQUI>** -p 3306:3306 --name **<NOME-DO-CONTAINER>** -d mysql:8.3.0

Os arquivos com a modelagem DER do banco e script de importacao se encontram em: **Resources/db/<arquivos\>**, caso deseje rodar o script, o banco pode ser acessado
pelo mysql-workbench (que se encontra no [site oficial do projeto](https://dev.mysql.com/downloads/workbench/) ou no [repositorio oficial do Arch Linux](https://archlinux.org/packages/extra/x86_64/mysql-workbench/)) ou pode ser acessar pelo terminal (nesse caso sendo necessario [copiar o script para dentro do container](https://docs.docker.com/engine/reference/commandline/container_cp/) e rodar internamente, ou pelo comando do proprio docker)

> mysql -u<USUARIO-AQUI> -p < caminho1/caminho2/meuScript.sql

### Integracao Projeto + Banco de Dados

Caso precise reconfigurar a conexao com banco, pode ser feito a partir do arquivo _appsettings.json_, no registro com nome _AppDbConnectionString_.


