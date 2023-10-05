using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Contratos
{
    public interface IUserPersist : IGeralPersist
    {
        Task<IEnumerable<User>> GetUsersASync();
        Task<User> GetUsersByIdASync(int id);
        Task<User> GetUsersByNameASync(string username);
        
    }
}