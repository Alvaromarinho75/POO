## Introdução

O objetivo deste experimento foi implementar uma API REST simples para simular um serviço de monitoramento de temperatura, utilizando C# e ASP.NET. O sistema foi dividido em dois componentes principais: um servidor que fornece leituras de temperatura em diferentes unidades (Celsius, Kelvin e Fahrenheit) e um cliente que consome periodicamente essa API, exibindo as leituras e indicando variações.

A atividade buscou consolidar conhecimentos em desenvolvimento de APIs REST, consumo de serviços HTTP, serialização/deserialização de dados em JSON e manipulação de tarefas assíncronas em C#.

## Desenvolvimento

O projeto foi estruturado em dois módulos principais:

- **ServidorTemp:** Implementa uma API REST usando ASP.NET, expondo um endpoint `/temperatura/{unidade}` que retorna a temperatura atual simulada na unidade solicitada. A temperatura é calculada com base em uma função senoidal do horário atual, acrescida de um ruído aleatório para simular variações reais.
- **MonitorTemp:** Cliente em C# que solicita ao usuário a unidade de temperatura desejada (Celsius, Kelvin ou Fahrenheit) e o intervalo de leitura em segundos. O cliente faz requisições periódicas ao servidor, registra o horário exato de cada leitura e exibe no console o valor da temperatura, indicando se subiu, desceu ou permaneceu igual em relação à leitura anterior, utilizando cores para destacar a variação.

A comunicação entre cliente e servidor foi realizada via HTTP, utilizando o método GET. O servidor retorna respostas no formato JSON, contendo os campos `unidade` e `valor`. O cliente processa a resposta e apresenta o resultado ao usuário, destacando variações de temperatura com cores diferentes no terminal: vermelho para “subiu”, azul para “desceu” e cor padrão para “sem alteração”.

O programa permite interrupção a qualquer momento pelo usuário (Ctrl+C), exibindo uma mensagem de encerramento apropriada. Também foram implementados tratamentos de erro para falhas de conexão ou respostas inválidas, exibindo mensagens em amarelo.

## Resultados

O sistema funcionou conforme o esperado. O servidor respondeu corretamente às requisições nas três unidades suportadas, e o cliente exibiu as leituras de forma clara, indicando as variações de temperatura. O uso de tarefas assíncronas permitiu um monitoramento contínuo sem travar a interface do console.

## Conclusão

A prática permitiu consolidar conceitos fundamentais de APIs REST, manipulação de JSON e programação assíncrona em C#. O projeto demonstrou a integração entre diferentes componentes de software, reforçando a importância do consumo e exposição de serviços em aplicações modernas.

O uso do ASP.NET para a construção do servidor e do `HttpClient` para o consumo das APIs mostrou-se eficiente e alinhado com as práticas atuais de desenvolvimento. O experimento também evidenciou a importância do tratamento de erros e da validação de dados em sistemas distribuídos.