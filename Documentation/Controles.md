# Controles

Esta seção contém uma lista dos controles para usar a aplicação.

# Controles

## ℹ️ **Legenda**

🎙️ → Leitor de Tela Habilitado
🔺 → Giroscópio Habilitado


## Gameplay

| Ação | Comando(s) | Notas |
| --- | --- | --- |
| Andar | ⬆️ → Frente
⬇️ → Trás | Posição é limitada pela grade de jogo, podendo mover-se 1 unidade por vez. |
| Girar | s/ 🔺 → ⬅️ Esquerda/➡️ Direita; c/ 🔺 → Girar Celular | Rotação é limitada dependendo da grade. Apesar do giroscópio permitir rotação livre da câmera, a movimentação é limitada aos 4 pontos cardeais. |
| Ativar/Desativar Acessibilidade | Escape (Computador)/◀️ (Mobile) | Ativa/desativa o Leitor de Tela |

## Menu

| Ação | Comando(s) |
| --- | --- |
| Navegar | s/ 🎙️ → Touch/Mouse; c/ 🎙️ → Setas do Teclado/Gesto de Swipe |
| Interagir | s/ 🎙️ → Toque/clique único; c/ 🎙️ → Toque/clique duplo |

# Implementação

## Monitor do Analógico (`AxisTracker`)

Componente responsável por verificar a mudança de estado do analógico para identificar quando o usuário aperta em uma das direções. Usado para garantir que o joystick saiu de uma posição neutra para uma direção especificada. Possui 2 métodos: um para lidar com o movimento horizontal (`HorizontalDown`) e outro para o movimento vertical (`VerticalDown`).

## Camera do Giroscópio (`GyroCamera`)

Componente responsável por controlar a câmera do usuário com o giroscópio quando o mesmo está habilitado e o dispositivo suporta. Possui um sistema de calibração interno ao ser inicializado para garantir consistência da camera.

## Controlador do Personagem (`PlayerController`)

Componente que funciona como mediador entre os controles de jogabilidade e os sistemas de movimentação, rotação, colisão e feedback sonoro.