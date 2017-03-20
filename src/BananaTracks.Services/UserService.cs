using BananaTracks.Data;
using Microsoft.Azure.Documents;
using System.Threading.Tasks;
using User = BananaTracks.Entities.User;

namespace BananaTracks.Services
{
    public class UserService
    {
        private readonly DocumentRepository<User> _userRepository;

        public UserService(IDocumentClient documentClient)
        {
            _userRepository = new DocumentRepository<User>(documentClient);
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User> AddUser(User user)
        {
            return await _userRepository.CreateItem(user);
        }
    }
}
