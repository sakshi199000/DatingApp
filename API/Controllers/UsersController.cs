using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(DatingAppDb context) : ControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
           return await context.Users.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUsers(int id){
           var user=await context.Users.FindAsync(id);
           if(user==null){
            return NotFound();
           }
           return user;
        }
    }
}
