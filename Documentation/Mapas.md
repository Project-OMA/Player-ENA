# Mapas

Está seção contém informações sobre a função dos componentes relacionados aos mapas de jogo.

# Sobre o Mapa

Todo mapa construído pelo Editor Web é exportado para o ENA em um formato XML contendo o leiaute do ambiente virtual a ser interpretado pelo componente `MapCreator`, responsável por construir o mapa usando um objeto `PropTheme` para criar e posicionar os prefabs nos locais indicados pelo leiaute. Um mapa é divido em 8 categorias:

- Floor: Camada relacionada ao piso do jogo. Por padrão, todo mapa possui um piso que cobre toda a sua extensão, mas o uso de elementos especiais permite identificar em qual tipo de chão está pisando.
- Wall: Camada relacionada as paredes de jogo. Por padrão, os mapas possuem também uma parede invisível para evitar a saída do jogador para fora do mapa.
- DoorWindow: Camada onde são posicionadas as portas e janelas.
- Furniture: Camada relacionada à mobilia do ambiente.
- Electronics: Camada relacionada aos eletrônicos presentes no ambiente.
- Utensils: Camada relacionada a outros elementos de mobília.
- Interactive: Camada que define onde os objetivos são posicionados.
- CharacterElements: Camada para identificar a posição inicial do jogador.
- Ceiling: Apesar de não existir no mapa em si, a camada é baseada nas informações da Camada Floor para criar o teto de jogo. Assim, segue as mesmas características da camada na qual se baseia.

# Implementação

## Categoria de Mapa (`MapCategory`)

Enumerador que possui todas as camadas listadas para uso no código, incluindo a camada Ceiling.

## Dados do Mapa (`MapData`)

Estrutura de dados usada para definir um mapa de jogo, contendo o caminho para os arquivos do mapa e seu respectivo identificador.

## Controle do Minimapa (`ControleMiniMap`)

Componente utilitário usado para sincronizar a configuração do mini mapa com o elemento de UI que mostra o mesmo na tela.