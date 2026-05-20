using Aplication.Ports.Out;
using Domain.Models;

namespace Infrastructure.Adapters.Rest;

public class EhrEventPublisherAdapter
    : IEhrEventPublisherPort
{
    public async Task PublishAppointmentAsync(
        Appointment appointment)
    {
        // Simulación de envío asíncrono
        // hacia RabbitMQ o Kafka

        await Task.Delay(1000);

        Console.WriteLine(
            $"Evento enviado al historial clínico: {appointment.Id}");
    }
}