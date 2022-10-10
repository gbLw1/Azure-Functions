using AZ.Function.App.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AZ.Function.App.Endpoints;

public class ClientesDelete
{
    private readonly IClienteRepository _clienteRepository;

    public ClientesDelete(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [FunctionName(nameof(ClientesDelete))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "clientes/{clienteId:guid}")] HttpRequest req,
        ILogger log,
        Guid clienteId,
        CancellationToken cancellationToken)
    {
        log.LogCritical("request -> [clientes-remover]");

        var cliente = await _clienteRepository.ObterClientePorId(cancellationToken, clienteId);

        if (cliente is null)
        {
            return new BadRequestObjectResult("Não foi possível remover o cliente, cheque o ID informado pois nenhum foi encontrado.");
        }

        await _clienteRepository.Remover(cliente);

        return new OkObjectResult("Cliente removido com sucesso.");
    }
}