# 💸 Empréstimos em Tempo Real com SignalR

Este projeto é uma aplicação simples baseada em .NET 8 + SignalR para gerenciamento de solicitações de empréstimos com atualização em tempo real. A aplicação possui três interfaces separadas:

- **Cliente**: Solicita empréstimos.
- **Operador**: Aprova ou rejeita solicitações.
- **Financeiro**: Visualiza e paga empréstimos aprovados.

---

## 🛠️ Tecnologias Utilizadas

- ASP.NET Core 8 (Minimal API)
- SignalR
- HTML + JavaScript puro
- CSS básico
- Swagger (para testar a API)

---

## 🚀 Como Executar o Projeto

1. **Clone o repositório**:

   ```bash
   git clone https://github.com/seu-usuario/emprestimos-signalr.git
   cd emprestimos-signalr
````

2. **Restaure os pacotes e execute a API**:

   ```bash
   dotnet restore
   dotnet run
   ```

3. **Acesse as interfaces** no navegador:

   * Cliente: [`http://localhost:5000/cliente.html`](http://localhost:5000/cliente.html)
   * Operador: [`http://localhost:5000/operador.html`](http://localhost:5000/operador.html)
   * Financeiro: [`http://localhost:5000/financeiro.html`](http://localhost:5000/financeiro.html)
   * Swagger: [`http://localhost:5000/swagger`](http://localhost:5000/swagger)

---

## 🧩 Funcionalidades

### 👤 Cliente

* Preenche nome, valor e número de parcelas.
* Solicita empréstimo via API.
* Recebe atualização de status em tempo real.

### 🧑‍💼 Operador

* Recebe lista em tempo real de novas solicitações.
* Aprova ou rejeita pedidos.
* Atualizações refletidas automaticamente para todos.

### 💰 Financeiro

* Recebe somente empréstimos aprovados.
* Pode marcar empréstimos como **"Pagos"**.
* Atualizações transmitidas via SignalR.

---

## 🗂️ Estrutura de Arquivos

```text
📁 wwwroot/
 ├── cliente.html
 ├── operador.html
 └── financeiro.html
📁 Models/
 └── Emprestimo.cs
📁 Services/
 └── EmprestimoHub.cs
Program.cs
```

---

## 📦 Endpoints da API

| Verbo | Rota                           | Descrição                                   |
| ----- | ------------------------------ | ------------------------------------------- |
| POST  | /api/emprestimos               | Cria um novo empréstimo                     |
| PUT   | /api/emprestimos/{id}/{status} | Atualiza o status do empréstimo             |
| GET   | /api/emprestimos               | Retorna todos os empréstimos (GET opcional) |

---

## 📡 SignalR Eventos

| Evento SignalR     | Emitido Por | Descrição                                  |
| ------------------ | ----------- | ------------------------------------------ |
| `NovaSolicitacao`  | API         | Enviado a todos os operadores              |
| `StatusAtualizado` | API         | Enviado ao cliente e a todas as interfaces |

---

## ✅ Melhorias Futuras

* Autenticação por função (cliente, operador, financeiro).
* Banco de dados com EF Core.
* Histórico de status.
* Blazor ou React no frontend.

---

## 📄 Licença

Este projeto é livre para uso educacional e pessoal. Sinta-se à vontade para contribuir!
