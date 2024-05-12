using FoodForRequest.Data;
using FoodForRequest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodForRequest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngridientController : ControllerBase
    {

        private readonly IFoodRequestRepository foodRepository;
        private readonly IOfferRepository offerRepository;
        private readonly IingridientRepository ingridientRepository;


        private readonly UserManager<FoodUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        ApplicationDbContext context;

        public IngridientController(ApplicationDbContext context, IFoodRequestRepository repository, IOfferRepository offerRepository, IingridientRepository ingrep, UserManager<FoodUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.foodRepository = repository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.ingridientRepository = ingrep;
            this.offerRepository = offerRepository;
            this.context = context;
        }


        // GET: api/<IngridientController>
        [HttpGet("GetAll")]
        public IEnumerable<IngredientViewModel> Get()
        {


            var ingredient = ingridientRepository.GetAll();

            var ingredientInf = new List<IngredientViewModel>();
            foreach (var ing in ingredient)
            {
                ingredientInf.Add(new IngredientViewModel
                {
                    Id = ing.Id,
                    Description = ing.Description,
                    Name = ing.Name,
                    FoodId = ing.FoodId,

                });
            }

            return ingredientInf;
        }

        // GET api/<IngridientController>/5
        [HttpGet("GetOne/{id}")]
        public Ingredient Get(string id)
        {
            return ingridientRepository.GetOne(id);
        }
        /*
        // POST api/<IngridientController>
        [HttpPost("CreateIngredient")]
        public void Post([FromBody] Ingredient value)
        {

            ingridientRepository.Create(value);


        }*/
        [Authorize]
        [HttpPost("CreateIngredient")]
        public void Post([FromBody] IngredientCreateViewModel value)
        {

            Ingredient i = new Ingredient();

            i.Name = value.Name;
            i.Description = value.Description;

            ingridientRepository.Create(i);


        }
        /*
        // PUT api/<IngridientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }
        */
        // DELETE api/<IngridientController>/5
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public void Delete(string id)
        {

            ingridientRepository.Delete(id);

        }
        [Authorize]
        [HttpPut("{requestId}/addIngredients")]
        public async Task<IActionResult> AddIngredientsToRequest(string requestId, [FromBody] List<Ingredient> ingredients)
        {
            var request = await context.Foodrequests.FindAsync(requestId);

            if (request == null)
            {
                return NotFound();
            }

            foreach (var ingredient in ingredients)
            {
                var dbIngredient = await context.Ingredients.FindAsync(ingredient.Id);
                if (dbIngredient != null)
                {
                    request.Ingridients.Add(dbIngredient);
                }
            }

            context.Entry(request).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return NoContent();
        }
        [Authorize]
        [HttpGet("GetIngredientsForRequest/{id}")]
        public IEnumerable<Ingredient> GetOffersForRequest(string id)
        {
            return ingridientRepository.GetIngredientsForRequest(id);
        }

    }
}
