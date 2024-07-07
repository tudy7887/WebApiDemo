using Learning.Model;
using System.ComponentModel.DataAnnotations;

namespace Learning.DTO.ElementDTO
{
    public class AdressDTO
    {
        public string Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }
}
