using EmployeeService.Models;

namespace EmployeeService
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<EmployeeDbContext>());
            }
        }

        private static void SeedData(EmployeeDbContext context)
        {

            if (!context.Employees.Any())
            {
                Console.WriteLine("--> Seeding data...");

                context.Employees.AddRange(
                    new Employee
                    {
                        EmployeeName = "Pera",
                        Available = true
                    },
                    new Employee
                    {
                        EmployeeName = "Mika",
                        Available = true
                    },
                    new Employee
                    {
                        EmployeeName = "Zika"
                    },
                    new Employee
                    {
                        EmployeeName = "Laza",
                        Available= true
                    },
                    new Employee
                    {
                        EmployeeName = "Pera"
                    }
                    );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
