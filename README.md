# Consultorio_API – Persistência + API em C#

API REST desenvolvida em **C# (.NET)** utilizando **Entity Framework Core** e **SQLite**, para o gerenciamento de **pacientes de um consultório**.  
O projeto inclui tanto a **API Web** quanto uma **interface de Console** que acessam o mesmo banco de dados.

---

## ⚙️ Passos para Rodar o Projeto

🧩 1. Clonar o repositório

🧩 2. Restaurar pacotes

dotnet restore

🧩 3. Criar o banco de dados (SQLite)

Execute os comandos do Entity Framework Core para gerar o banco consultorio.db:

dotnet ef migrations add InitialCreate  
dotnet ef database update

Isso cria a tabela Patients automaticamente no banco SQLite.

🧩 4. Executar a API

dotnet run

  
A aplicação será iniciada em:

http://localhost:5099


Acesse o Swagger para testar as rotas:

http://localhost:5099/swagger

<br><br>
🧱 Entidade Principal – Patient(Paciente)

Representa os pacientes cadastrados no consultório.
Mapeada pelo Entity Framework Core e validada com DataAnnotations.

| Campo         | Tipo     | Obrigatório | Validações                                                |
| ------------- | -------- | ----------- | --------------------------------------------------------- |
| **Id**        | int      | Sim         | Chave primária                                            |
| **Name**      | string   | Sim         | `[Required]`, `[MaxLength(120)]`                          |
| **CPF**       | string   | Sim         | `[Required]`, `[MaxLength(14)]`, único                    |
| **Email**     | string   | Sim         | `[Required]`, `[EmailAddress]`, `[MaxLength(100)]`, único |
| **BirthDate** | DateOnly | Sim         | `[Required]`, `[Column(TypeName = "DATE")]`               |

<br><br>
🧠 Regras de negócio

Todos os campos são obrigatórios.

CPF e Email são únicos no banco.

BirthDate deve seguir o formato dd/MM/yyyy.

Erros de validação retornam 400 BadRequest.

Duplicação de dados retorna 409 Conflict.

<br><br>
🌐 Rotas da API
| Método     | Rota                    | Descrição                   | Retornos possíveis                          |
| ---------- | ----------------------- | --------------------------- | ------------------------------------------- |
| **GET**    | `/api/v1/patients`      | Lista todos os pacientes    | 200 OK                                      |
| **GET**    | `/api/v1/patients/{id}` | Busca paciente pelo ID      | 200 OK / 404 NotFound                       |
| **POST**   | `/api/v1/patients`      | Cadastra novo paciente      | 201 Created / 400 BadRequest / 409 Conflict |
| **PUT**    | `/api/v1/patients/{id}` | Atualiza paciente existente | 200 OK / 404 NotFound / 409 Conflict        |
| **DELETE** | `/api/v1/patients/{id}` | Remove paciente pelo ID     | 204 NoContent / 404 NotFound                |

<br><br>
## 📬 Exemplos de Requisições

➕ Criar paciente (POST)

POST http://localhost:5099/api/v1/patients
Content-Type: application/json

{
  "name": "Maria Oliveira",
  "email": "maria@teste.com",
  "cpf": "12345678901",
  "birthDate": "1998-03-15"
}


Respostas possíveis:

✅ 201 Created – Paciente criado com sucesso

⚠️ 400 BadRequest – Dados inválidos ou ausentes

⚠️ 409 Conflict – CPF ou Email já cadastrados

<br><br>
🔍 Buscar paciente por ID (GET)

GET http://localhost:5099/api/v1/patients/1

Respostas:

✅ 200 OK

⚠️ 404 NotFound – Paciente inexistente

<br><br>
✏️ Atualizar paciente (PUT)

PUT http://localhost:5099/api/v1/patients/1
Content-Type: application/json

{
  "name": "Maria O. Santos",
  "email": "maria@teste.com",
  "cpf": "12345678901",
  "birthDate": "1998-03-15"
}

Respostas:

✅ 200 OK – Atualizado com sucesso

⚠️ 404 NotFound – ID não encontrado

⚠️ 409 Conflict – CPF ou Email duplicados

<br><br>
❌ Excluir paciente (DELETE)

DELETE http://localhost:5099/api/v1/patients/1

Respostas:

✅ 204 NoContent – Removido com sucesso

⚠️ 404 NotFound – Paciente não encontrado

<br><br>
## 🧪 Como Testar a API

🔹 Opção 1 – Swagger

Execute o projeto:

dotnet run


Acesse:
👉 http://localhost:5099/swagger

Envie requisições diretamente pelo navegador.

<br><br>
🔹 Opção 2 – Postman

Abra o Postman.

Crie uma nova coleção chamada Consultório API.

Adicione as rotas acima (GET, POST, PUT, DELETE).

Envie as requisições com os exemplos JSON.

Verifique os retornos e códigos HTTP.

<br><br>
🔹 Opção 3 – Arquivo .http

Crie um arquivo requests.http e cole:

### Criar paciente
POST http://localhost:5099/api/v1/patients
Content-Type: application/json

{
  "name": "João Souza",
  "email": "joao@teste.com",
  "cpf": "98765432100",
  "birthDate": "1999-08-12"
}

### Listar todos
GET http://localhost:5099/api/v1/patients

### Buscar por ID
GET http://localhost:5099/api/v1/patients/1

### Atualizar
PUT http://localhost:5099/api/v1/patients/1
Content-Type: application/json

{
  "name": "João S. Lima",
  "email": "joao@teste.com",
  "cpf": "98765432100",
  "birthDate": "1999-08-12"
}

### Excluir
DELETE http://localhost:5099/api/v1/patients/1


Depois, clique em “Send Request” (se estiver no VS Code com a extensão REST Client).

<br><br>
💾 Banco de Dados – consultorio.db

Criado automaticamente pelo Entity Framework Core via migrations.
<br><br>
👩‍💻 Autora: Letícia de Souza de Almeida
📚 Disciplina: Desenvolvimnto de Sistemas
🏫 Instituição: UniCEUB
👨‍🏫 Professor: Fábio
