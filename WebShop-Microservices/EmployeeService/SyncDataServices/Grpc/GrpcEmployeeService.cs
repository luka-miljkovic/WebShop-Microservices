using EmployeeService.Protos;
using Grpc.Core;
using static EmployeeService.Protos.GrpcEmployee;

namespace EmployeeService.SyncDataServices.Grpc
{
    public class GrpcEmployeeService : GrpcEmployee.GrpcEmployeeBase
    {
        private readonly EmployeeDbContext _dbContext;

        public GrpcEmployeeService(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override Task<EmployeeResponse> GetAllEmployees(GetAllRequest request, ServerCallContext context)
        {
            var response =new EmployeeResponse();
            var employees = _dbContext.Employees.ToList();

            foreach (var employee in employees)
            {
                response.Employee.Add(new GrpcEmployeeModel
                {
                    EmployeeId = employee.EmployeeId,
                    Name = employee.EmployeeName,
                    Available = employee.Available
                });
            }

            return Task.FromResult(response);
        }
    }
}
