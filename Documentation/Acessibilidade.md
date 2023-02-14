# Acessibilidade

Nesta seção temos um resumo sobre as funcionalidades de acessibilidade do ENA, providas pelo pacote Unity Accessibility Package (UAP) by MetalPop Games. Para mais detalhes que vão além da documentação a seguir, a documentação oficial para o pacote UAP se encontra [aqui](http://www.metalpopgames.com/assetstore/accessibility/doc/index.html). Também é possível ver casos de uso disponibilizados pelo pacote na pasta "Assets → UAP → Examples”.

## Leitor de Tela

<aside>
ℹ️ O conteúdo da seção é apenas um guia rápido sobre o leitor de tela do pacote UAP. Para mais detalhes, ver a documentação oficial.

</aside>

O plugin funciona anexando um componente de acessibilidade ao elemento da interface gráfica, permitindo sua identificação pelo Accessibility Manager. Usando gestos de swipe ou as setas do teclado, é possível navegar entre os componentes visíveis da interface, mesmo que estejam desabilitados. Ao passar por cima de um elemento usando o leitor de tela, o sistema de texto para voz fala, nessa ordem de importância: nome, valor, tipo de componente e uma dica. Por padrão, o pacote suporta a leituras de *labels*, botões, *sliders*, *toggles*, *dropdowns* e campos de texto. A configuração do componente para o leitor de tela pode acontecer de forma automática ao adicionar o componente a um objeto, mas também pode ser configurado manualmente.

## Texto para Voz

O componente `SpeakerComponent` provê uma fachada da funcionalidade fornecida pelo pacote UAP para a aplicação ENA, permitindo de qualquer ponto da aplicação usar o método estático `SpeakerComponent.Speak` para falar uma mensagem de texto. `SpeakerComponent` também age como um MonoBehaviour comum, com métodos para integrar um conjunto de mensagens pré-definidas da aplicação via UnityEvents, listados a seguir:

| Tipo de Mensagem | Método | Notas |
| --- | --- | --- |
| Atividade finalizada | `SpeakActivityResults(bool wasSuccessful)` | `wasSuccessful` indica se todos os objetivos da atividade foram alcançados |
| Colidiu com um objeto | `SpeakCollision(objectName)` | `objectName` é o nome do objeto com o qual o jogador colidiu |
| Progresso na atividade | `SpeakObjectiveFound(string currentObjective, string nextObjective)` | `currentObjective` é a missão atual que foi concluída, `nextObjective` é a próxima missão a ser feita pelo jogador  |
| Dica sobre o objetivo | `SpeakHint(string objectiveName)` | `objectiveName` é o nome do objeto a ser encontrado |
| Mensagem de Introdução à Atividade | `SpeakIntro(List<string> objectives)` | `objectives` é a lista com o nome de todos os objetos a serem encontrados |
| Loading | `SpeakLoading()` |  |