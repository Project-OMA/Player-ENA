# Persistência

## Configurações

As configurações do jogo são salvas localmente nas preferências do usuário através do objeto `SettingsProvider`. O sistema possui as seguintes configurações:

- **Elementos Desaparecem:** Quando habilitado, o objetivo desaparece após ser completado.
- **Giroscópio:** Quando habilitado, a camera é controlada usando o giroscópio do dispositivo.
- **Mini Mapa:** Quando habilitado, exibe um mini-mapa mostrando o layout da fase e a trajetória do jogador.
- **Óculus de Realidade Virtual:** Quando habilitado, ajusta a visão para uso da aplicação móvel através de um HMD.
- **Vibração:** Quando habilitado, aplica feedback háptico.

`OptionsPlayer` ajusta os elementos da jogabilidade de acordo com as configurações de jogo.