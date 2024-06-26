namespace Project.Models
{
    public record class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
    }
}