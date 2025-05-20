```markdown
# 📊 Sistema de Empréstimos em Tempo Real com SignalR

Este projeto é uma aplicação web para gerenciamento de empréstimos, com comunicação em tempo real usando **SignalR**, API RESTful com ASP.NET Core e páginas HTML interativas para diferentes perfis: **Clientes**, **Operadores** e **Financeiro**.

---

## 🚀 Funcionalidades

- ✍️ **Solicitação de Empréstimos** por clientes
- 🕵️‍♂️ **Aprovação ou Recusa** por operadores
- 💰 **Confirmação de Pagamento** pelo setor financeiro
- 🔄 **Atualizações em tempo real** via SignalR para cada grupo de usuários
- 📑 Interface clara e separada para cada tipo de usuário

---

## 🛠️ Tecnologias Utilizadas

- ASP.NET Core 7.0 (Minimal APIs)
- SignalR (para comunicação em tempo real)
- HTML + CSS puro (sem frameworks)
- JavaScript (client-side)
- Entity Framework Core (persistência)
- SQL Server (ou InMemory para testes)

---

## 📁 Estrutura do Projeto

```

/Projeto
├── Controllers
│   └── EmprestimosController.cs
├── Hubs
│   └── EmprestimoHub.cs
├── Models
│   ├── Emprestimo.cs
│   └── Enums/StatusEmprestimo.cs
├── Services
│   └── EmprestimoService.cs
├── wwwroot
│   ├── cliente.html
│   ├── operador.html
│   └── financeiro.html
├── Program.cs
└── README.md

````

---

## 🧪 Endpoints da API

### `POST /api/emprestimos`
> Solicita um novo empréstimo.

**Body (JSON):**
```json
{
  "cliente": "João Silva",
  "valor": 1000.0,
  "parcelas": 12
}
````

---

### `GET /api/emprestimos/pendentes`

> Lista empréstimos pendentes para aprovação (usado por operadores)

---

### `PUT /api/emprestimos/{id}/aprovar`

> Aprova um empréstimo e notifica clientes e financeiro via SignalR.

---

### `PUT /api/emprestimos/{id}/recusar`

> Recusa um empréstimo.

---

### `GET /api/emprestimos/aprovados`

> Lista empréstimos aprovados (usado pelo financeiro)

---

### `PUT /api/emprestimos/{id}/pagar`

> Marca empréstimo como pago e notifica clientes e operadores.

---

## 💬 SignalR - Grupos e Eventos

| Grupo        | Usuários conectados     | Eventos recebidos  |
| ------------ | ----------------------- | ------------------ |
| `clientes`   | Páginas cliente.html    | `StatusAtualizado` |
| `operadores` | Páginas operador.html   | `StatusAtualizado` |
| `financeiro` | Páginas financeiro.html | `StatusAtualizado` |

**Evento `StatusAtualizado(emp)`** é disparado sempre que o status do empréstimo muda.

---

## 🌐 Páginas HTML

### 🧍 cliente.html

* Solicita empréstimo
* Recebe status em tempo real

### 🧑‍💼 operador.html

* Aprova ou recusa empréstimos
* Visualiza solicitações pendentes

### 🧾 financeiro.html

* Visualiza empréstimos aprovados
* Marca como pagos
* Recebe atualizações em tempo real

---

## ▶️ Como Executar

1. Clone o repositório:

```bash
git clone https://github.com/seu-usuario/emprestimos-signalr.git
```

2. Navegue para o projeto e execute:

```bash
dotnet run
```

3. Acesse as páginas diretamente:

* [http://localhost:5000/cliente.html](http://localhost:5000/cliente.html)
* [http://localhost:5000/operador.html](http://localhost:5000/operador.html)
* [http://localhost:5000/financeiro.html](http://localhost:5000/financeiro.html)

---

## ✅ Melhorias Futuras

* Autenticação e autorização por perfil
* Dashboard com gráficos e métricas
* Histórico de empréstimos
* Testes automatizados com xUnit + Testcontainers

---

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

---