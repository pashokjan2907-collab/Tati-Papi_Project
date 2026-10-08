using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TatiPapi.Api.Models;
using TatiPapi.Api.Repositories;

namespace TatiPapi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {
        private readonly IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository) {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user) {
            if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.PassHash))
                return BadRequest(new { Error = "Логин и пароль обязательны." });

            user.PassHash = HashPassword(user.PassHash);
            await _userRepository.AddAsync(user);
            return Ok(new { Message = "Пользователь Tati-Papi успешно создан." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User updatedUser) {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.Login = updatedUser.Login;
            if (!string.IsNullOrWhiteSpace(updatedUser.PassHash)) {
                user.PassHash = HashPassword(updatedUser.PassHash);
            }
            
            await _userRepository.UpdateAsync(user);
            return Ok(new { Message = "Данные обновлены." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            await _userRepository.DeleteAsync(id);
            return Ok(new { Message = "Пользователь удален." });
        }

        private string HashPassword(string password) {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(bytes);
        }
    }
}