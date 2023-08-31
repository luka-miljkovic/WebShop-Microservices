using DeliveryService.Models;
using EmployeeService.Protos;
using Grpc.Net.Client;

namespace DeliveryService.SyncDataService.Grpc
{
    public class EmployeeDataClient : IEmployeeDataClient
    {
        private readonly IConfiguration _configuration;

        public EmployeeDataClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<Employee> ReturnAllEmployees()
        {
            Console.WriteLine($"---> Calling GRPC Service {_configuration["GrpcPlatform"]}");
            var channel = GrpcChannel.ForAddress("http://employeeservice");
            var client = new GrpcEmployee.GrpcEmployeeClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllEmployees(request);
                var employees = reply.Employee;

                List<Employee> empls = new List<Employee>();
                foreach (var employee in employees)
                {
                    empls.Add(new Employee
                    {
                        EmployeeId = employee.EmployeeId,
                        EmployeeName = employee.Name,
                        Available = employee.Available
                    });
                }

                return empls;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}
