namespace coreServices.DTOs.Product
{
    public class UpdateProductDTO
    {
        public Guid ProductId { get; set; }
        public int? Cost { get; set; }
        public string? Name { get; set; }
        public uint? Amount { get; set; }
    }
}
