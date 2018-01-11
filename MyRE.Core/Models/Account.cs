namespace MyRE.Core.Models
{
    public class Account
    {
        public long AccountId { get; set; }
        
        public string RemoteAccountId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
    }
}
