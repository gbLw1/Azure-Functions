using AZ.Function.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AZ.Function.App.Data;

public interface IClienteRepository
{
    List<Cliente> Clientes { get; }
    Task<IEnumerable<Cliente>> ObterTodosClientes();
    Cliente ObterClientePorId(Guid? id = null);
    IEnumerable<Cliente> ObterClientePorNome(string nome);
    void Adicionar(Cliente cliente);
    Cliente Atualizar(Guid clienteId, Cliente cliente);
    void Remover(Cliente cliente);

}