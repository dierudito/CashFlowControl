## CashFlowControl - README

### Descrição

O CashFlowControl é uma API RESTful desenvolvida em C# .NET 8 para auxiliar comerciantes no controle de seu fluxo de caixa diário. A API permite o registro de transações (débitos e créditos) e fornece um relatório consolidado diário do saldo.

### Pré-requisitos

* **SQL Server:** Você precisará de um servidor SQL Server em execução para armazenar os dados.
* **.NET 8 SDK:** Certifique-se de ter o .NET 8 SDK instalado em sua máquina. Você pode baixá-lo em: [https://dotnet.microsoft.com/pt-br/download/dotnet/8.0](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)

### Configuração

1. **Crie o banco de dados:**
   - Execute o script SQL localizado em `/scripts` no seu servidor SQL Server para criar a estrutura do banco de dados.

2. **Configure a string de conexão:**
   - Abra o arquivo `appsettings.json` localizado em `/src/DMoreno.CashFlowControl.bff`.
   - Substitua o valor da chave `ConnectionStrings:CashFlowControlDbSqlServer` pela string de conexão do seu banco de dados recém-criado.

### Executando a aplicação

1. **Abra um terminal:** Navegue até o diretório do projeto principal (`/src/DMoreno.CashFlowControl.bff`).

2. **Execute o comando:**
   ```bash
   dotnet run
   ```
   A aplicação iniciará e exibirá a URL em que está sendo executada (ex: `Now listening on: https://localhost:7201`).

### Acessando a API

Você tem duas opções para interagir com a API:

**Opção 1: Navegador**

1. Abra a URL da aplicação no seu navegador.
2. Acesse a documentação da API em: `/swagger/index.html`.
3. Utilize a interface do Swagger para testar os endpoints, seguindo as instruções da documentação.

**Opção 2: Postman/Insomnia**

1. Importe a collection do Postman localizada em `/collection/CashFlowControl.postman_collection.json`.
2. Atualize a URL base em todos os endpoints para a URL em que sua aplicação está sendo executada (a que você obteve na etapa "Executando a aplicação").
3. Utilize o Postman ou Insomnia para enviar requisições para os endpoints da API.

---

**Observações:**

* Este projeto segue as melhores práticas RESTful para o design de APIs.
* A documentação da API no Swagger fornece informações detalhadas sobre os endpoints, parâmetros e exemplos de uso.
* Sinta-se à vontade para explorar o código-fonte e os testes unitários para entender melhor a implementação.

**Contribuindo:**

Contribuições são bem-vindas! Se você encontrar algum problema ou tiver sugestões de melhoria, por favor, abra uma issue ou envie um pull request.

[![Netlify Status](https://api.netlify.com/api/v1/badges/5ec8b167-fcdf-43b0-b48f-714c02bf3424/deploy-status)](https://app.netlify.com/sites/morenocashflowcontrol/deploys)

![Sonar Test Success Rate (branch)](https://img.shields.io/sonar/test_success_density/swellaby%3Aletra/master?server=https%3A%2F%2Fsonarcloud.io)