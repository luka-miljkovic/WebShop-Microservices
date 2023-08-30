using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewOrder(OrderPublishedDto order);
    }
}
