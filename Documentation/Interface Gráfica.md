# Interface Gráfica

Para a implementação no Player ENA, a interface gráfica foi construída em Unity UI usando a arquitetura MVC com Coordenadores, separando a responsabilidade da navegação e do estado da janela atual.

# Componentes Customizados

## Coordenadores

Coordenadores são responsáveis por lidar com o fluxo de navegação entre as telas. Ao iniciar a cena, o Gerenciador de UI fica responsável por carregar o coordenador raiz, que fica responsável por carregar todos os outros coordinators da cena, e cada coordenador fica responsável por um aspecto da navegação do aplicativo.

O Player ENA possui os seguintes coordinators implementados:

| Nome | Responsabilidade |
| --- | --- |
| ENARootCoordinator | Configurar todos os outros coordenadores do Menu Principal |
| AuthCoordinator | Lidar com a autenticação do usuário |
| GameplayCoordinator | Gerencia a interface durante a realização de uma atividade |
| MapDataCoordinator | Gerenciar o carregamento de mapas |
| SettingsCoordinator | Gerenciar as configurações de jogo |

## Gerenciador

Componente responsável para gerenciar o fluxo da interface gráfica dentro da aplicação, controlando a pilha de paineis da interface e mantém o registro de serviços usados por Coordenadores para prover funcionalidade a aplicação.

## Painel

Componente usado para designar uma janela da interface gráfica. Para fazer com que um grupo de elementos de interface funcione como uma janela, é preciso adicionar o componente UIPanel para permitir o monitoramento do painel dentro da pilha do gerenciador.

| Painel | Descrição |
| --- | --- |
| AuthDisplay | Recebe as credenciais necessárias para a autenticação |
| MainMenuDisplay | Painel do Menu Principal da aplicação, onde são exibidos o usuário logado e os mapas disponíveis  |
| MapDataDisplay | Página que aparece ao selecionar um mapa, mostrando informações simples do mapa e as ações que podem ser feitas com o mesmo |
| MapInfoDisplay | Painel que serve como modelo para as thumbnails de mapa a serem exibidas no menu principal |
| PauseMenuDisplay | Painel responsável por lidar com o menu de pausa durante a jogabilidade. |
| TaskSettingsDisplay | Página de configurações, onde é possível alterar elementos do jogo. |
| TrackerDisplay | Painel responsável por gerenciar o minimapa a ser salvo ao final da atividade |

## UI List

Componente que permite a criação de coleções de elementos dentro da cena a partir de um prefab especificado.