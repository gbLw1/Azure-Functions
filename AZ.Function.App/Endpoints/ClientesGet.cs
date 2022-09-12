using AZ.Function.App.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AZ.Function.App.Endpoints;

public class ClientesGet
{
    private readonly IClienteRepository _clienteRepository;

    public ClientesGet(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [FunctionName("ClientesGet")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "clientes/{clienteId?}")] HttpRequest req,
        ILogger log,
        Guid? clienteId = null)
    {
        try
        {
            var queryParams = req.GetQueryParameterDictionary();

            if (clienteId is not null)
            {
                log.LogCritical("request -> [obter-por-id]");

                var cliente = _clienteRepository.ObterClientePorId(clienteId);

                dynamic responseMessage = cliente is null
                    ? $"Cliente com id: [ {clienteId} ] não encontrado."
                    : cliente;

                return new OkObjectResult(responseMessage);
            }
            else if (queryParams.TryGetValue("nome", out string nome))
            {
                log.LogCritical("request -> [obter-por-nome]");

                var cliente = _clienteRepository.ObterClientePorNome(nome);

                dynamic responseMessage = cliente is null
                    ? $"Cliente com nome: [ {nome} ] não encontrado."
                    : cliente;

                return new OkObjectResult(responseMessage);
            }
            else
            {
                log.LogCritical("request -> [obter-todos]");

                var clientes = await _clienteRepository.ObterTodosClientes();

                dynamic responseMessage = clientes is null
                    ? "Não há nenhum cliente cadastrado."
                    : clientes.ToList();

                return new OkObjectResult(responseMessage);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
