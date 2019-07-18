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
        protected readonly DbContext Context;

        public UnitOfWork(DbContext context, IComponentContext componentContext)
        {
            Context = context;
            _componentContext = componentContext;
        }

        public DbContext DbContext => Context;

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _componentContext.Resolve<TRepository>();
        }

        public void ExecuteProcedure(string procedureCommand, params object[] sqlParams)
        {
            Context.Database.ExecuteSqlCommand(procedureCommand, sqlParams);
        }

        public void ExecuteSql(string sql)
        {
            Context.Database.ExecuteSqlCommand(sql);
        }

        public List<T> SqlQuery<T>(string sql)
        {
            return Context.Database.SqlQuery<T>(sql).ToList();
        }

        public void SaveChanges()
        {
            try
            {
                Context.SaveChanges();
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
            Context?.Dispose();
        }
    }
}
