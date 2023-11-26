using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Python.Runtime;
using System.Diagnostics;
using System.Text;

namespace Application.Services
{
    public class RecommendPurchasingTogetherService : IRecommendPurchasingTogetherService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly float _minSupport = 0.2F;
        private readonly int _topK = 10;
        private string _scriptPath = "D:\\Algorithm\\FP-Growth.py";

        public RecommendPurchasingTogetherService(IOrderRepository orderRepo) 
        {
            _orderRepo = orderRepo;
        }

        public async Task Get(int pProductId)
        {
            string arguments = $"{pProductId} {_minSupport} {_topK}";
            try
            {
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "C:\\Users\\thuan\\AppData\\Local\\Programs\\Python\\Python38\\python.exe",
                        Arguments = $"{_scriptPath} {arguments}",
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
                            outputBuilder.AppendLine($"Output: {e.Data}");
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
                    int code = process.ExitCode;

                    int a = 1;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }    
    }
}
