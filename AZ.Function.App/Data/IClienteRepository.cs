using AZ.Function.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AZ.Function.App.Data;

public interface IClienteRepository
{
    //List<Cliente> Clientes { get; }
    Task<IEnumerable<Cliente>> ObterTodosClientes();
    Task<Cliente> ObterClientePorId(Guid? id = null);
    Task<IEnumerable<Cliente>> ObterClientePorNome(string nome);
    Task Adicionar(Cliente cliente);
    Task<Cliente> Atualizar(Guid clienteId, Cliente cliente);
    Task Remover(Cliente cliente);
}