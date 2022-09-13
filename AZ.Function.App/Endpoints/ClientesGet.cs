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

            // -> /clientes/{id}
            if (clienteId is not null)
            {
                log.LogCritical("request -> [obter-por-id]");

                var cliente = await _clienteRepository.ObterClientePorId(clienteId);

                if (cliente is null)
                {
                    return new BadRequestObjectResult($"Cliente com id: [ {clienteId} ] não encontrado.");
                }

                return new OkObjectResult(cliente);
            }

            // -> /clientes?nome=
            else if (queryParams.TryGetValue("nome", out string nome))
            {
                log.LogCritical("request -> [obter-por-nome]");

                var clientes = await _clienteRepository.ObterClientePorNome(nome);

                if (!clientes.Any())
                {
                    return new BadRequestObjectResult("Não foi encontrado nenhum cliente, verifique o nome informado.");
                }

                return new OkObjectResult(clientes);
            }

            // -> /clientes
            else
            {
                log.LogCritical("request -> [obter-todos]");

                var clientes = await _clienteRepository.ObterTodosClientes();

                if (!clientes.Any())
                {
                    return new BadRequestObjectResult("Não há nenhum cliente cadastrado.");
                }

                return new OkObjectResult(clientes);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
