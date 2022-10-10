using AZ.Function.App.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AZ.Function.App.Data;

public interface IClienteRepository
{
    //List<Cliente> Clientes { get; }
    Task<IEnumerable<Cliente>> ObterTodosClientes(CancellationToken cancellationToken);
    Task<Cliente> ObterClientePorId(CancellationToken cancellationToken, Guid? id = null);
    Task<IEnumerable<Cliente>> ObterClientePorNome(string nome, CancellationToken cancellationToken);
    Task Adicionar(Cliente cliente);
    Task<Cliente> Atualizar(Guid clienteId, Cliente cliente);
    Task Remover(Cliente cliente);
}