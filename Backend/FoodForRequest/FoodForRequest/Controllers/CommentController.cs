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

namespace FoodForRequest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {

        private readonly IFoodRequestRepository foodRepository;
        private readonly IOfferRepository offerRepository;
        private readonly ICommentRepository commentRepository;

        private readonly UserManager<FoodUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public CommentController(IFoodRequestRepository repository, IOfferRepository offerRepository, ICommentRepository commentrep, UserManager<FoodUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.foodRepository = repository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.commentRepository = commentrep;
            this.offerRepository = offerRepository;
        }


        // GET: api/<CommentController>
        [HttpGet("GetAll")]
        public IEnumerable<Comment> Get()
        {
            return commentRepository.GetAll();
        }

        // GET api/<CommentController>/5
        [HttpGet("GetOne/{id}")]
        public Comment Get(string id)
        {
            return commentRepository.GetOne(id);
        }

        // POST api/<CommentController>
        

        [HttpPost]
        public void Post( [FromBody] Comment c)
        {
            Comment comment = new Comment()
            {
                RequestId = c.RequestId,
                ContractorId = c.ContractorId,
                Text = c.Text

            };
            commentRepository.Create(comment);
        }
        /*
        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }
        */
        // DELETE api/<CommentController>/5
        

        [HttpDelete("{id}")]
        public void Delete(string id)
        {

            commentRepository.Delete(id);

        }


        //GET api/<WordController>/5
        [HttpGet("GetCommentsForRequest/{id}")]
        public IEnumerable<Comment> GetCommentsForRequest(string id)
        {
            return commentRepository.GetCommentssForRequest(id);
        }

    }
}
