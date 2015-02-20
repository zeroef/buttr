using Buttr.Domain.Enumeration;

namespace Buttr.Domain
{
    public class Fips
    {
        public int FipsId { get; set; }
        public string State { get; set; }
        public string StateFips { get; set; }
        public string CountyFips { get; set; }
        public string County { get; set; }
        public FipsClassCode FipsClassCode { get; set; }
    }
}
