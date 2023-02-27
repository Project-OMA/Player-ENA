# Serviços

Serviços são classes que provem funcionalidades ao Player ENA para a comunicação com elementos externos ao contexto da aplicação. Serviços são implementados usando a interface `IService` e usados principalmente por Coordenadores na interface gráfica.

# Implementados

## Autenticação

A classe `AuthService` contém um conjunto de funções para lidar com a autenticação dentro da aplicação:

- Verificar se o usuário está logado
- Validar as credenciais para acessar sua conta
- Acesso a conta atualmente ativa
- Sair da conta

Para isso, a classe provê uma interface interna chamada `CredentialManager`, que deve ser implementada pelo componente responsável por se comunicar com a validação das credenciais, seja por:

- Nome de usuário e senha
- Token de acesso

## Mapas

O serviço `MapService` é responsável por gerenciar os mapas dentro da aplicação, sendo possível carregar os dados de mapas salvos localmente ou em uma fonte de dados especificada. Ao carregar os dados de uma fonte de dados externa, o serviço salva os resultados localmente para facilitar o acesso quando o usuário não estiver conectado a Internet. O resultado final exibido ao usuário é uma combinação dos dados encontrados no cache local e na fonte de dados externas. Para implementar uma nova fonte de dados, é preciso usar a interface interna chamada `DataSource`.

## Cache Local

Através das classes `DataPath` e `LocalCache`, a aplicação determina onde serão salvos localmente os arquivos de conteúdo da aplicação. São considerados arquivos de conteúdo os mapas de atividade e os artefatos de métricas gerados pela aplicação.

## Serviço Web

<aside>
⚠️ A API da aplicação funciona apenas como um mock de informações de mapas

</aside>

`ENAWebService` é o serviço principal de comunicação com a API da aplicação, implementando as interfaces internas dos serviços de autenticação e mapas para prover a validação de credenciais (futuramente) e fonte de mapas. A classe utiliza a estrutura `WebService`, que serve como uma URL pré-definida da localização da API, permitindo que se possa fazer chamadas sem acessar outras URLs.

Além dos serviços providos, o serviço Web é responsável por fazer o Download e, futuramente, o Upload dos arquivos necessários para o funcionamento da aplicação.

## Integração com Micelio

`MicelioWebService` provê uma fachada para o Micelio, facilitando a criação e encerramento de sessões, assim como o registro de dados com a MicelioAPI. É usado pelas classes que fazem o registro das métricas para sincronizar a escrita local com a MicelioAPI.

Além de simplificar o uso da API, possui 2 configurações que permitem ativar ou desativar o modo de desenvolvedor e o envio de requisições, facilitando a testagem.