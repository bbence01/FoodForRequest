import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FoodRequest } from '../models/foodRequest';
import { FoodRequestService } from '../services/foodRequestService';
import { HttpClient } from '@angular/common/http';
//import { User } from '../models/user';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  foodRequests: Array<FoodRequest>;
  http: HttpClient;
  displayFoodRequests: Array<FoodRequest> | undefined;

  constructor(private router: Router,public authService: AuthService,http: HttpClient,private foodRequestService: FoodRequestService) {
    this.http = http
    this.foodRequests = []
    this.getAllFood()
  }
/*
  foodRequests: FoodRequest[] = [
    new FoodRequest(1, 'Food Request 1', 'Sample food request 1', 'https://via.placeholder.com/150', 1, new Date(), new Date()),
    new FoodRequest(2, 'Food Request 2', 'Sample food request 2', 'https://via.placeholder.com/150', 2, new Date(), new Date()),
    // Add more food request objects as needed
  ];*/

  public getAllFood() {
    this.http.get<any>('http://localhost:5274/api/Foodrequest/GetAll')
      .subscribe(resp => {
        resp.map((x:any) => {
          let c = new FoodRequest()

          c.id = x.id
          c.name = x.name
          c.description = x.description
          c.requestorId = x.requestorId
          c.pictureUrl = x.pictureURL

          this.foodRequests.push(c);
        })

        this.displayFoodRequests = this.foodRequests.slice(0, 6);
      })
  }



  ngOnInit(): void {
    /*this.foodRequestService.getAll().subscribe(foodRequests => {
      this.foodRequests = foodRequests;
    });*/
  }


  logout() {
    this.authService.doLogout();
    this.router.navigateByUrl('/login');
  }

  redirectIfNotLoggedIn() {
    if (!this.authService.isLoggedIn) {
      this.router.navigateByUrl('/login');
    }
  }

  onSearch(searchTerm: string) {
    this.router.navigate(['/request-list'], { queryParams: { searchTerm } });
  }
  @ViewChild('searchInput') searchInputElement!: ElementRef;


  fillSearch(value: string, event: Event): void {
    event.preventDefault();
    this.searchInputElement.nativeElement.value = value;
  }


}
