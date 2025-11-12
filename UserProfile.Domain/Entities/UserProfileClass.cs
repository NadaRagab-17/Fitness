using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProfile.Domain.Entities
{
    public class UserProfileClass
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string? Goal { get; set; }
        public string? ActivityLevel { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
