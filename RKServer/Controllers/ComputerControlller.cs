using AutoMapper;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Server.BL.Interface;
using Server.Models.Models;
using Server.Models.Request;
using Server.Models.Response;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RKServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ComputerController : ControllerBase
    {
        private readonly IComputerService _computerService;
        private readonly IMapper _mapper;

        public ComputerController(IComputerService computerService, IMapper mapper)
        {
            _computerService = computerService;
            _mapper = mapper;
        }

        [HttpPost]
        public void Post([FromBody] Computer computer)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                channel.QueueDeclare(queue: "ComputerQueue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "" + computer.ComputerId + ", ComputerBrand: " + computer.ComputerBrand;
                var body = Encoding.UTF8.GetBytes(message);

                // ne sam siguren tuka dali e taka

                var Computer = MessagePackSerializer.Serialize<Computer>(computer);

                channel.BasicPublish(exchange: "",
                                            routingKey: "ComputerQueue",
                                            basicProperties: null,
                                            body: body);
            }
        }

        [HttpPost("Create")]

        public async Task<IActionResult> Create(ComputerRequest request)
        {
            if (request == null)
            {
                return BadRequest(request);
            }

            var position = _mapper.Map<Computer>(request);

            var result = await _computerService.Create(position);

            var response = _mapper.Map<ComputerResponse>(result);

            return Ok(response);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] ComputerRequest request)
        {
            var result = await _computerService.Update(_mapper.Map<Computer>(request));

            if (result == null) return NotFound();

            var computer = _mapper.Map<ComputerResponse>(result);

            return Ok(computer);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _computerService.Delete(id);
            return Ok();
        }

        [HttpGet("GetAll")]

        public async Task<IActionResult> GetAll()
        {
            var result = await _computerService.GetAll();

            if (result == null)
            {
                return NotFound("Collection is empty");
            }

            var computerresult = _mapper.Map<IEnumerable<ComputerResponse>>(result);

            return Ok(computerresult);
        }

    }
}
