using AZ.Function.App.Data;
using AZ.Function.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace AZ.Function.App.Endpoints;

public class ClientesPost
{
    private readonly IClienteRepository _clienteRepository;

    public ClientesPost(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [FunctionName("ClientesPost")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "clientes")] HttpRequest req,
        ILogger log)
    {
        log.LogCritical("request -> [clientes-adicionar]");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var cliente = JsonConvert.DeserializeObject<Cliente>(requestBody);

        // teste de usu�rio existente
        var clienteExistente = await _clienteRepository.ObterClientePorId(cliente.Id);

        if (clienteExistente is not null)
            return new BadRequestObjectResult("Cliente j� cadastrado.");

        // cadastra o usu�rio
        await _clienteRepository.Adicionar(cliente);

        // busca o usu�rio para testar se foi cadastrado
        if (_clienteRepository.ObterClientePorId(cliente.Id) is null)
        {
            return new BadRequestObjectResult("Houve um erro ao cadastrar o usu�rio.");
        }

        string responseMessage = $"Cliente adicionado com sucesso. \nId: [ {cliente.Id} ]";

        return new OkObjectResult(responseMessage);
    }
}
