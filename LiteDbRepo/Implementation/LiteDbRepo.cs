using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiteDB;

namespace DbLiteCore.Implementation
{
    public class LiteDbRepo<EntityBaseModel> where EntityBaseModel : LiteDbEntity
    {
        // made the IBaseRepo totaly async as this is 2020 but litedb does not seem to support it so decided to just waste the 
        // state overhead added by the compiler but keep it standardized the kb's lost is minimal in 2020

        protected string DbFilePath;

        public LiteDbRepo(string dbFilePath)
        {
            DbFilePath = dbFilePath;
        }

        public virtual T OpenConnectionAndExecute<T>(Func<LiteDatabase, T> action)
        {
            using (var db = new LiteDatabase(DbFilePath))
            {
                return action.Invoke(db);
            }
        }

        public virtual ILiteCollection<T> GetCollection<T>(LiteDatabase db)
        {
            return db.GetCollection<T>(typeof(T).Name);
        }

        public void Delete<T>(T item) where T : EntityBaseModel
        {
            var b = item as LiteDbEntity;
            OpenConnectionAndExecute(f => GetCollection<T>(f).Delete(b.Id));
        }

        public void Update<T>(T entity) where T : EntityBaseModel
        {
            OpenConnectionAndExecute(f => f.GetCollection<T>().Update(entity));
        }

        public IList<T> GetAll<T>() where T : EntityBaseModel
        {
            return GetAll<T>(f => true);
        }

        public IList<T> GetAll<T>(Expression<Func<T, bool>> query) where T : EntityBaseModel
        {
            return GetAll(query, f => f);
        }

        public IList<Tout> GetAll<T, Tout>(Expression<Func<T, bool>> query, Expression<Func<T, Tout>> projection,
            int? skip = null, int? take = null)
            where T : EntityBaseModel
        {
            return GetAll(query, projection, true);
        }

        public IList<Tout> GetAll<T, Tout>(Expression<Func<T, bool>> query,
            Expression<Func<T, Tout>> projection, bool tracking,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] includes)
            where T : EntityBaseModel
        {
           return OpenConnectionAndExecute(f =>
                {
                    var colectionDetails = GetCollection<T>(f).Query().Where(query);

                    colectionDetails = includes != null && includes.Any()
                        ? includes.Aggregate(colectionDetails, (current, item) => current.Include(item))
                        : colectionDetails;

                    var col = colectionDetails.Select(projection);
                    if (skip != null)
                    {
                        col = col.Skip(skip.Value);
                    }

                    if (take != null)
                    {
                        col = col.Limit(take.Value);
                    }

                    return col.ToArray();
                }
            );
        }

        public T GetFirst<T>(Expression<Func<T, bool>> query) where T : EntityBaseModel
        {
            return OpenConnectionAndExecute(f => GetCollection<T>(f).FindOne(query));
        }

        public void Add<T>(T entity) where T : EntityBaseModel
        {
            OpenConnectionAndExecute(f => GetCollection<T>(f).Insert(entity));
        }

        public void AddMany<T>(IEnumerable<T> entity) where T : EntityBaseModel
        {
            OpenConnectionAndExecute(f => GetCollection<T>(f).InsertBulk(entity));
        }
    }
}