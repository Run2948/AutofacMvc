# 分享基于EF6、Unitwork、Autofac的Repository模式设计
[TOC]

## 一、实现的思路和结构图

* Repository的共同性

> 有一些公共的方法(增删改查)， 这些方法无关于Repository操作的是哪个实体类，可以把这些方法定义成接口IRepository<TEntity>，然后有个基类BaseRepository<TEntity>实现该接口的方法。常见的方法，比如Find, Filter, Delete, Create等

* Repository的差异性

> 每个Repository类又会有一些差异性，应当允许它们能够继承BaseRepository<TEntity>之外，还能够再扩展自己的一些方法。所以每个类都可以再定义一个自己特有的接口，定义一些属于自己Repository的方法。

* Repository的协同性
> 不同的Repository可能需要协同，Repository对数据的修改，需要在统一的保存.
> 最终实现的类结构图如下:

![类结构图](https://images0.cnblogs.com/blog/14408/201309/09181057-1467ca24d7e14f1e86f0b861801666db.png)

## 二、Repository设计具体的实现代码

> IRepository<TEntity>接口定义了Repository共有的方法, BaseRepository<TEntity>实现了这些接口的方法。其它的Repository类再集成BaseRepository<TEntity>方法，就天然的得到了对数据操作的基本方法。

* IRepository<TEntity>代码
```csharp
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets all objects from database
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> All();

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <returns></returns>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets objects from database with filtering and paging.
        /// </summary>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <returns></returns>
        bool Contains(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        /// <returns></returns>
        TEntity Find(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        /// <returns></returns>
        void Create(TEntity t);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>
        void Delete(TEntity t);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        /// <returns></returns>
        void Update(TEntity t);

        /// <summary>
        /// Select Single Item by specified expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression);
    }
```

* BaseRepository<TEntity>代码
```csharp
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public BaseRepository(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Gets all objects from database
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> All()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where<TEntity>(predicate).AsQueryable<TEntity>();
        }

        /// <summary>
        /// Gets objects from database with filtering and paging.
        /// </summary>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0,
            int size = 50)
        {
            var skipCount = index * size;
            var resetSet = filter != null
                ? Context.Set<TEntity>().Where<TEntity>(filter).AsQueryable()
                : Context.Set<TEntity>().AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <returns></returns>
        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Any(predicate);
        }

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        /// <returns></returns>
        public virtual TEntity Find(params object[] keys)
        {
            return Context.Set<TEntity>().Find(keys);
        }

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault<TEntity>(predicate);
        }

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        /// <returns></returns>
        public virtual void Create(TEntity t)
        {
            Context.Set<TEntity>().Add(t);
        }

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>
        public virtual void Delete(TEntity t)
        {
            Context.Set<TEntity>().Remove(t);
        }

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                Context.Set<TEntity>().Remove(obj);
            return Context.SaveChanges();
        }

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        /// <returns></returns>
        public virtual void Update(TEntity t)
        {
            try
            {
                var entry = Context.Entry(t);
                Context.Set<TEntity>().Attach(t);
                entry.State = EntityState.Modified;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Select Single Item by specified expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return All().FirstOrDefault(expression);
        }
    }
```
> IUnitOfWork接口定义了方法获取特定的Repository, 执行存储过程, SaveChange方法提交修改，统一更新数据。

* IUnitOfWork接口代码:
```csharp
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        TRepository GetRepository<TRepository>() where TRepository : class;
        void ExecuteProcedure(string procedureCommand, params object[] sqlParams);
        void ExecuteSql(string sql);
        List<T> SqlQuery<T>(string sql);
        void SaveChanges();
    }
```

> UnitOfWork代码, 代码中使用到了Autofac中的IComponentContext来获取Repository实例

```csharp
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
```

## 三、Repository设计的具体的使用

> 这里我们定义一个`IStudentRepository`接口, 包含了方法`GetAllStudents()`, 同时继承于`IRepository<Student>`接口

```csharp
public interface IStudentRepository : IRepository<Student>
{
    IEnumerable<dynamic> GetAllStudents();
}
```

> 接着定义StudentRepository类来实现这个接口

```csharp
public class StudentRepository : BaseRepository<Student>, IStudentRepository
{
    private readonly SchoolContext _context;

    public StudentRepository(SchoolContext context)
        : base(context)
    {
        _context = context;
    }

    public IEnumerable<dynamic> GetAllStudents()
    {
        return _context.Students;
    }
}
```

* 在`Application_Start`方法中使用Autofac注册Repository的代码如下：
```csharp
    var builder = new ContainerBuilder();

    //register controllers
    builder.RegisterControllers(typeof(MvcApplication).Assembly);

    //register repository
    builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

    //add the Entity Framework context to make sure only one context per request
    builder.RegisterType<SchoolContext>().InstancePerRequest();
    builder.Register(c => c.Resolve<SchoolContext>()).As<DbContext>().InstancePerRequest();

    var container = builder.Build();
    DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
```

* 在控制器中注入使用Repository的代码如下:

```csharp
private readonly IUnitOfWork _repositoryCenter;

private readonly IStudentRepository _studentRepository;

public HomeController(IUnitOfWork repositoryCenter)
{
    _repositoryCenter = repositoryCenter;
    _studentRepository = _repositoryCenter.GetRepository<IStudentRepository>();
}

public ActionResult Index(Student sessionStudent)
{
    var students = _studentRepository.GetAllStudents();

    // 同时你也可以使用定义于IRepository<Student>中的方法， 比如:

    _studentRepository.Delete(students.First());
    _repositoryCenter.SaveChanges();

    ...

    return View(students);
}
```

## 四、思路总结

> 上面的设计，把Repository的通用代码剥离到父类中，同时又允许每个Repository扩展自己的方法，达到了比较理想的状态。

> 只是现在的设计和Autofac耦合了，但是如果想继续剥离Autofac直接使用 `_repositoryCenter.GetRepository<IStudentRepository>();` 的方式获取IStudentRepository的实例就很困难了。

## 五、案例源码

源代码仓库 [AutoFacMvc](https://github.com/Run2948/AutofacMvc)
