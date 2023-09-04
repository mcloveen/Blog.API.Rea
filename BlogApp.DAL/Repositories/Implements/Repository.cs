using System;
using System.Linq.Expressions;
using BlogApp.Core.Entities.Commons;
using BlogApp.DAL.Contexts;
using BlogApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DAL.Repositories.Implements
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
    {
        readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public async Task CreateAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            Table.Remove(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await FindByIdAsync(id);
            Table.Remove(entity);
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = Table.AsQueryable();
            return _getIncludes(query,includes).Where(expression);
        }

        public async Task<TEntity> FindByIdAsync(int id, params string[] includes)
        {
            if(includes.Length==0)
            {
                return await Table.FindAsync(id);
            }
            var query = Table.AsQueryable();
            return await _getIncludes(query,includes).SingleOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<TEntity> GetAll(params string[] includes)
        {
            var query = Table.AsQueryable();
            return _getIncludes(query,includes);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = Table.AsQueryable();
            return await _getIncludes(query,includes).SingleOrDefaultAsync(expression);
        }

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Table.AnyAsync(expression);
        }

        public void RevertSoftDelete(TEntity entity)
        {
            entity.IsDeleted = false;
        }

        public void SoftDelete(TEntity entity)
        {
            entity.IsDeleted = true;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        IQueryable<TEntity> _getIncludes(IQueryable<TEntity> query, params string[] includes)
        {
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return query;
        }
    }
}

