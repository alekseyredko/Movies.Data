using System;

#nullable disable

namespace Movies.Data.Models
{
    public partial class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired { get; set; }
        public bool IsRevoked { get; set; }

        public virtual User User { get; set; }
    }
}
