using Microsoft.AspNetCore.Mvc;

using Room.Me.Data;
using System;

namespace Room.Me.Controllers
{
    public class Access : Controller
    {
        private readonly RoomMeDbContext _context;

        public Access(RoomMeDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}
