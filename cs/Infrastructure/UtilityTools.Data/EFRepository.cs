using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UtilityTools.Data
{
    public class EFRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields

        private readonly IDbContext _context;
        private DbSet<T> _entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EFRepository(IDbContext context)
        {
            this._context = context;
        }

        #endregion

        //#region Utilities

        ///// <summary>
        ///// Get full error
        ///// </summary>
        ///// <param name="exc">Exception</param>
        ///// <returns>Error</returns>
        //protected string GetFullErrorText(DbEntityValidationException exc)
        //{
        //    var msg = string.Empty;
        //    foreach (var validationErrors in exc.EntityValidationErrors)
        //        foreach (var error in validationErrors.ValidationErrors)
        //            msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
        //    return msg;
        //}

        //#endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }


        //public IList<T> SqlQuery(string commandString, params object[] args)
        //{
        //    return _context.SqlQuery<T>(commandString, args).ToList();
        //}

        //public int ExcuteNullQuery(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        //{
        //    return _context.ExecuteSqlCommand(sql, doNotEnsureTransaction, timeout, parameters);
        //}
        #endregion

        //public int ExcuteSql(string sql, object param = null)
        //{
        //    return ExcuteNullQuery(sql, false, null, null);
        //}

        public T Find(Expression<Func<T, bool>> func, object param = null)
        {
            return Table.FirstOrDefault(func);
        }

        public T Find(string sql, object param = null)
        {
            throw new NotImplementedException();
        }

        public IList<T> FindList(Expression<Func<T, bool>> func, object param = null)
        {
            return Table.Where(func).ToList();
        }

        public IList<T> FindList(string sql, object param = null)
        {
            throw new NotImplementedException();
        }

        public IList<T> Get()
        {
            throw new NotImplementedException();
        }
        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);

                this._context.SaveChanges();
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public int Insert(DbConnection conn, T data)
        {
            throw new NotImplementedException();
        }

        public virtual async Task InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                await this.Entities.AddAsync(entity);

                await this._context.SaveChangesAsync();
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public int Save(T entity)
        {

            throw new NotImplementedException();
        }

        public EntityEntry<TEntity> Attach<TEntity>(TEntity t) where TEntity : BaseEntity
        {
            return _context.Set<TEntity>().Attach(t);
        }

        public int Save(DbConnection conn, T data)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this._context.SaveChanges();
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public int Update(DbConnection conn, T data)
        {
            throw new NotImplementedException();
        }

        public T GetById(object id)
        {
            throw new NotImplementedException();
        }


        public void Update(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            this.Entities.Remove(entity);

            this._context.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this.Entities.Remove(entity);

                this._context.SaveChanges();
            }
            catch (Exception )
            {
                throw ;
            }
        }
    }
}
