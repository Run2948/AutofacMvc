using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Autofac;

namespace AutoFacMvc.Repository.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IComponentContext _componentContext;

        public UnitOfWork(DbContext context, IComponentContext componentContext)
        {
            DbContext = context;
            _componentContext = componentContext;
        }

        public DbContext DbContext { get; }

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _componentContext.Resolve<TRepository>();
        }

        public void ExecuteProcedure(string procedureCommand, params object[] sqlParams)
        {
            DbContext.Database.ExecuteSqlCommand(procedureCommand, sqlParams);
        }

        public void ExecuteSql(string sql)
        {
            DbContext.Database.ExecuteSqlCommand(sql);
        }

        public List<T> SqlQuery<T>(string sql)
        {
            return DbContext.Database.SqlQuery<T>(sql).ToList();
        }

        public void SaveChanges()
        {
            try
            {
                DbContext.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                if (!ex.Message.Contains("The changes to the database were committed successfully"))
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
