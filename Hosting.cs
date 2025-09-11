namespace WebAppApiCs.Models
{
    public class Hosting
    {  
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Port { get; set; }
        public int Rps { get; set; }
    }
}
