using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Consultorio.Data;
using Consultorio.Models;

var builder = WebApplication.CreateBuilder(args);

// Porta fixa (opcional, facilita testes)
builder.WebHost.UseUrls("http://localhost:5099");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=consultorio.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

var webTask = app.RunAsync();
Console.WriteLine("API online em http://localhost:5099 (Swagger em /swagger)");

Console.WriteLine("== API CONSULTÓRIO - Sistema de Pacientes ==");
Console.WriteLine("Console + API executando juntos!");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Escolha uma opção:");
    Console.WriteLine("1 - Cadastrar paciente");
    Console.WriteLine("2 - Listar pacientes");
    Console.WriteLine("3 - Atualizar paciente (por Id)");
    Console.WriteLine("4 - Remover paciente (por Id)");
    Console.WriteLine("0 - Sair");
    Console.Write("> ");

    var opt = Console.ReadLine();

    if (opt == "0") break;

    switch (opt)
    {
    case "1":
        await CreatePatientAsync();
        break;

    case "2":
        await ListPatientsAsync();
        break;

    case "3":
        await UpdatePatientAsync();
        break;

    case "4":
        await DeletePatientAsync();
        break;

    default:
        Console.WriteLine("Opção inválida.");
        break;
    }
}

await app.StopAsync();
await webTask;

async Task CreatePatientAsync()
{
    Console.Write("Nome: ");
    var name = (Console.ReadLine() ?? "").Trim();

    Console.Write("Email: ");
    var email = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    Console.Write("CPF: ");
    var cpf = (Console.ReadLine() ?? "").Trim();

    Console.Write("Data de Nascimento (dd/MM/yyyy): ");
    var birthDateString = Console.ReadLine(); // Lê como string
    
    // Validação básica e conversão
    if (!System.DateTime.TryParseExact(
        birthDateString, "dd/MM/yyyy",null,
        System.Globalization.DateTimeStyles.None,out var birthDate)){Console.WriteLine("Formato de data inválido ou data não fornecida. Use dd/MM/yyyy."); return; }

    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(cpf))
    {
        Console.WriteLine("Nome, Email e CPF são obrigatórios.");
        return;
    }

    using var db = new AppDbContext();
    var emailExists = await db.Patients.AnyAsync(p => p.Email == email);
    if (emailExists) { Console.WriteLine("Já existe um paciente com esse email."); return; }

    var cpfExists = await db.Patients.AnyAsync(p => p.CPF == cpf);
    if (cpfExists){ Console.WriteLine("Já existe um paciente com esse CPF."); return; }

    var patient = new Patient { Name = name, Email = email, CPF = cpf, BirthDate = DateOnly.Parse(birthDateString)};
    db.Patients.Add(patient);
    await db.SaveChangesAsync();
    Console.WriteLine($"Cadastrado com sucesso! Id: {patient.Id}");
}

async Task ListPatientsAsync()
{
    using var db = new AppDbContext();
    var patients = await db.Patients.OrderBy(p => p.Id).ToListAsync();

    if (patients.Count == 0) { Console.WriteLine("Nenhum paciente encontrado."); return; }

    Console.WriteLine("Id | Name                 | Email                    | CPF                    | BirthDate                    ");
    foreach (var p in patients)
        Console.WriteLine($"{p.Id,2} | {p.Name,-20} | {p.Email,-24} | {p.CPF,-24} | {p.BirthDate:dd/MM/yyyy}");
}

async Task UpdatePatientAsync()
{
    Console.Write("Informe o Id do paciente a atualizar: ");
    if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

    using var db = new AppDbContext();
    var patient = await db.Patients.FirstOrDefaultAsync(p => p.Id == id);
    if (patient is null) { Console.WriteLine("Paciente não encontrado."); return; }

    Console.WriteLine($"Atualizando Id {patient.Id}. Deixe em branco para manter.");
    Console.WriteLine($"Nome atual : {patient.Name}");
    Console.Write("Novo nome  : ");
    var newName = (Console.ReadLine() ?? "").Trim();

    Console.WriteLine($"Email atual: {patient.Email}");
    Console.Write("Novo email : ");
    var newEmail = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    if (!string.IsNullOrWhiteSpace(newName)) patient.Name = newName;
    if (!string.IsNullOrWhiteSpace(newEmail))
    {
        var emailTaken = await db.Patients.AnyAsync(p => p.Email == newEmail && p.Id != id);
        if (emailTaken) { Console.WriteLine("Já existe outro paciente com esse email."); return; }
        patient.Email = newEmail;
    }

    await db.SaveChangesAsync();
    Console.WriteLine("Paciente atualizado com sucesso.");
}

async Task DeletePatientAsync()
{
    Console.Write("Informe o Id do paciente a remover: ");
    if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

    using var db = new AppDbContext();
    var patient = await db.Patients.FirstOrDefaultAsync(p => p.Id == id);
    if (patient is null) { Console.WriteLine("Paciente não encontrado."); return; }

    db.Patients.Remove(patient);
    await db.SaveChangesAsync();
    Console.WriteLine("Paciente removido com sucesso.");
}
