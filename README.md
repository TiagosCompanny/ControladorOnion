# Teste Programador

## Contexto

*“Seja bem vindo à Onion S.A.!*
*Nós somos uma empresa líder na indústria de eletrônicos, dedicada à inovação e tecnologia. Nossos produtos são eletrônicos como celular, smart TVs e notebooks.*
*Hoje temos um grande problema de controle, manutenção das vendas e logística e precisamos montar um MVP com urgência para nos ajudar com essas questões.*
*Sabendo disso, selecionamos você para desenvolver um sistema revolucionário que irá solucionar todos os nossos problemas!“*

## Descrição do Sistema

O usuário do sistema deverá importar uma planilha de pedidos ([exemplo](https://docs.google.com/spreadsheets/d/1htc2DHNomvfUtr3pOizMjb0d6X9NuKvlGMw-mkUnaiM/edit?usp=sharing)) e logo em seguida a tela deve exibir:

- 📊 **Gráfico de vendas por região**
- 📈 **Gráfico de vendas por produto**
- 📝 **Lista de vendas** com o nome do cliente, produto, valor final com entrega e data de entrega

## Campos da Planilha ([exemplo](https://docs.google.com/spreadsheets/d/1htc2DHNomvfUtr3pOizMjb0d6X9NuKvlGMw-mkUnaiM/edit?usp=sharing](https://docs.google.com/spreadsheets/d/1s3jgRIIKBWqlOTEz5D4HM7yB9yAefzVA/edit?usp=sharing&ouid=111362286810822471623&rtpof=true&sd=true)))

- **CPF ou CNPJ**
- **Nome ou Razão Social**
- **CEP**
- **Produto**
- **Número do pedido**
- **Data**

## Regras de Negócio

- **Valores dos produtos:**
  - 📱 Celular: R$ 1.000
  - 💻 Notebook: R$ 3.000
  - 📺 Televisão: R$ 5.000

- **Cálculo de frete por região:**
  - 🌍 Norte/Nordeste: 30% do valor do produto
  - 🌎 Centro-Oeste/Sul: 20% do valor do produto
  - 🗺 Sudeste: 10% do valor do produto
  - 🏙 São Paulo Capital: gratuito

- **Cálculo do tempo de entrega (a partir da data de entrega):**
  - 🌍 Norte/Nordeste: 10 dias úteis
  - 🌎 Centro-Oeste/Sul: 5 dias úteis
  - 🗺 Sudeste: 1 dia corrido
  - 🏙 São Paulo Capital: no mesmo dia


## Regras

- O teste deverá ser entregue em até **1 semana** do recebimento do mesmo (se for terça, até a meia-noite da próxima terça).
- Deverá conter as tecnologias: **C#**, **Banco de dados InMemory**, **Entity Framework** e um framework Javascript à sua escolha.
- Ser um sistema web.
- Criar seed de produtos.

## Instruções de Setup

1. **Clone o repositório:**

    ```bash
    git clone https://github.com/TiagosCompanny/ControladorOnion.git
    cd ControladorOnion
    ```

2. **Inicie o servidor:**

      ```bash
      dotnet run
      ```

