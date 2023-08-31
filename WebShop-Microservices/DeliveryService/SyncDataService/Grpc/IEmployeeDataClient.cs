using DeliveryService.Models;

namespace DeliveryService.SyncDataService.Grpc
{
    public interface IEmployeeDataClient
    {
        IEnumerable<Employee> ReturnAllEmployees();
    }
}
