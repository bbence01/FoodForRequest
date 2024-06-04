using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodForRequest.Data;
using FoodForRequest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using System.Numerics;
using FoodForRequest.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WordQuiz.Controllers
{

    // [Authorize(Roles = "FoodUser, Admin")]
    [Route("[controller]")]
    [ApiController]
    // 
    public class FoodUserController : ControllerBase
    {
        IFoodUserRepository fooduserRepository;

        private readonly UserManager<FoodUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        IHubContext<SignalRHub> hub;

        public FoodUserController(IHubContext<SignalRHub> hub,IFoodUserRepository foodUserRepository, UserManager<FoodUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.fooduserRepository = foodUserRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.hub = hub;

        }

        public IFoodUserRepository FoodUser { get => fooduserRepository; set => fooduserRepository = value; }

        public UserManager<FoodUser> UserManager => userManager;

        public RoleManager<IdentityRole> RoleManager => roleManager;
        /*
        // GET: api/<PlayerController>/all
        [HttpGet("all")]
        public IEnumerable<FoodUser> GetAllFoodusers()
        {
            return fooduserRepository.GetAllFoodusers().Result;
        }*/

        [HttpGet("all")]
        public async Task<IEnumerable<FooduserInfoViewModel>> GetAllFooduser()
        {
            var foodusers = fooduserRepository.GetAllFoodusers();

            var playerInfos = new List<FooduserInfoViewModel>();
            foreach (var fooduser in foodusers)
            {
                playerInfos.Add(new FooduserInfoViewModel
                {
                    Id = fooduser.Id,
                    Email = fooduser.Email,                   
                    UserName = fooduser.UserName,
                    FoodUserName = fooduser.FoodUserName,
                    Founds = fooduser.Founds,
                    IsAdmin = await userManager.IsInRoleAsync(fooduser, "Admin")


                });
            }

            return playerInfos;
        }


        [HttpGet("{id}")]
        public async Task<object> GetFooduser(string id)
        {
            FoodUser p = fooduserRepository.GetFooduserById(id);
            return new FooduserInfoViewModel
            {
                Id = p.Id,
                Email = p.Email,
                UserName = p.UserName,
                FoodUserName = p.FoodUserName,
                Founds = p.Founds,
                IsAdmin = await userManager.IsInRoleAsync(p, "Admin")
            };
        }

        [HttpGet]
        
        public async Task<object> GetFooduser()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            System.Diagnostics.Debug.WriteLine("Authorization Header: " + authHeader); // Log the Authorization header

            FoodUser p = (await userManager.GetUserAsync(User));
            System.Diagnostics.Debug.WriteLine("User: " + p); // Log the User object

            //return p;

            return new
            {
                email = p.Email,
                id = p.Id,
                foodUserName = p.FoodUserName,
                userName = p.UserName,
                admin = await this.userManager.IsInRoleAsync(p, "Admin")
            };
        }


        //// POST api/<PlayerController>
        //[HttpPost]
        //public async void AddPlayer([FromBody] FoodUser value)
        //{
        //     fooduserRepository.CreateFooduser(value);

        //}

        // PUT api/<PlayerController>/5
        

        [HttpPut("{id}")]
        public async Task<IActionResult> EditFooduser(int id, [FromBody] FoodUser value)
        {
            fooduserRepository.UpdateFooduser(value);
            return Ok();
        }


        

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFooduser(string id)
        {
            fooduserRepository.DeleteFooduser(id);
            return Ok();
        }
    }
}
