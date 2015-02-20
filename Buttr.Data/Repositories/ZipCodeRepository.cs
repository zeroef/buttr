using System.Collections.Generic;
using Buttr.Domain;

namespace Buttr.Data.Repositories
{
    public interface IZipCodeRepository
    {
        IEnumerable<ZipCode> GetZipCodes();
        IEnumerable<ZipCode> GetZipCodesByFipsCode(string fipsCode);
    }

    public class ZipCodeRepository : BaseRepository, IZipCodeRepository
    {
        public IEnumerable<ZipCode> GetZipCodes()
        {
            return CreateContext().Query<ZipCode>("SELECT * FROM ZipCodes");
        }

        public IEnumerable<ZipCode> GetZipCodesByFipsCode(string fipsCode)
        {
            return CreateContext().Query<ZipCode>("SELECT * FROM ZipCodes where FipsCode = @fipsCode", new { fipsCode });
        }

    }
}
