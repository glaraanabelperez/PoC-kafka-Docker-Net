using Confluent.Kafka;

namespace KafkaConsumerApi.Services;

public class KafkaBackgroundConsumerService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<KafkaBackgroundConsumerService> _logger;
    private readonly List<string> _mensajes = new();
    private readonly object _lock = new();

    public KafkaBackgroundConsumerService(IConfiguration configuration, ILogger<KafkaBackgroundConsumerService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IReadOnlyList<string> ObtenerMensajes()
    {
        lock (_lock)
        {
            return _mensajes.ToList(); // devolvemos una copia para seguridad
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            Console.WriteLine($"✅ Tópico leído desde configuración: {_configuration["Kafka:Topic"]}");

            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = _configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Latest, // <--- esto es clave
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_configuration["Kafka:Topic"]);

            _logger.LogInformation("✅ Kafka Consumer iniciado...");
            Console.WriteLine("llegamos al sevricio: ");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result != null)
                    {
                        _logger.LogInformation($"📨 Mensaje recibido: {result.Message.Value}");
                        Console.WriteLine("mensaje: " + result.Message);

                        lock (_lock)
                        {
                            _mensajes.Add(result.Message.Value);

                            // mantener solo los últimos 100
                            if (_mensajes.Count > 100)
                                _mensajes.RemoveAt(0);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("🛑 Consumo cancelado");
                consumer.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error: {ex.Message}");
            }
        }, stoppingToken);
    }
}
