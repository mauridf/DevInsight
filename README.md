
# DevInsight API

**DevInsight** – Plataforma para Consultoria em Desenvolvimento de Sistemas

📌 **Visão Geral**  
Sistema web voltado à o da consultoria em desenvolvimento de sistemas, cobrindo todas as etapas: diagnóstico, proposta, roadmap, o e entrega. 
Desenvolvido em .NET 9 com PostgreSQL, JWT, BCrypt e arquitetura DDD + Clean Architecture.
A API oferece endpoints para:

- Autenticação e gerenciamento de usuários
- Gerenciamento completo de projetos
- Controle de requisitos, funcionalidades e tarefas
- Gestão de reuniões e stakeholders
- Validações técnicas e entregáveis
- Documentação e soluções propostas

🚀 **Como Executar a API**

### Pré-requisitos
- .NET 9.0 SDK
- PostgreSQL
- Git

### Configuração Inicial

**Clonar o repositório:**
```bash
git clone https://github.com/seu-usuario/devinsight-api.git
cd devinsight-api
```

**Configurar banco de dados:**
Crie um banco PostgreSQL chamado `DevInsight` e atualize a connection string no `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=5432;Database=DevInsight;User Id=postgres;Password=sua-senha;"
}
```

**Configurar JWT:**
```json
"Jwt": {
  "Key": "uma-chave-secreta-longa-e-complexa",
  "Issuer": "DevInsight",
  "Audience": "DevInsightUsers",
  "ExpiryInMinutes": 60
}
```

**Configurar AWS S3 (opcional):**
```json
"AWS": {
  "BucketName": "devinsight-uploads",
  "Region": "us-east-1",
  "AccessKey": "sua-access-key",
  "SecretKey": "sua-secret-key"
}
```

**Aplicar migrações:**
```bash
dotnet ef database update
```

**Executar a aplicação:**
```bash
dotnet run
```
A API estará disponível em `https://localhost:7168` com Swagger ativado.

🔐 **Autenticação**  
A API usa JWT para autenticação. No Swagger:

1. Faça login ou registre um usuário.
2. Copie o token retornado.
3. Clique no botão "Authorize".
4. Cole o token no formato: `Bearer seu-token-aqui`.

## 📚 Endpoints

### 🔐 Autenticação (`AuthController`)

| Método | Endpoint                  | Descrição              | Autenticação       |
|--------|---------------------------|-------------------------|---------------------|
| POST   | `/api/auth/registrar`     | Registrar novo usuário | Não                 |
| POST   | `/api/auth/login`         | Fazer login            | Não                 |

### 👤 Usuários (`UsuarioController`)

| Método | Endpoint                            | Descrição                 | Autenticação       |
|--------|-------------------------------------|----------------------------|---------------------|
| GET    | `/api/usuarios/{id}`                | Obter usuário por ID      | Qualquer usuário    |
| GET    | `/api/usuarios/email/{email}`       | Obter usuário por email   | Qualquer usuário    |
| GET    | `/api/usuarios`                     | Listar todos usuários     | Admin               |
| PUT    | `/api/usuarios/{id}`                | Atualizar usuário         | Próprio usuário     |
| DELETE | `/api/usuarios/{id}`                | Excluir usuário           | Admin               |

### 📁 Projetos (`ProjetoController`)

| Método | Endpoint                                   | Descrição                         | Autenticação          |
|--------|--------------------------------------------|------------------------------------|------------------------|
| POST   | `/api/projetos`                            | Criar novo projeto                | Qualquer usuário       |
| GET    | `/api/projetos/{id}`                       | Obter projeto por ID              | Qualquer usuário       |
| GET    | `/api/projetos`                            | Listar todos projetos             | Qualquer usuário       |
| GET    | `/api/projetos/usuario/{usuarioId}`        | Listar projetos por usuário       | Admin, Consultor       |
| GET    | `/api/projetos/meus-projetos`              | Listar projetos do usuário atual | Qualquer usuário       |
| PUT    | `/api/projetos/{id}`                       | Atualizar projeto                 | Dono do projeto        |
| DELETE | `/api/projetos/{id}`                       | Excluir projeto                   | Admin, Consultor       |

### 🔧 Funcionalidades (`FuncionalidadeController`)

| Método | Endpoint                                                   | Descrição                    | Autenticação      |
|--------|------------------------------------------------------------|-------------------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/funcionalidades`               | Criar funcionalidade         | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/funcionalidades/{id}`          | Obter funcionalidade         | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/funcionalidades`               | Listar funcionalidades       | Qualquer usuário   |
| PUT    | `/api/projetos/{projetoId}/funcionalidades/{id}`          | Atualizar funcionalidade     | Qualquer usuário   |
| DELETE | `/api/projetos/{projetoId}/funcionalidades/{id}`          | Excluir funcionalidade       | Admin, Consultor   |

### 📋 Requisitos (`RequisitoController`)

| Método | Endpoint                                              | Descrição                | Autenticação      |
|--------|-------------------------------------------------------|---------------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/requisitos`               | Criar requisito          | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/requisitos/{id}`          | Obter requisito          | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/requisitos`               | Listar requisitos        | Qualquer usuário   |
| PUT    | `/api/projetos/{projetoId}/requisitos/{id}`          | Atualizar requisito      | Qualquer usuário   |
| DELETE | `/api/projetos/{projetoId}/requisitos/{id}`          | Excluir requisito        | Admin, Consultor   |

### 👥 Stakeholders (`StakeHolderController`)

| Método | Endpoint                                                 | Descrição              | Autenticação      |
|--------|----------------------------------------------------------|-------------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/stakeholders`               | Criar stakeholder      | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/stakeholders/{id}`          | Obter stakeholder      | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/stakeholders`               | Listar stakeholders    | Qualquer usuário   |
| PUT    | `/api/projetos/{projetoId}/stakeholders/{id}`          | Atualizar stakeholder  | Qualquer usuário   |
| DELETE | `/api/projetos/{projetoId}/stakeholders/{id}`          | Excluir stakeholder    | Admin, Consultor   |

### ✅ Tarefas (`TarefaController`)

| Método | Endpoint                                                 | Descrição                  | Autenticação      |
|--------|----------------------------------------------------------|-----------------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/tarefas`                    | Criar tarefa               | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/tarefas/{id}`               | Obter tarefa               | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/tarefas`                    | Listar tarefas             | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/tarefas/kanban`             | Listar tarefas (Kanban)    | Qualquer usuário   |
| PUT    | `/api/projetos/{projetoId}/tarefas/{id}`               | Atualizar tarefa           | Qualquer usuário   |
| PATCH  | `/api/projetos/{projetoId}/tarefas/{id}/status`        | Atualizar status da tarefa | Qualquer usuário   |
| DELETE | `/api/projetos/{projetoId}/tarefas/{id}`               | Excluir tarefa             | Admin, Consultor   |

### 📅 Reuniões (`ReuniaoController`)

| Método | Endpoint                                                   | Descrição                  | Autenticação      |
|--------|------------------------------------------------------------|-----------------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/reunioes`                     | Criar reunião              | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/reunioes/{id}`                | Obter reunião              | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/reunioes`                     | Listar reuniões            | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/reunioes/proximas`           | Listar próximas reuniões   | Qualquer usuário   |
| PUT    | `/api/projetos/{projetoId}/reunioes/{id}`                | Atualizar reunião          | Qualquer usuário   |
| DELETE | `/api/projetos/{projetoId}/reunioes/{id}`                | Excluir reunião            | Admin, Consultor   |

### 🧪 Validações Técnicas (`ValidacaoTecnicaController`)

| Método | Endpoint                                                              | Descrição              | Autenticação      |
|--------|-----------------------------------------------------------------------|-------------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/validacoes-tecnicas`                     | Criar validação técnica| Admin, Consultor   |
| GET    | `/api/projetos/{projetoId}/validacoes-tecnicas/{id}`               | Obter validação        | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/validacoes-tecnicas`                    | Listar validações      | Qualquer usuário   |
| PUT    | `/api/projetos/{projetoId}/validacoes-tecnicas/{id}`               | Atualizar validação    | Admin, Consultor   |
| PATCH  | `/api/projetos/{projetoId}/validacoes-tecnicas/{id}/validar`       | Marcar como validado   | Admin, Consultor   |
| DELETE | `/api/projetos/{projetoId}/validacoes-tecnicas/{id}`               | Excluir validação      | Admin, Consultor   |

### 📦 Entregas Finais (`EntregaFinalController`)

| Método | Endpoint                                                      | Descrição            | Autenticação      |
|--------|---------------------------------------------------------------|-----------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/entregas-finais`                 | Criar entrega final  | Admin, Consultor   |
| GET    | `/api/projetos/{projetoId}/entregas-finais/{id}`            | Obter entrega final  | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/entregas-finais`                 | Listar entregas      | Qualquer usuário   |
| PUT    | `/api/projetos/{projetoId}/entregas-finais/{id}`            | Atualizar entrega    | Admin, Consultor   |
| DELETE | `/api/projetos/{projetoId}/entregas-finais/{id}`            | Excluir entrega      | Admin, Consultor   |

### 💡 Soluções Propostas (`SolucaoPropostaController`)

| Método | Endpoint                                                     | Descrição              | Autenticação      |
|--------|--------------------------------------------------------------|-------------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/solucoes-propostas`             | Criar solução proposta| Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/solucoes-propostas/{id}`        | Obter solução         | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/solucoes-propostas`             | Listar soluções       | Qualquer usuário   |
| PUT    | `/api/projetos/{projetoId}/solucoes-propostas/{id}`        | Atualizar solução     | Qualquer usuário   |
| DELETE | `/api/projetos/{projetoId}/solucoes-propostas/{id}`        | Excluir solução       | Admin, Consultor   |

### 📂 Entregáveis Gerados (`EntregavelGeradoController`)

| Método | Endpoint                                                    | Descrição                | Autenticação      |
|--------|-------------------------------------------------------------|---------------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/entregaveis`                   | Criar entregável         | Admin, Consultor   |
| GET    | `/api/projetos/{projetoId}/entregaveis/{id}`              | Obter entregável         | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/entregaveis`                   | Listar entregáveis       | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/entregaveis/{id}/download`     | Gerar URL de download    | Qualquer usuário   |
| DELETE | `/api/projetos/{projetoId}/entregaveis/{id}`              | Excluir entregável       | Admin, Consultor   |

### 🔗 Documentos Links (`DocumentoLinkController`)

| Método | Endpoint                                                    | Descrição                 | Autenticação      |
|--------|-------------------------------------------------------------|----------------------------|--------------------|
| POST   | `/api/projetos/{projetoId}/documentos-links`              | Criar documento link      | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/documentos-links/{id}`         | Obter documento link      | Qualquer usuário   |
| GET    | `/api/projetos/{projetoId}/documentos-links`              | Listar documentos         | Qualquer usuário   |
| PUT    | `/api/projetos/{projetoId}/documentos-links/{id}`         | Atualizar documento link  | Qualquer usuário   |
| DELETE | `/api/projetos/{projetoId}/documentos-links/{id}`         | Excluir documento link    | Admin, Consultor   |

🛠️ **Tecnologias Utilizadas**
- **Backend**: .NET 6
- **Banco de Dados**: PostgreSQL
- **Autenticação**: JWT
- **Armazenamento**: AWS S3
- **Documentação**: Swagger
- **ORM**: Entity Framework Core
- **Logging**: Serilog
- **Testes**: xUnit (recomendado para implementação futura)

🤝 **Contribuição**
1. Fork o projeto
2. Crie uma branch (`feature/nova-feature`)
3. Commit (`git commit -m 'Adiciona nova feature'`)
4. Push (`git push origin feature/nova-feature`)
5. Abra um Pull Request

📧 **Contato**
- Email: mauridf@gmail.com
