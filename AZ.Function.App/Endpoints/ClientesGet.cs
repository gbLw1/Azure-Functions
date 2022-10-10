using AZ.Function.App.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AZ.Function.App.Endpoints;

public class ClientesGet
{
    private readonly IClienteRepository _clienteRepository;

    public ClientesGet(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [FunctionName(nameof(ClientesGet))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "clientes/{clienteId:guid?}")] HttpRequest req,
        ILogger log,
        CancellationToken cancellationToken,
        Guid? clienteId = null)
    {
        try
        {
            var queryParams = req.GetQueryParameterDictionary();

            // -> /clientes/{id}
            if (clienteId is not null)
            {
                log.LogCritical("request -> [obter-por-id]");

                var cliente = await _clienteRepository.ObterClientePorId(cancellationToken, clienteId);

                if (req.HttpContext.RequestAborted.IsCancellationRequested)
                {
                    log.LogCritical("request -> [Cancelled]");
                    throw new TaskCanceledException();
                }

                log.LogCritical("request -> [Executed]");

                if (cliente is null)
                {
                    return new BadRequestObjectResult($"Cliente com id: [ {clienteId} ] não encontrado.");
                }

                return new OkObjectResult(cliente);
            }

            // -> /clientes?name=
            else if (queryParams.TryGetValue("name", out string name))
            {
                log.LogCritical("request -> [obter-por-nome]");

                var clientes = await _clienteRepository.ObterClientePorNome(name, cancellationToken);

                if (req.HttpContext.RequestAborted.IsCancellationRequested)
                {
                    log.LogCritical("request -> [Cancelled]");
                    throw new TaskCanceledException();
                }

                log.LogCritical("request -> [Executed]");

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

                var clientes = await _clienteRepository.ObterTodosClientes(cancellationToken);

                if (req.HttpContext.RequestAborted.IsCancellationRequested)
                {
                    log.LogCritical("request -> [Cancelled]");
                    throw new TaskCanceledException();
                }

                log.LogCritical("request -> [Executed]");

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