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
        private readonly DbContext _context;

        public UnitOfWork(DbContext context, IComponentContext componentContext)
        {
            _context = context;
            _componentContext = componentContext;
        }

        public DbContext DbContext => _context;

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _componentContext.Resolve<TRepository>();
        }

        public void ExecuteProcedure(string procedureCommand, params object[] sqlParams)
        {
            _context.Database.ExecuteSqlCommand(procedureCommand, sqlParams);
        }

        public void ExecuteSql(string sql)
        {
            _context.Database.ExecuteSqlCommand(sql);
        }

        public List<T> SqlQuery<T>(string sql)
        {
            return _context.Database.SqlQuery<T>(sql).ToList();
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
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
            _context?.Dispose();
        }
    }
}
