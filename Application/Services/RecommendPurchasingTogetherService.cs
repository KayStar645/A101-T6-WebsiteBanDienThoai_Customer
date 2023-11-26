using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.DTOs;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Application.Services
{
    public class RecommendPurchasingTogetherService : IRecommendPurchasingTogetherService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly float _minSupport = 0.3F;
        private readonly int _topK = 10;

        string _scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Algorithm", "FP-Growth.py")
                                 .Replace("View", "Application");

        public RecommendPurchasingTogetherService(IOrderRepository orderRepo) 
        {
            _orderRepo = orderRepo;
        }

        public async Task<List<int>> Get(int pProductId)
        {
            var transaction = await _orderRepo.GetTransactions(pProductId);
            string transactionJson = JsonConvert.SerializeObject(transaction.Select(x => x.ProductIds));
            string arguments = $"{transactionJson} {_minSupport} {_topK}";

            using (Process process = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Users\\thuan\\AppData\\Local\\Programs\\Python\\Python38\\python.exe",
                    Arguments = $"\"{_scriptPath}\" {arguments}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.StartInfo = startInfo;

                StringBuilder outputBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        outputBuilder.AppendLine($"{e.Data}");
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        errorBuilder.AppendLine($"Error: {e.Data}");
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                string output = outputBuilder.ToString();
                string error = errorBuilder.ToString();
                //int code = process.ExitCode;

                if (string.IsNullOrEmpty(output) == false)
                {

                    var result =  JsonConvert.DeserializeObject<List<ItemsetResultDto>>(output);


                    var distinctItems = result.SelectMany(x => x.Itemset)
                                            .Distinct()
                                            .Where(item => item != pProductId)
                                            .ToList();

                    return distinctItems;
                }
                return new List<int>();
            }
        }    
    }
}
