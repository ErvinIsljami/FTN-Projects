using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositories;
using Backend.DataAccess.ModelRepositoryInterfaces;
using Unity.Attributes;

namespace Backend.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork() { }

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        // Model repositories
        [Dependency]
        public virtual ICommentRepository CommentRepository { get; set; }
        [Dependency]
        public virtual ILocationRepository LocationRepository { get; set; }
        [Dependency]
        public virtual IRideRepository RideRepository { get; set; }
        [Dependency]
        public virtual IUserRepository UserRepository { get; set; }
        [Dependency]
        public virtual ICarRepository CarRepository { get; set; }

        public virtual int Complete()
        {
            return _context.SaveChanges();
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }
    }
}
