using Quartz;
using LoanEnquiryApi.Service;
using Microsoft.EntityFrameworkCore;

namespace LoanEnquiryApi.Jobs
{
    public class ExecuteJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Console.Out.WriteLineAsync($"Job executed at: {DateTime.Now}");

                var options = new DbContextOptions<DataContext>();

                using (var dataContext = new DataContext(options))
                {
                    SoraServices _servicessora = new SoraServices(dataContext);
                    // Call the method to make the API call
                    await _servicessora.GetSoraRateAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
