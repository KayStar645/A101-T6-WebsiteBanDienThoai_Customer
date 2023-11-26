using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Python.Runtime;

namespace Application.Services
{
    public class RecommendPurchasingTogetherService : IRecommendPurchasingTogetherService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly int minSupport = 1;

        public RecommendPurchasingTogetherService(IOrderRepository orderRepo) 
        {
            _orderRepo = orderRepo;
        }

        public async Task Get(int pProductId)
        {

            try
            {
                using (Py.GIL()) // Mở Global Interpreter Lock (GIL)
                {
                    dynamic py = Py.Import("__main__");
                    dynamic module = Py.Import("../../Algorithm/FP-Growth");

                    var transactions = await _orderRepo.GetTransactions(pProductId);

                    string resultJson = module.run_fpgrowth(transactions, 0.2, 3);

                    Console.WriteLine(resultJson);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }    
    }
}
