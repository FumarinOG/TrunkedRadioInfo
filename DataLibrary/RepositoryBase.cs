using AutoMapper;
using DataAccessLibrary;
using NLog;
using System.Collections.Generic;

namespace DataLibrary
{
    public abstract class RepositoryBase
    {
        protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        protected readonly IMapper _mapperConfig;

        protected RepositoryBase() => _mapperConfig = Config.AutoMapperSetup();

        public virtual TrunkedRadioInfoEntities CreateEntities()
        {
            var dataEntities = new TrunkedRadioInfoEntities();

            dataEntities.Database.CommandTimeout = 3600;
            return dataEntities;
        }

        public List<T> CreateList<T>() where T : new() => new List<T>();
    }
}
