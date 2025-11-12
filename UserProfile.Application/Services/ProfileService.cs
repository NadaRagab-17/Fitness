using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProfile.Application.Services
{
    public class ProfileService
    {
        public double? CalculateBmi(double? weightKg, double? heightCm)
        {
            if (!weightKg.HasValue || !heightCm.HasValue || heightCm == 0) return null;
            var h = heightCm.Value / 100.0;
            return Math.Round(weightKg.Value / (h * h), 2);
        }
    }
}
