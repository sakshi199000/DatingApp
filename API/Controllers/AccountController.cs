using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class AccountController (DatingAppDb context, ITokenService tokenService): BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
            if(await(UserExists(registerDto.Username)) )
            return BadRequest("Username is taken");
           using var hmac= new HMACSHA512();
           var user = new AppUser{
            UserName=registerDto.Username.ToLower(),
            PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt= hmac.Key
           };
         context.Add(user);
         await context.SaveChangesAsync();
         return new UserDto{
            Username=user.UserName, 
            Token= tokenService.CreateToken(user)
         };
        }

         [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user= await context.Users.FirstOrDefaultAsync(
                x=> x.UserName.ToLower() ==loginDto.Username.ToLower());
               if(user ==null){
                return Unauthorized("Invalid username");
               }else if(user.PasswordHash.Length==0 ||user.PasswordSalt.Length==0){
                return Unauthorized("Invalid password");
               } 

               
               using var hmac=new HMACSHA512(user.PasswordSalt);
               var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
               for(int i=0;i<computeHash.Length;i++){
                 if(computeHash[i]!=user.PasswordHash[i]){
                    return Unauthorized("Invalid password");
                 }
               }
           return new UserDto{
            Username=user.UserName,
            Token = tokenService.CreateToken(user)
           };
        } 

        private async Task<bool> UserExists(string username){
            return await context.Users.AnyAsync(x=>x.UserName.ToLower()==username.ToLower());
        }
    }
}