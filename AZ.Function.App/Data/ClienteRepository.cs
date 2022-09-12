using AZ.Function.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AZ.Function.App.Data;

public class ClienteRepository : IClienteRepository
{
    public List<Cliente> Clientes { get; private set; } = new List<Cliente>();

    public Cliente ObterClientePorId(Guid? id)
    {
        return Clientes.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Cliente> ObterClientePorNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return Enumerable.Empty<Cliente>();
        }

        return Clientes.Where(c => c.Nome.Contains(nome)).ToList();
    }

    public async Task<List<Cliente>> ObterTodosClientes()
    {
        await Task.Delay(1000);
        return Clientes;
    }

    public void Adicionar(Cliente cliente)
    {
        Clientes.Add(cliente);
    }

    public Cliente Atualizar(Guid clienteId, Cliente clienteUpdate)
    {
        var cliente = Clientes.FirstOrDefault(c => c.Id == clienteId);

        if (cliente is not null)
        {
            cliente.Nome = clienteUpdate.Nome;
            return cliente;
        }

        return null;
    }

    public void Remover(Cliente cliente)
    {
        Clientes.Remove(cliente);
    }
}
