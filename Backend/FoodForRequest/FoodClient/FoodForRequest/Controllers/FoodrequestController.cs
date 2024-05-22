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
using FoodForRequest.Migrations;
using Microsoft.AspNetCore.SignalR;
using FoodForRequest.Services;
using Newtonsoft.Json.Linq;

namespace FoodForRequest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodrequestController : Controller
    {
        private readonly IFoodRequestRepository foodrepository;
        private readonly IOfferRepository offerrepository;
        private readonly ICommentRepository commentRepository;
        private readonly IFoodUserRepository foodUserRepository;
        private readonly IingridientRepository ingredientRepository;

        IHubContext<SignalRHub> hub;


        private readonly UserManager<FoodUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public FoodrequestController(IHubContext<SignalRHub> hub,IingridientRepository ingredientRepository,IFoodUserRepository foodUserRepository, IFoodRequestRepository repository, IOfferRepository offerRepository, ICommentRepository commentrep, UserManager<FoodUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.foodrepository = repository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.commentRepository = commentrep;
            this.offerrepository = offerRepository;
            this.foodUserRepository = foodUserRepository;
            this.ingredientRepository = ingredientRepository;
            this.hub = hub;
        }


        [Authorize]
        [HttpGet("Search")]
        public IEnumerable<FoodRequestViewModel> Search(string query)
        {

            var id = userManager.GetUserId(User);

            var requestors = foodrepository.GetAll();

            var rInfos = new List<FoodRequestViewModel>();

            foreach (var r in requestors)
            {
                if (r.Description.Contains(query) || r.Name.Contains(query))
                {
                    rInfos.Add(new FoodRequestViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        Payment = r.Payment,
                        IsDone = r.IsDone,
                        InProgress = r.InProgress,
                        Deliveryoptions = r.Deliveryoptions,
                        RequestorId = r.RequestorId,
                        PictureURL = r.PictureURL


                    });
                }
               
            }

            return rInfos;
        }

        [HttpGet("FilterByIngredients")]
        public IEnumerable<FoodRequestViewModel> FilterByIngredients([FromQuery] string ingredients)
        {
            var ingredientList = ingredients.Split(',').Select(i => i.Trim()).ToList();
            var foodRequests = foodrepository.GetAll();
            var filteredFoodRequests = new List<FoodRequestViewModel>();

            var uid = userManager.GetUserId(User);


            foreach (var foodRequest in foodRequests)
            {
                var requestIngredients = ingredientRepository.GetIngredientsForRequest(foodRequest.Id).Select(i => i.Name).ToList();
                if (ingredientList.All(ingredient => requestIngredients.Contains(ingredient)))
                {
                    filteredFoodRequests.Add(new FoodRequestViewModel
                    {
                        Id = foodRequest.Id,
                        Name = foodRequest.Name,
                        Description = foodRequest.Description,
                        Payment = foodRequest.Payment,
                        IsDone = foodRequest.IsDone,
                        InProgress = foodRequest.InProgress,
                        Deliveryoptions = foodRequest.Deliveryoptions,
                        RequestorId = foodRequest.RequestorId,
                        PictureURL = foodRequest.PictureURL
                    });
                }
            }

            return filteredFoodRequests;
        }

        [HttpGet("GetAll")]
        public IEnumerable<FoodRequestViewModel> GetAllProduct()
        {

            var requestors = foodrepository.GetAll();

            var rInfos = new List<FoodRequestViewModel>();
            foreach (var r in requestors)
            {
                rInfos.Add(new FoodRequestViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Payment = r.Payment,
                    IsDone = r.IsDone,
                    InProgress = r.InProgress,
                    Deliveryoptions = r.Deliveryoptions,
                    RequestorId = r.RequestorId,
                    PictureURL = r.PictureURL


                });
            }

            return rInfos;
        }

        
        [HttpGet("{id}")]
        public FoodRequestViewModel? GetFood(string id)
        {

            var uid = userManager.GetUserId(User);


            var rInfos = new FoodRequestViewModel();
            var r = foodrepository.GetOne(id);

            rInfos = (new FoodRequestViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Payment = r.Payment,
                IsDone = r.IsDone,
                InProgress = r.InProgress,
                Deliveryoptions = r.Deliveryoptions,
                RequestorId = r.RequestorId,
                PictureURL = r.PictureURL


            });



            return rInfos;
        }

        
        [HttpPost("CreateFd")]
        public IActionResult Create([FromBody] FoodrequestCreateViewmodel food /*IFormFile picture*/)
        {
            if (ModelState.IsValid)
            {
                FoodRequest f = new FoodRequest()
                {

                    Name = food.Name,
                    Description = food.Description,
                    Payment = food.Payment,
                    Deliveryoptions = food.Deliveryoptions,
                    RequestorId = userManager.GetUserId(User),
                    PictureURL = food.PictureURL


                };
                /* using (var stream = picture.OpenReadStream())
                 {
                     byte[] buffer = new byte[picture.Length];
                     stream.Read(buffer, 0, (int)picture.Length);
                     f.Picture = buffer;
                     f.PictureContentType = picture.ContentType;
                 }*/

                FoodRequest newr = this.foodrepository.Create(f);


              

                foreach (var item in food.Ingredients)
                {
                    Ingredient i = ingredientRepository.GetOne(item);

                    if (i == null)
                    {
                        i =
                             new Ingredient
                             {
                                 Name = item,
                                 Description = item,
                              
                             };

                    }


                    Ingredient newi = new Ingredient();
                    newi.Name = i.Name;
                    newi.Description = i.Name;

                    var foodlist = foodrepository.GetAll();

                    foreach (var fooditem in foodlist)
                    {
                        if (foodrepository.GetOneName(f.Name).Description == f.Description && foodrepository.GetOneName(f.Name).RequestorId == f.RequestorId)
                        {
                            newi.FoodId = newr.Id;
                        }


                    }




                    ingredientRepository.Create(newi);



                }

                this.hub.Clients.All.SendAsync("FoodRequestCreateFd", f);

                return Ok();
            }

            return BadRequest(ModelState); // Model was not valid
        }



        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] FoodrequestCreateViewmodel food)
        {
            if (ModelState.IsValid)
            {
                FoodRequest f = foodrepository.GetOne(id); // Retrieve existing food request
                if (f == null)
                {
                    return NotFound(); // No food request with this id
                }

               // if (userManager.GetUserId(User) == f.RequestorId)
                {
                    // Update properties
                    f.Name = food.Name;
                    f.Description = food.Description;
                    f.Payment = food.Payment;
                    f.Deliveryoptions = food.Deliveryoptions;
                   
                    f.PictureURL = food.PictureURL;

                    // Update the ingredients
                    var ingredients = ingredientRepository.GetAll();
                    foreach (var item in ingredients)
                    {
                        if (item.FoodId == f.Id)
                        {
                            item.FoodId = null;
                        }
                    }

                    foreach (var item in food.Ingredients)
                    {
                        Ingredient i = ingredientRepository.GetOne(item);
                        if (i== null)
                        {
                            i =
                                 new Ingredient
                                 {
                                     Name = item,
                                     Description = item,
                                     FoodId = id
                                 };
                                
                        }

                        Ingredient newi = new Ingredient
                        {
                            Name = i.Name,
                            Description = i.Description,
                            FoodId = id
                        };
                        ingredientRepository.Create(newi);
                    }

                    // Save updated request back to the repository
                    foodrepository.Update(f);
                    this.hub.Clients.All.SendAsync("FoodRequestUpdate", f);

                    return Ok();
                }
            }

            return BadRequest(ModelState); // Model was not valid
        }


        /*
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var food = this.foodrepository.GetOne(id);
            if (food != null && (food.Requestor.Id == userManager.GetUserId(User) || User.IsInRole("Admin")))
            {
                return Ok(food);
            }
            return Ok();
        }
        [AllowAnonymous]
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(string id, [FromBody] FoodRequest food)
        {
            if (ModelState.IsValid)
            {
                var old = this.foodrepository.GetOne(id);

                if (old.Requestor.Id != userManager.GetUserId(User) && !User.IsInRole("Admin"))
                    return RedirectToAction(nameof(Index));

                old.Name = food.Name;
                old.Description = food.Description;
                old.IsDone = food.IsDone;

                this.foodrepository.Update(old);
                return Ok(food);
            }

            return Ok(food);
        }*/

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var food = this.foodrepository.GetOne(id);

            if (food != null && (food.RequestorId == userManager.GetUserId(User) || User.IsInRole("Admin")))
            {
                this.foodrepository.Delete(food);
            }

            return Ok();


        }

       /*
        [AllowAnonymous]
        [HttpGet("Image")]
        public IActionResult GetImage(string id)
        {
            FoodRequest p = this.foodrepository.GetOne(id);
            if (p == null)
            {
                return NotFound();
            }

            return new FileContentResult(p.Picture, p.PictureContentType);
        }*/

     

        
        [HttpGet("SeeAcceptedOffers")]
        public  IEnumerable<FoodRequestViewModel> SeeAcceptedOffers()
        {
            var foodrequest = this.foodrepository.SeeAcceptedOffers(userManager.GetUserId(User));

            var rInfos = new List<FoodRequestViewModel>();
            foreach (var r in foodrequest)
            {
                rInfos.Add(new FoodRequestViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Payment = r.Payment,
                    IsDone = r.IsDone,
                    InProgress = r.InProgress,
                    Deliveryoptions = r.Deliveryoptions,
                    RequestorId = r.RequestorId,
                    PictureURL = r.PictureURL


                });
            }

            return rInfos;
        }


        
        [HttpGet("SeeOtherAcceptedOffers/{id}")]

        public IEnumerable<FoodRequestViewModel> SeeOtherAcceptedOffers(string id)
        {
            var foodrequest = this.foodrepository.SeeAcceptedOffers(id);

            var rInfos = new List<FoodRequestViewModel>();
            foreach (var r in foodrequest)
            {
                rInfos.Add(new FoodRequestViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Payment = r.Payment,
                    IsDone = r.IsDone,
                    InProgress = r.InProgress,
                    Deliveryoptions = r.Deliveryoptions,
                    RequestorId = r.RequestorId,
                    PictureURL = r.PictureURL


                });
            }

            return rInfos;
        }
        
        [HttpPut("ChooseOffer")]

        public async Task<IActionResult> ChooseOffer(string foodId, string offerId)
        {
            var food = this.foodrepository.GetOne(foodId);
            var offer = this.offerrepository.GetOne(offerId);
            var contractor = this.foodUserRepository.GetFooduserById(offer.ContractorId);
            var requestor = this.foodUserRepository.GetFooduserById(userManager.GetUserId(User));


            if (food == null || food.RequestorId != userManager.GetUserId(User))
                return BadRequest(new { message = "Invalid food or unauthorized access" });

            if (food.InProgress || food.IsDone)
                return BadRequest(new { message = "Food request is already in progress or done" });

            if (offer == null || offer.Choosen)
                return BadRequest(new { message = "Invalid offer or offer is already chosen" });


            food.InProgress = true;
            offer.Choosen = true;
            food.Contractor = contractor;



            this.foodrepository.Update(food);
            this.offerrepository.Update(offer);


            return Ok();
        }

        
        [HttpPut("CompleteRequest")]

        public async Task<IActionResult> CompleteRequest(string id)
        {
            var food = this.foodrepository.GetOne(id);
            var chosenOffer = offerrepository.GetAll().Where(x => x.FoodId == id && x.Choosen).FirstOrDefault();
            var requestor = this.foodUserRepository.GetFooduserById(food.RequestorId);
            var contractor = this.foodUserRepository.GetFooduserById(userManager.GetUserId(User));


            if (food == null || !food.InProgress || food.IsDone)
                return BadRequest(new { message = "Invalid food request or food request is not in progress or already done" });

            if (chosenOffer == null || !chosenOffer.Choosen)
                return BadRequest(new { message = "Invalid offer or offer is not chosen" });


            if (chosenOffer != null)
            {
                if (food == null || chosenOffer.ContractorId != userManager.GetUserId(User))
                    return Ok();


                contractor.Founds = contractor.Founds + food.Payment;

                requestor.Founds = requestor.Founds - food.Payment;

                food.IsDone = true;
                this.foodrepository.Update(food);
                this.foodUserRepository.UpdateFooduser(contractor);
                this.foodUserRepository.UpdateFooduser(requestor);


            }

            return Ok();
        }


        
        [HttpPut("CancelRequest")]

        public async Task<IActionResult> CancelRequest(string id)
        {
            var food = this.foodrepository.GetOne(id);
            
            var chosenOffer = offerrepository.GetAll().Where(x => x.FoodId == id && x.Choosen).FirstOrDefault();

            if (food == null || !food.InProgress || food.IsDone)
                return BadRequest(new { message = "Invalid food request or food request is not in progress or already done" });

            if (chosenOffer == null || !chosenOffer.Choosen)
                return BadRequest(new { message = "Invalid offer or offer is not chosen" });


            if (chosenOffer != null)
            {
                if (food == null || (food.RequestorId != userManager.GetUserId(User) && chosenOffer.ContractorId != userManager.GetUserId(User)))
                return Ok();

            food.IsDone = false;
            food.InProgress = false;
            food.Contractor = null;


           
                chosenOffer.Choosen = false;
                this.offerrepository.Update(chosenOffer);
            }

            this.foodrepository.Update(food);

            return Ok();
        }

        
        [HttpPut("Resubmit")]

        public async Task<IActionResult> Resubmit(string id)
        {
            var food = this.foodrepository.GetOne(id);

            if (food == null )
                return BadRequest(new { message = "Invalid food request" });

            food.IsDone = false;
            food.InProgress = false;
            food.Contractor = null;
            food.Offers = null;

            var offers = offerrepository.GetAll();
           
            var chosenOffer = offers.Where(x => x.FoodId == id);

            if (chosenOffer != null)
            {
                foreach (var item in chosenOffer)
                {

                    item.FoodId = null;
                    offerrepository.Update(item);


                }

            }

            this.foodrepository.Update(food);

            return Ok();
        }



    }
}

