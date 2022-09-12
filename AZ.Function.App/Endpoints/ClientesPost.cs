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

        var clienteExistente = _clienteRepository.ObterClientePorId(cliente.Id);

        if (clienteExistente is not null)
            return new BadRequestObjectResult("Cliente já cadastrado.");

        _clienteRepository.Adicionar(cliente);

        string responseMessage = _clienteRepository.ObterClientePorId(cliente.Id) is null
            ? "Houve um erro ao cadastrar o usuário."
            : $"Cliente adicionado com sucesso. \nId: [ {cliente.Id} ]";

        return new OkObjectResult(responseMessage);
    }
}
