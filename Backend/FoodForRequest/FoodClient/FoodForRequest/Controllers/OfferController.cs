using FoodForRequest.Data;
using FoodForRequest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FoodForRequest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace FoodForRequest.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OfferController : Controller
    {
        IFoodRequestRepository requestRepo;
        IOfferRepository offerRepo;
        private readonly UserManager<FoodUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public OfferController(IFoodRequestRepository foodRepo, IOfferRepository offerRepo, UserManager<FoodUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.requestRepo = foodRepo;
            this.offerRepo = offerRepo;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }



         //GET: api/<WordController>
        [HttpGet("all")]
        public IEnumerable<Offer> GetAllOffer()
        {
            return  offerRepo.GetAll();
          }



        [HttpGet("GetOffersForRequest/{id}")]
        public IEnumerable<Offer> GetOffersForRequest( string id)
        {
            return offerRepo.GetOffersForRequest(id);
        }

        //GET api/<WordController>/5
        [HttpGet("{id}")]
        public Offer? GetOffer(string id)
        {
            return offerRepo.GetOne(id);
        }



        

        // POST api/<WordController>
        [HttpPost]
        public async void AddOffer([FromBody] Offer value)
        {
            Offer offer = new Offer()
            {
                FoodId = value.FoodId,
                ContractorId = value.ContractorId,
                Choosen = false

            };

            offerRepo.Create(offer);

          

        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffer(string id)
        {
            offerRepo.Delete(id);
            return Ok();

        }

        /*
        // PUT api/<WordController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditWord(int id, [FromBody] Offer value)
        {

            offerRepo.UpdateWord(value);
            return Ok();
        }*/

        // DELETE api/<WordController>/5

        /*
       */

        //GET api/<WordController>/5
        /*
        [HttpGet("GetOffersForRequest/{id}")]
        public IEnumerable<Offer> GetOffersForRequest(string id)
        {
            return offerRepo.GetOffersForRequest(id);
        }




        
        [HttpGet]
        public IActionResult GetOne(string requestId)
        {
            var foodrequest = this.requestRepo.GetOne(requestId);

            if (foodrequest != null)
            {
                OfferViewModel vm = new OfferViewModel();
                vm.FoodId = requestId;
                vm.Choosen = false;

                return Ok();
            }
            else
            {
                return Ok();
            }
        }

        
        [HttpPost]
        public IActionResult Create(OfferViewModel newOffer)
        {
            FoodRequest f = this.requestRepo.GetOne(newOffer.FoodId);

            if (ModelState.IsValid)
            {
                Offer offer = new Offer()
                {

                    FoodId = newOffer.FoodId,
                    ContractorId = userManager.GetUserId(User)
                };

                this.offerRepo.Create(offer);

                return Ok();
            }
            else { return Ok(); }
        }
        */
    }
}
