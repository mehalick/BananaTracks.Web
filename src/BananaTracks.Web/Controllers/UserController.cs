using BananaTracks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Threading.Tasks;
using User = BananaTracks.Entities.User;

namespace BananaTracks.Web.Controllers
{
    [Route("api/users")]
    public class UserController : ApiBaseController
    {
        private readonly UserService _userService;

        public UserController(IDocumentClient documentClient)
        {
            _userService = new UserService(documentClient);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody]User model)
        {
            var user = await _userService.AddUser(model);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
    }
}