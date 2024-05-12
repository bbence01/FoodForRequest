import { Component, OnInit } from '@angular/core';
import { FoodRequest, Ingredient, CommentF,Offer  } from '../models/foodRequest';
import { FoodRequestService } from '../services/foodRequestService';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin } from 'rxjs';
import { of } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpHeaders } from '@angular/common/http';
import { SignalRService } from '../services/signalRService';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Pipe, PipeTransform } from '@angular/core';



@Pipe({ name: 'truncateWords' })
export class TruncateWordsPipe implements PipeTransform {
  transform(value: string, limit: number = 10): string {
    if (!value) {
      return '';
    }

    const words = value.split(/\s+/);

    if (words.length > limit) {
      return words.slice(0, limit).join(' ') + '...';
    }

    return value;
  }
}


@Component({
  selector: 'app-request-list',
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.scss']
})
export class RequestListComponent implements OnInit {
  requests: FoodRequest[]
  filteredRequests: FoodRequest[] = [];
  searchTerm: string = '';
  ingredients: Ingredient[]
  selectedIngredient: Ingredient =  new Ingredient;
  offers: Offer[]
  commentsf: CommentF[]
  http: HttpClient
  filterDone: boolean = false
  filterInProgress: boolean = false




  constructor( private route: ActivatedRoute,private router: Router,public authService: AuthService,http: HttpClient,private FoodRequestService: FoodRequestService,    private signalRService: SignalRService
    ) {
    this.http = http
    this.offers =[]
    this.commentsf=[]
    this.ingredients =[]
    this.filteredRequests=[]
    console.log('Ingredients:', this.ingredients)

    ;


    this.requests = []

    console.log('Requests:', this.requests);
   console.log('Comment:', this.commentsf);
   console.log('Offers:', this.offers);
  this.pairIngredientsToFood()






  }


  ngOnInit(): void {
    forkJoin([
      this.FoodRequestService.getAll(),
      this.FoodRequestService.getIngredients(),
      this.FoodRequestService.getOffers(),
      this.FoodRequestService.getComments(),





    ]).subscribe(([requests, ingredients,offers,commentsf ]) => {
      this.requests = requests;
      this.filteredRequests = requests;
      this.ingredients = ingredients;
      this.offers = offers;
      this.commentsf = commentsf;
      this.pairIngredientsToFood();
      this.pairCommentsToFood();
      this.pairOffesToFood();
      this.fetchUniqueIngredientsForRequests();

    });

    this.route.queryParams.subscribe(params => {
      this.searchTerm = params['searchTerm'];
      // Perform the search
      this.filterRequests();
    });



    this.signalRService.startConnection();

    this.signalRService.hubConnection.on('FoodRequestCreateFd', (data) => {
      forkJoin([
        this.FoodRequestService.getAll(),
        this.FoodRequestService.getIngredients(),
        this.FoodRequestService.getOffers(),
        this.FoodRequestService.getComments(),





      ]).subscribe(([requests, ingredients,offers,commentsf ]) => {
        this.requests = requests;
        this.filteredRequests = requests;
        this.ingredients = ingredients;
        this.offers = offers;
        this.commentsf = commentsf;
        this.pairIngredientsToFood();
        this.pairCommentsToFood();
        this.pairOffesToFood();
        this.fetchUniqueIngredientsForRequests();

      });
    });

    this.signalRService.hubConnection.on('FoodRequestUpdate', (data) => {
      forkJoin([
        this.FoodRequestService.getAll(),
        this.FoodRequestService.getIngredients(),
        this.FoodRequestService.getOffers(),
        this.FoodRequestService.getComments(),





      ]).subscribe(([requests, ingredients,offers,commentsf ]) => {
        this.requests = requests;
        this.filteredRequests = requests;
        this.ingredients = ingredients;
        this.offers = offers;
        this.commentsf = commentsf;
        this.pairIngredientsToFood();
        this.pairCommentsToFood();
        this.pairOffesToFood();
        this.fetchUniqueIngredientsForRequests();

      });
    });



  }


  searchName(): void {
    this.filteredRequests = this.requests.filter((request) =>
      request.name.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  searchDescription(): void {
    this.filteredRequests = this.requests.filter((request) =>
      request.description.toLowerCase().includes(this.searchTerm.toLowerCase())
    );

  }
  filterRequests(): void {
    if (!this.searchTerm && !this.selectedIngredient && !this.filterDone && !this.filterInProgress) {
      this.filteredRequests = this.requests;
      return;
    }

    this.filteredRequests = this.requests.filter((request) =>
      (!this.searchTerm ||
        request.name.toLowerCase().includes(this.searchTerm.toLowerCase())) &&
      (!this.selectedIngredient ||
        (request.ingredients &&
        request.ingredients.some(
          (ingredient) => ingredient.name === this.selectedIngredient.name))) &&
      (!this.filterDone || request.isDone) &&
      (!this.filterInProgress || request.inProgress)
    );
    console.log(this.filteredRequests);
  }




  addIngredients(request: FoodRequest): void {
    const ingredientsToAdd: Ingredient[] = [
    ];

    this.FoodRequestService.addIngredientsToRequest(request.id, ingredientsToAdd).subscribe(
      (updatedRequest) => {
        console.log('Updated request:', updatedRequest);
        // Update the local data if needed
        const index = this.requests.findIndex((r) => r.id === updatedRequest.id);
        if (index !== -1) {
          this.requests[index] = updatedRequest;
          this.filterRequests();
        }
      },
      (error) => {
        console.error('Error adding ingredients:', error);
      }
    );
  }


  pairIngredientsToFood() {
    console.log('Ingredients:', this.ingredients);
    console.log('Requests:', this.requests);

    if (this.requests.length > 0 && this.ingredients.length > 0) {
      this.ingredients.forEach(ing => {
        console.log('Ingredient:', ing);
        const request = this.requests.find(request => request.id === ing.foodid);
        console.log('Matching request:', ing.foodid);

        if (request) {
          request.ingredients.push(ing);

          console.log('Matching request found for ingredient:', ing);
        } else {
          console.log('No matching request found for ingredient:', ing);
        }
      });
      console.log('Ingredients:', this.ingredients);
      console.log('Requests:', this.requests);
    }
  }


  pairCommentsToFood() {
    console.log('Comments:', this.commentsf);
    console.log('Requests:', this.requests);

    if (this.requests.length > 0 && this.commentsf.length > 0) {
      this.commentsf.forEach(ing => {
        console.log('Ingredient:', ing);
        const request = this.requests.find(request => request.id === ing.requestId);
        console.log('Matching request:', ing.requestId);

        if (request) {
          request.comments.push(ing);

          console.log('Matching request found for ingredient:', ing);
        } else {
          console.log('No matching request found for ingredient:', ing);
        }
      });
      console.log('comments:', this.commentsf);
      console.log('Requests:', this.requests);
    }
  }

  pairOffesToFood() {
    console.log('Offers:', this.offers);
    console.log('Requests:', this.requests);

    if (this.requests.length > 0 && this.offers.length > 0) {
      this.offers.forEach(ing => {
        console.log('Ingredient:', ing);
        const request = this.requests.find(request => request.id === ing.foodId);
        console.log('Matching request:', ing.foodId);

        if (request) {
          request.offers.push(ing);

          console.log('Matching request found for ingredient:', ing);
        } else {
          console.log('No matching request found for ingredient:', ing);
        }
      });
      console.log('Offers:', this.offers);
      console.log('Requests:', this.requests);
    }
  }


  fetchUniqueIngredientsForRequests(): void {
    this.FoodRequestService.getIngredients().subscribe((ingredients: any[]) => {
        let uniqueIngredients = Array.from(new Set(ingredients.map((i: { name: any; }) => i.name)))
            .map(name => {
                return ingredients.find((i: { name: unknown; }) => i.name === name);
            })
            .filter((ingredient): ingredient is Ingredient => ingredient !== undefined);

        this.ingredients = uniqueIngredients;
    });
}


redirectIfNotLoggedIn() {
  if (!this.authService.isLoggedIn) {
    this.router.navigateByUrl('/login');
  }
}



}
