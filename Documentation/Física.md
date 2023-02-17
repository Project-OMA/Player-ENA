# Física

Esta seção possui informações sobre os componentes de física implementados para a aplicação.

# Collidable Prop

Componente que identifica um prop de cenário dentro do ambiente. É possível definir o nome do objeto e uma fonte de áudio para a colisão.

# Collision Tracker

Componente adicionado ao jogador para monitorar colisões com outros objetos, assim como disparar eventos:

- Ao tocar em novo piso (`OnHitFloor`)
- Ao tocar em um obstáculo que não seja o objetivo (`OnHitObstacle`)
- Ao tocar no objetivo (`OnHitObjective`)

# Movement Tracker

Componente responsável por gerenciar o movimento do jogador para frente e para trás dentro da grade de jogo, sendo possível alterar sua velocidade de navegação. Dispara eventos:

- Ao iniciar um movimento para uma nova posição (`OnBeginWalking`)
- Ao andar com sucesso para a nova posição (`OnEndWalking`)
- Quando a ação de movimento é cancelada durante sua execução (`OnCancelWalking`)

# Rotation Tracker

Componente responsável por gerenciar a rotação do jogador para esquerda e direita, sendo possível alterar a velocidade de rotação. Dispara um evento quando realiza uma rotação do personagem (`OnTurn`)