using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace AutoFacMvc.Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        TRepository GetRepository<TRepository>() where TRepository : class;
        void ExecuteProcedure(string procedureCommand, params object[] sqlParams);
        void ExecuteSql(string sql);
        List<T> SqlQuery<T>(string sql);
        void SaveChanges();
    }
}
