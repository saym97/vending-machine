namespace coreServices.DTOs.Product.Out
{
    public class BuyProductDTO
    {
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public int TotalCost { get; set; }
        public List<int> Change { get; set; }
    }
}
