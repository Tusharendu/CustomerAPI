namespace CustomerApi.Models
{
    public class Customer
    {
        public int Id { get; set; } // Unique ID
        public int CustomerNumber { get; set; } // Unique customer number
        public string CustomerName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
}
