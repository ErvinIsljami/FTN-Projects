using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositories
{
    public class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        protected DatabaseContext DatabaseContext
        {
            get { return _context as DatabaseContext; }
        }

        public CommentRepository(DbContext context) : base(context) { }
        public Comment GetByIdIncludeUser(int id)
        {
            return _entities.Where(c => c.Id == id).Include(c => c.User).FirstOrDefault();
        }

        public Comment FindCommentIncludeUser(Expression<Func<Comment, bool>> predicate)
        {
            return _entities.Where(predicate).Include(c => c.User).FirstOrDefault();
        }

        public IEnumerable<Comment> GetAllByIdIncludeUser(int id)
        {
            return _entities.Where(c => c.Id == id).Include(c => c.User);
        }

        public IEnumerable<Comment> FindAllCommentsIncludeUser(Expression<Func<Comment, bool>> predicate)
        {
            return _entities.Where(predicate).Include(c => c.User);
        }
    }
}
