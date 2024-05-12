import { Component, OnInit } from '@angular/core';
import { FoodRequestService } from '../services/foodRequestService';
import { AuthService } from 'src/app/services/auth.service';
import { FoodRequest, Offer } from '../models/foodRequest';
import { Observable, forkJoin } from 'rxjs';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})

export class UserProfileComponent implements OnInit {
  userRequests: FoodRequest[] = [];
  userChosenRequests: FoodRequest[] = [];
  allOffers: Offer[] = [];
  showUserRequests = true;

  constructor(private foodRequestService: FoodRequestService, public authService: AuthService) { }

  ngOnInit(): void {
    forkJoin([
      this.foodRequestService.getAll(),
      this.foodRequestService.getOffers()
    ]).subscribe(([requests, offers]) => {
      this.allOffers = offers;
      this.userChosenRequests = requests.filter(request => this.isCurrentUserChosenForRequest(request));
      this.userRequests = requests.filter(request => request.requestorId === this.authService?.currentUser?.id);
    });
  }

  isCurrentUserChosenForRequest(request: FoodRequest): boolean {
    const offerForRequest = this.allOffers.find(offer => offer.foodId === request.id && offer.choosen);
    return offerForRequest ? offerForRequest.contractorId === this.authService?.currentUser?.id : false;
  }

  toggleView(): void {
    this.showUserRequests = !this.showUserRequests;
  }
}
