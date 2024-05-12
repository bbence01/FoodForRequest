import { Component, OnInit } from '@angular/core';


import { FoodRequest, Ingredient, Offer, CommentF, FoodUser  } from '../models/foodRequest';
import { FoodRequestService } from '../services/foodRequestService';

import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin } from 'rxjs';
import { of } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpHeaders } from '@angular/common/http';

import { AuthService, User } from 'src/app/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { map, switchMap, takeUntil } from 'rxjs/operators';

import { Router } from '@angular/router';


interface OfferViewModel {
  offer: Offer;
  contractor: User;
}



@Component({
  selector: 'app-request-details',
  templateUrl: './request-details.component.html',
  styleUrls: ['./request-details.component.scss']
})
export class RequestDetailsComponent implements OnInit {

  request: FoodRequest = new FoodRequest();
  isRequestOwner: boolean = false;
  ingredients: Ingredient[] = [];
  offers: Offer[] = [];
  comments: CommentF[] = [];
  private destroy$ = new Subject<void>();
  newComment: string = '';

  offersview: OfferViewModel[] = [];

  constructor(
    public route: ActivatedRoute,
    public foodRequestService: FoodRequestService,
    public authService: AuthService,
    private router: Router
    ) {}

  ngOnInit(): void {
    const requestId = this.route.snapshot.paramMap.get('id') ?? '';
    this.loadOffers(requestId);
    this.loadComments(requestId);
    this.loadIngredients(requestId);
    if (requestId) {
      this.foodRequestService.getRequestById(requestId).subscribe(request => {
        this.request = request;
        this.checkRequestOwner();
        console.log(this.request);

      });
    } else {

    }



      this.loadOffersView(requestId);

  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  checkRequestOwner(): void {
    if (this.request) {
      this.authService.getUserProfile()
        .pipe(takeUntil(this.destroy$))
        .subscribe(
          (currentUser: User) => {
            this.isRequestOwner = currentUser.id === this.request?.requestorId;
          },
          (error) => {
            console.error('Error fetching user profile:', error);
          }
        );
    }
  }


  loadIngredients(requestId: string): void {
    // Implement a function in your foodRequestService to fetch comments by requestId
    this.foodRequestService.getIngredientssByRequestId(requestId).subscribe(ingredients => {
      this.ingredients = ingredients;
      console.log(ingredients)
    });
  }
  // Implement other functions for making an offer, adding a comment, and editing request details
  loadOffers(requestId: string): void {
    // Implement a function in your foodRequestService to fetch offers by requestId
    this.foodRequestService.getOffersByRequestId(requestId).subscribe(offers => {
      this.offers = offers;
      console.log(offers)
    });
  }

  loadComments(requestId: string): void {
    // Implement a function in your foodRequestService to fetch comments by requestId
    this.foodRequestService.getCommentsByRequestId(requestId).subscribe(comments => {
      this.comments = comments;
      console.log(comments)
    });
  }



  makeOffer(request: FoodRequest): void {

    let offer = new Offer(

    )




     offer.contractorId = this.authService.currentUser!.id
    offer.foodId =  request.id
    offer.choosen = false

    // Implement a function in your foodRequestService to create an offer
    this.foodRequestService.createOffer(offer).subscribe(createdOffer => {
      this.offers.push(createdOffer);
    });
  }

  addComment(request:FoodRequest, ctext: string): void {

    let comment = new CommentF(


    )

      comment.text = ctext
      comment.requestId =request.id
      comment.contractorId = this.authService.currentUser!.id





    // Implement a function in your foodRequestService to create a comment
    this.foodRequestService.createComment(comment).subscribe(createdComment => {
      this.comments.push(createdComment);
    });
  }

  getRequest(): void {
    const requestId = this.route.snapshot.paramMap.get('id');
    if (requestId) {
      this.foodRequestService.getRequestById(requestId).subscribe((request) => {
        this.request = request;
        this.checkRequestOwner();

      });
    }
    console.log(this.request.id);
    console.log(this.request.pictureUrl);

  }



  createOffer(): void {
    if (this.authService.currentUser) {
      const offer = new Offer();
      offer.foodId = this.request.id;
      offer.contractorId = this.authService.currentUser.id;

      this.foodRequestService.createOffer(offer).subscribe(
        () => {
          console.log('Offer created successfully');
          // You might want to refresh the request data here to show the newly created offer
        },
        (        error: any) => {
          console.log('Error creating offer:', error);
        }
      );
    } else {
      console.log('Error: No current user');
    }
  }


  loadOffersView(requestId: string): void {
    this.foodRequestService.getOffersByRequestId(requestId).pipe(
      switchMap(offers => {
        const offerRequests = offers.map(offer => {
          return this.authService.getUserById(offer.contractorId).pipe(
            map((contractor: any) => ({ offer, contractor }))
          );
        });
        return forkJoin(offerRequests);
      })
    ).subscribe(offerViewModels => {
      this.offersview = offerViewModels;
    });
    console.log(this.request.id);
    console.log(this.request.pictureUrl);
  }


  deleteComment(commentId: string): void {
    this.foodRequestService.deleteComment(commentId).subscribe(() => {
      this.comments = this.comments.filter(comment => comment.id !== commentId);
    });
  }

  deleteOffer(offerId: string): void {
    this.foodRequestService.deleteOffer(offerId).subscribe(() => {
      this.offers = this.offers.filter(offer => offer.id !== offerId);
    });
  }

  deleteRequest(): void {
    this.foodRequestService.deleteRequest(this.request.id).subscribe(() => {
      // After successfully deleting the request, you might want to navigate away from the page
      this.router.navigate(['/request-list']);

    });
  }

  isCurrentUserOwner(): boolean {

     return this.authService.currentUser?.id  === this.request.requestorId;
  }
  acceptOffer(offer: OfferViewModel): void {
    if (!this.request.inProgress && !this.request.isDone && !offer.offer.choosen) {
      this.foodRequestService.chooseOffer(this.request.id, offer.offer.id).subscribe(() => {
        console.log('Offer accepted successfully');
        this.loadOffersView(this.request.id);
      }, error => {
        console.error('Error accepting offer:', error);
        // Here you can show the error message to the user
      });
    } else {
      console.log('Invalid request or offer status for accepting the offer');
      // Here you can show a message to the user
    }
  }

  isChosenOfferContractor(): boolean {
    const chosenOffer = this.offersview.find(offer => offer.offer.choosen);
    return this.authService.currentUser?.id === chosenOffer?.contractor.id;
  }

  cancelRequest(): void {
    const chosenOffer = this.offersview.find(offer => offer.offer.choosen);
    if (this.request.inProgress && !this.request.isDone && chosenOffer) {
      this.foodRequestService.cancelRequest(this.request.id).subscribe(() => {
        console.log('Request cancelled successfully');
        this.getRequest();
        this.loadOffersView(this.request.id);
      }, error => {
        console.error('Error cancelling request:', error);
      });
    } else {
      console.log('Invalid request or offer status for cancelling the request');
      // Here you can show a message to the user
    }
  }


  completeRequest(): void {
    const chosenOffer = this.offersview.find(offer => offer.offer.choosen);
    if (this.request.inProgress && !this.request.isDone && chosenOffer) {
      this.foodRequestService.completeRequest(this.request.id).subscribe(() => {
        console.log('Request completed successfully');
        this.getRequest();
        this.loadOffersView(this.request.id);
      }, error => {
        console.error('Error completing request:', error);
      });
    } else {
      console.log('Invalid request or offer status for completing the request');
      // Here you can show a message to the user
    }
  }

  resubmit(id: string) {
    this.foodRequestService.resubmit(id).subscribe(
      () => {
        this.router.navigate(['/request-list']);
      },
      error => {
        console.error('Error', error);
      }
    );
  }



}
