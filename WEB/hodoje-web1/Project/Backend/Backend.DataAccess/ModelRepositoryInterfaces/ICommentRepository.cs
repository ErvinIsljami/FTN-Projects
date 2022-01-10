using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DomainEntities.Models;

namespace Backend.DataAccess.ModelRepositoryInterfaces
{
    public interface ICommentRepository : IRepository<Comment, int>
    {
        Comment GetByIdIncludeUser(int id);
        Comment FindCommentIncludeUser(Expression<Func<Comment, bool>> predicate);
        IEnumerable<Comment> GetAllByIdIncludeUser(int id);
        IEnumerable<Comment> FindAllCommentsIncludeUser(Expression<Func<Comment, bool>> predicate);
    }
}
