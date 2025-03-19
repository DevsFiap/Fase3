# Fase 3 - Projeto de Criação de Contatos - Arquitetura de Microserviços

Este projeto tem como objetivo implementar uma aplicação para gerenciamento de contatos utilizando uma arquitetura baseada em microserviços. A solução foi dividida em dois serviços independentes, um *producer* e um *consumer*, ambos desenvolvidos com .NET 8. O *producer* é uma Web API, enquanto o *consumer* é um Worker Service. A comunicação entre esses dois serviços é feita através do RabbitMQ, com o *consumer* sendo responsável por processar mensagens da fila e interagir com o banco de dados.

## Arquitetura

![image](https://github.com/user-attachments/assets/5ce5eaa9-1972-4d74-b79d-678325f82b1c)

A arquitetura do projeto é composta pelos seguintes elementos:

1. **Producer (Web API)**: A Web API expõe endpoints para a criação, edição e exclusão de contatos. Essas requisições são recebidas pela API e enviadas para a fila do RabbitMQ, onde o *consumer* irá processá-las.

2. **Consumer (Worker Service)**: O *consumer* é responsável por ler as mensagens da fila RabbitMQ. As mensagens contêm informações sobre os contatos e as operações a serem realizadas. O *consumer* verifica a operação a ser realizada (inserir, editar ou deletar) por meio de um *enum* enviado com a mensagem. O *consumer* também interage com o banco de dados SQL para persistir ou modificar os dados conforme necessário.

3. **RabbitMQ**: RabbitMQ é utilizado como sistema de mensagens para comunicação assíncrona entre os serviços. Há uma única fila chamada "contato", que é usada para enviar e processar as mensagens relacionadas aos contatos.

4. **Dead Letter Queue (DLQ)**: Para garantir a confiabilidade do sistema, o RabbitMQ possui uma DLQ configurada para armazenar mensagens que não possam ser processadas com sucesso. Em caso de falhas, as mensagens são encaminhadas para a DLQ, e uma Azure Function é responsável por ler e processá-las posteriormente.

5. **Azure Functions**: Duas funções do Azure são utilizadas:
   - Uma função lê a DLQ e trata mensagens que não puderam ser processadas pelo *consumer*.
   - A outra função lê os contatos diretamente do banco de dados.

6. **API Gateway (Azure API Management)**: O API Gateway realiza a chamada para o *producer*. Além disso, o Azure API Management (APIM) é configurado com um *rate limit* para limitar o número de requisições que podem ser feitas para o *producer*.

## Tecnologias Utilizadas

- **.NET 8**: Para o desenvolvimento da Web API (Producer) e Worker Service (Consumer).
- **RabbitMQ**: Para comunicação assíncrona entre os microserviços.
- **Azure Container Instances (ACI)**: Para hospedagem do *producer*, *consumer* e RabbitMQ.
- **SQL Database**: Para armazenar os contatos.
- **Azure Functions**: Para tratar mensagens na DLQ e processar contatos.
- **Azure API Management**: Para gerenciar a API e configurar *rate limits*.
- **Prometheus**: Para coleta de métricas do sistema.
- **Grafana**: Para visualização e monitoramento das métricas coletadas pelo Prometheus.

## Como Funciona

1. O API Gateway, através do APIM, realiza chamadas controladas ao *producer*, respeitando o limite de requisições configurado.
2. O *producer* recebe requisições para criar, editar ou deletar contatos.
3. A Web API envia uma mensagem para o RabbitMQ na fila "contato", contendo as informações do contato e o tipo de operação (inserir, editar ou deletar).
4. O *consumer* lê a mensagem da fila e verifica qual operação deve ser realizada (com base no *enum*).
5. O *consumer* realiza a operação no banco de dados SQL.
6. Caso ocorra algum erro, a mensagem é encaminhada para a DLQ no RabbitMQ, onde uma Azure Function pode tratá-la posteriormente.
7. O Prometheus coleta métricas dos serviços e as exibe no Grafana para monitoramento.

## Fluxo de Mensagens

- **Fila RabbitMQ**: Uma única fila chamada "contato" gerencia todas as operações de contatos (inserir, editar, deletar).
- **Enum**: Dentro da mensagem, o *consumer* usa um *enum* para determinar qual operação deve ser realizada no banco de dados.
- **DLQ**: Caso uma operação não possa ser processada, a mensagem é movida para a DLQ para posterior tratamento.
