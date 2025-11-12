using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProfile.Application.DTOs
{
    public class ProfileDto
    {
        public Guid UserId { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string? Goal { get; set; }
        public string? ActivityLevel { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? Bmi { get; set; }
    }
}
