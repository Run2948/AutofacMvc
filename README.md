# �������EF6��Unitwork��Autofac��Repositoryģʽ���
[TOC]

## һ��ʵ�ֵ�˼·�ͽṹͼ

* Repository�Ĺ�ͬ��

> ��һЩ�����ķ���(��ɾ�Ĳ�)�� ��Щ�����޹���Repository���������ĸ�ʵ���࣬���԰���Щ��������ɽӿ�IRepository<TEntity>��Ȼ���и�����BaseRepository<TEntity>ʵ�ָýӿڵķ����������ķ���������Find, Filter, Delete, Create��

* Repository�Ĳ�����

> ÿ��Repository���ֻ���һЩ�����ԣ�Ӧ�����������ܹ��̳�BaseRepository<TEntity>֮�⣬���ܹ�����չ�Լ���һЩ����������ÿ���඼�����ٶ���һ���Լ����еĽӿڣ�����һЩ�����Լ�Repository�ķ�����

* Repository��Эͬ��
> ��ͬ��Repository������ҪЭͬ��Repository�����ݵ��޸ģ���Ҫ��ͳһ�ı���.
> ����ʵ�ֵ���ṹͼ����:

![��ṹͼ](https://images0.cnblogs.com/blog/14408/201309/09181057-1467ca24d7e14f1e86f0b861801666db.png)

## ����Repository��ƾ����ʵ�ִ���

> IRepository<TEntity>�ӿڶ�����Repository���еķ���, BaseRepository<TEntity>ʵ������Щ�ӿڵķ�����������Repository���ټ���BaseRepository<TEntity>����������Ȼ�ĵõ��˶����ݲ����Ļ���������

* IRepository<TEntity>����
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

* BaseRepository<TEntity>����
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
> IUnitOfWork�ӿڶ����˷�����ȡ�ض���Repository, ִ�д洢����, SaveChange�����ύ�޸ģ�ͳһ�������ݡ�

* IUnitOfWork�ӿڴ���:
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

> UnitOfWork����, ������ʹ�õ���Autofac�е�IComponentContext����ȡRepositoryʵ��

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

## ����Repository��Ƶľ����ʹ��

> �������Ƕ���һ��`IStudentRepository`�ӿ�, �����˷���`GetAllStudents()`, ͬʱ�̳���`IRepository<Student>`�ӿ�

```csharp
public interface IStudentRepository : IRepository<Student>
{
    IEnumerable<dynamic> GetAllStudents();
}
```

> ���Ŷ���StudentRepository����ʵ������ӿ�

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

* ��`Application_Start`������ʹ��Autofacע��Repository�Ĵ������£�
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

* �ڿ�������ע��ʹ��Repository�Ĵ�������:

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

    // ͬʱ��Ҳ����ʹ�ö�����IRepository<Student>�еķ����� ����:

    _studentRepository.Delete(students.First());
    _repositoryCenter.SaveChanges();

    ...

    return View(students);
}
```

## �ġ�˼·�ܽ�

> �������ƣ���Repository��ͨ�ô�����뵽�����У�ͬʱ������ÿ��Repository��չ�Լ��ķ������ﵽ�˱Ƚ������״̬��

> ֻ�����ڵ���ƺ�Autofac����ˣ�����������������Autofacֱ��ʹ�� `_repositoryCenter.GetRepository<IStudentRepository>();` �ķ�ʽ��ȡIStudentRepository��ʵ���ͺ������ˡ�

## �塢����Դ��

Դ����ֿ� [AutoFacMvc](https://github.com/Run2948/AutofacMvc)
