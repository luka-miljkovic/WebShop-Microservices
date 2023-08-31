using DeliveryService.Dtos;
using DeliveryService.Models;
using DeliveryService.SyncDataService.Grpc;
using System.Text.Json;

namespace DeliveryService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public void ProcessEvent(string message)
        {
            Console.WriteLine("--> Order Published event Detected");
            AddDelivery(message);
        }

        private void AddDelivery(string orderPublishedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();

                var grpcService = scope.ServiceProvider.GetRequiredService<IEmployeeDataClient>();

                var orderPublishedDto = JsonSerializer.Deserialize<OrderPublishedDto>(orderPublishedMessage);

                try
                {
                    Delivery delivery = new Delivery()
                    {
                        DeliveryStartTime = DateTime.Now,
                        DeliveryEndTime = DateTime.Now,
                        OrderId = orderPublishedDto.OrderId
                    };

                    var employees = grpcService.ReturnAllEmployees();
                    foreach (var employee in employees)
                    {
                        if (employee.Available)
                        {
                            delivery.EmployeeId = employee.EmployeeId;
                        }
                    }

                    repo.Deliveries.Add(delivery);
                    repo.SaveChanges();

                    Console.WriteLine("--->   New delivery is created...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---> Could not add Delivery to DB: {ex.Message}");
                }
            }
        }
    }
}
