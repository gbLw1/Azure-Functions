using AZ.Function.App.Data;
using AZ.Function.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AZ.Function.App.Endpoints
{
    public class ClientesPut
    {
        private readonly IClienteRepository _clienteRepository;

        public ClientesPut(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [FunctionName("ClientesPut")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "clientes/{clienteId:guid}")] HttpRequest req,
            ILogger log,
            Guid clienteId)
        {
            log.LogCritical("request -> [clientes-atualizar]");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var cliente = JsonConvert.DeserializeObject<Cliente>(requestBody);

            var result = _clienteRepository.Atualizar(clienteId, cliente);

            dynamic responseMessage = result is null
                ? "Não foi possível atualizar o cliente, cheque os dados inseridos e tente novamente."
                : result;

            return new OkObjectResult(responseMessage);
        }
    }
}
