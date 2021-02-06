using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Domain.Repositories;
using TMS.Library.Interfaces;

namespace TMS.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
