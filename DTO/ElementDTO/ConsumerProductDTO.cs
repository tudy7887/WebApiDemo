namespace Learning.DTO.ElementDTO
{
    public class ConsumerProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public string ProducerId { get; set; }
        public string ProducerFirstName { get; set; }
        public string ProducerLastName { get; set; }
        public string ProducerProfilePicture { get; set; }
        public ICollection<ProductImageDTO> Pictures { get; set; }
    }
}
