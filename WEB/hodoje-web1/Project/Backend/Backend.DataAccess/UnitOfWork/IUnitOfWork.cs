using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess.ModelRepositoryInterfaces;

namespace Backend.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Model repositories        
        ICommentRepository CommentRepository { get; }        
        ILocationRepository LocationRepository { get; }
        IRideRepository RideRepository { get; }        
        IUserRepository UserRepository { get; }
        ICarRepository CarRepository { get; }

        int Complete();
    }
}
