using AZ.Function.App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AZ.Function.App.Data;

public class ClienteRepository : IClienteRepository
{
    //public List<Cliente> Clientes { get; private set; } = new List<Cliente>();
    private readonly FunctionsDbContext _context;

    public ClienteRepository(FunctionsDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente> ObterClientePorId(Guid? id)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Cliente>> ObterClientePorNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return Enumerable.Empty<Cliente>();
        }

        return await _context.Clientes.Where(c => c.Nome.Contains(nome)).ToListAsync();
    }

    public async Task<IEnumerable<Cliente>> ObterTodosClientes(CancellationToken cancellationToken)
    {
        await Task.Delay(5000, cancellationToken);
        return await _context.Clientes.ToListAsync(cancellationToken);
    }

    public async Task Adicionar(Cliente cliente)
    {
        //Clientes.Add(cliente);
        _context.Clientes.Add(cliente);

        await PersistirDados();
    }

    public async Task<Cliente> Atualizar(Guid clienteId, Cliente clienteUpdate)
    {
        var cliente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);

        if (cliente is not null)
        {
            cliente.Nome = clienteUpdate.Nome;

            _context.Clientes.Update(cliente);

            await PersistirDados();

            return cliente;
        }

        return null;
    }

    public async Task Remover(Cliente cliente)
    {
        //Clientes.Remove(cliente);
        _context.Clientes.Remove(cliente);
        await PersistirDados();
    }

    async Task PersistirDados()
    {
        var changes = await _context.SaveChangesAsync();

        if (changes == 0)
        {
            throw new InvalidOperationException("Erro ao persistir os dados.");
        }
    }
}
