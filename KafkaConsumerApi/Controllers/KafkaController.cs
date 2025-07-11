using KafkaConsumerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace KafkaConsumerApi.Controllers;

[ApiController]
[Route("kafka")]
public class KafkaController : ControllerBase
{
    private readonly KafkaBackgroundConsumerService _consumer;

    public KafkaController(KafkaBackgroundConsumerService consumer)
    {
        _consumer = consumer;
    }

    [HttpGet("messages")]
    public IActionResult ObtenerMensajes()
    {
        var mensajes = _consumer.ObtenerMensajes();
        return Ok(mensajes);
    }
}
