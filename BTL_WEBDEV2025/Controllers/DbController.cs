using BTL_WEBDEV2025.Data;
using BTL_WEBDEV2025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBDEV2025.Controllers
{
    [Route("db")]
    public class DbController : Controller
    {
        private readonly AppDbContext _db;

        public DbController(AppDbContext db)
        {
            _db = db;
        }

        // GET /db/ping
        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            var canConnect = await _db.Database.CanConnectAsync();
            var productCount = 0;
            if (canConnect)
            {
                try
                {
                    productCount = await _db.Products.CountAsync();
                }
                catch
                {
                    // ignored: table may not exist before migrations
                }
            }

            return Ok(new { canConnect, productCount });
        }

        // POST /db/selftest - verify we can INSERT/DELETE to Users table
        [HttpPost("selftest")]
        public async Task<IActionResult> SelfTest()
        {
            var result = new { canConnect = false, wrote = false, cleaned = false, message = string.Empty };
            try
            {
                var canConnect = await _db.Database.CanConnectAsync();
                if (!canConnect)
                {
                    return Ok(new { canConnect, wrote = false, cleaned = false, message = "Cannot connect to database" });
                }

                // Ensure RoleId 2 exists
                if (!await _db.Roles.AnyAsync(r => r.Id == 2))
                {
                    _db.Roles.Add(new Role { Id = 2, Name = "Customer" });
                    await _db.SaveChangesAsync();
                }

                var email = $"selftest_{Guid.NewGuid():N}@example.com";
                var user = new User { Email = email, PasswordHash = "test", FullName = "Self Test", RoleId = 2 };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                var wrote = user.Id > 0;

                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                return Ok(new { canConnect = true, wrote, cleaned = true, message = "OK" });
            }
            catch (Exception ex)
            {
                return Ok(new { canConnect = true, wrote = false, cleaned = false, message = ex.GetType().Name + ": " + (ex.InnerException?.Message ?? ex.Message) });
            }
        }
    }
}


