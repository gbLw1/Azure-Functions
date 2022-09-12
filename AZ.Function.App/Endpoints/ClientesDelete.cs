using AZ.Function.App.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AZ.Function.App.Endpoints
{
    public class ClientesDelete
    {
        private readonly IClienteRepository _clienteRepository;

        public ClientesDelete(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [FunctionName("ClientesDelete")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "clientes/{clienteId:guid}")] HttpRequest req,
            ILogger log,
            Guid clienteId)
        {
            log.LogCritical("request -> [clientes-remover]");

            var cliente = _clienteRepository.ObterClientePorId(clienteId);

            if (cliente is not null)
            {
                _clienteRepository.Remover(cliente);
            }

            var result = _clienteRepository.ObterClientePorId(cliente.Id);

            string responseMessage = result is not null
                ? "Não foi possível remover o cliente, cheque o ID informado."
                : "Cliente removido com sucesso.";

            return new OkObjectResult(responseMessage);
        }
    }
}
