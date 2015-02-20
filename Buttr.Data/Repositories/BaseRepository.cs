﻿using System.Configuration;

namespace Buttr.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected DbContext CreateContext()
        {
            return new DbContext("Buttr");
        }
    }
}