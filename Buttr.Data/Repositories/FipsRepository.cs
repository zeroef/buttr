using System;
using System.Collections.Generic;
using Buttr.Domain;

namespace Buttr.Data.Repositories
{
    public interface IFipsRepository
    {
        IEnumerable<Fips> GetFips();
        IEnumerable<Fips> GetFipsByState(string state);
    }

    public class FipsRepository : BaseRepository, IFipsRepository
    {
        public IEnumerable<Fips> GetFips()
        {
            return CreateContext().Query<Fips>("SELECT * FROM Fips");
        }

        public IEnumerable<Fips> GetFipsByState(string state)
        {
            return CreateContext().Query<Fips>("SELECT * FROM Fips where State = @state", new {state});
        }
    }
}
