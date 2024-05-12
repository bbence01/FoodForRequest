// src/app/services/food-request.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, map, throwError } from 'rxjs';
import { CommentF, FoodRequest, Ingredient, FoodUser, Offer } from '../models/foodRequest';
import { environment } from '../../environments/environment';
import { HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class FoodRequestService {
  private apiUrlGetAll = 'http://localhost:5274/Foodrequest/GetAll';
  private FoodRequest = 'http://localhost:5274/Foodrequest';
  private apiIngred = 'http://localhost:5274/Ingridient';
  private headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  requests: FoodRequest[] = []
  filteredRequests: FoodRequest[] = [];
  searchTerm: string = '';
  ingredients: Ingredient[] =[]


  constructor(
    private http: HttpClient,
    private authService: AuthService,
    ) { }
/*
  getAll(): Observable<FoodRequest[]> {
    return this.http.get<FoodRequest[]>(this.apiUrlGetAll);
  }*/

  getAll(): Observable<FoodRequest[]> {
    return this.http
      .get<FoodRequest[]>(`${environment.apiUrl}FoodRequest/GetAll`, {
        headers: this.headers,
      })
      .pipe(
        map((data: any[]) => {
          this.requests = data.map((request) => {
            const req = new FoodRequest();
            req.id = request.id;
            req.name = request.name;
            req.description = request.description;
            req.payment = request.payment;
            req.isDone = request.isDone;
            req.inProgress = request.inProgress;
            req.deliveryoptions = request.deliveryoptions;

            req.requestorId = request.requestorId;
            req.pictureUrl = request.pictureURL;
            req.ingredients = [];
            return req;
          });
          return this.requests;
        }),
      );
  }





  getIngredients(): Observable<Ingredient[]> {
    return this.http
      .get<Ingredient[]>(`${environment.apiUrl}Ingridient/GetAll`, {
        headers: this.headers,
      })
      .pipe(
        map((data: any[]) => {
          let ingredients = data.map((ingredient) => {
            const ing = new Ingredient();
            ing.id = ingredient.id;
            ing.description = ingredient.description;
            ing.name = ingredient.name;
            ing.foodid = ingredient.foodId;
            return ing;
          });
          return ingredients;
        }),
      );
  }


  getOne(id: number): Observable<FoodRequest> {
    return this.http.get<FoodRequest>(`${this.FoodRequest}/${id}`)  .pipe(catchError(this.handleError));

  }

  update(foodRequest: FoodRequest): Observable<FoodRequest> {
    return this.http.put<FoodRequest>(`${this.FoodRequest}/${foodRequest.id}`, foodRequest)  .pipe(catchError(this.handleError));

  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.FoodRequest}/${id}`)  .pipe(catchError(this.handleError));

  }

  getRequests(): Observable<FoodRequest[]> {
    const apiUrl = `${environment.apiUrl}/Foodrequests`;
    return this.http.get<FoodRequest[]>(apiUrl)  .pipe(catchError(this.handleError));

  }

  addIngredientsToRequest(requestId: string, ingredients: Ingredient[]): Observable<FoodRequest> {
    const api = `${environment.apiUrl}Ingridient/${requestId}/addIngredients`;
    return this.http.put<FoodRequest>(api, ingredients, { headers: this.headers }).pipe(
      map((res) => {
        return res as FoodRequest;
      }),

    );
  }


  getRequestById(id: string): Observable<FoodRequest> {
    return this.http.get<FoodRequest>(`${environment.apiUrl}FoodRequest/${id}`, {
      headers: this.headers,
    }).pipe(
      map((request: any) => {
        const req = new FoodRequest();
        req.id = request.id;
        req.name = request.name;
        req.description = request.description;
        req.payment = request.payment;
        req.isDone = request.isDone;
        req.inProgress = request.inProgress;
        req.deliveryoptions = request.deliveryoptions;
        req.requestorId = request.requestorId;
        req.pictureUrl = request.pictureURL;
        req.ingredients = [];
        return req;
      }),
    );
  }








  getComments(): Observable<CommentF[]> {
    return this.http
      .get<CommentF[]>(`${environment.apiUrl}Comment/GetAll`, {
        headers: this.headers,
      })
      .pipe(
        map((data: any[]) => {
          let comments = data.map((comment) => {
            const ing = new CommentF();
            ing.id = comment.id;
            ing.text = comment.text;
            ing.contractorId = comment.contractorId;
            ing.requestId = comment.requestId;
            return ing;
          });
          return comments;
        }),
      );
  }
  getOffers(): Observable<Offer[]> {
    return this.http
      .get<Offer[]>(`${environment.apiUrl}Offer/all`, {
        headers: this.headers,
      })
      .pipe(
        map((data: any[]) => {
          let offers = data.map((offer) => {
            const ing = new Offer();
            ing.id = offer.id;
            ing.choosen = offer.choosen;
            ing.foodId = offer.foodId;
            ing.contractorId = offer.contractorId;

            return ing;
          });
          return offers;
        }),
      );
  }

  getIngredientssByRequestId(requestId: string): Observable<Ingredient[]> {
    return this.http.get<Ingredient[]>(`${environment.apiUrl}Ingridient/GetIngredientsForRequest/${requestId}`, {
      headers: this.headers,
    })  .pipe(catchError(this.handleError));

  }

  getOffersByRequestId(requestId: string): Observable<Offer[]> {
    return this.http.get<Offer[]>(`${environment.apiUrl}Offer/GetOffersForRequest/${requestId}`, {
      headers: this.headers,
    })  .pipe(catchError(this.handleError));

  }

  getCommentsByRequestId(requestId: string): Observable<CommentF[]> {
    return this.http.get<CommentF[]>(`${environment.apiUrl}Comment/GetCommentsForRequest/${requestId}`, {
      headers: this.headers,
    })  .pipe(catchError(this.handleError));

  }

  createOffer(offer: Offer): Observable<Offer> {
    return this.http.post<Offer>(`${environment.apiUrl}Offer`, offer, {
      headers: this.headers,
    })  .pipe(catchError(this.handleError));

  }

  createComment(comment: CommentF): Observable<CommentF> {
    return this.http.post<CommentF>(`${environment.apiUrl}Comment`, comment, {
      headers: this.headers,
    })  .pipe(catchError(this.handleError));

  }

  updateRequest(request: FoodRequest): Observable<FoodRequest> {
    return this.http.put<FoodRequest>(`${environment.apiUrl}Foodrequest/${request.id}`, request, {
      headers: this.headers,
    })  .pipe(catchError(this.handleError));

  }

deleteComment(commentId: string): Observable<void> {
  return this.http.delete<void>(`${environment.apiUrl}Comment/${commentId}`, {
    headers: this.headers,
  })  .pipe(catchError(this.handleError));

}

deleteOffer(offerId: string): Observable<void> {
  return this.http.delete<void>(`${environment.apiUrl}Offer/${offerId}`, {
    headers: this.headers,
  })  .pipe(catchError(this.handleError));

}

deleteRequest(requestId: string): Observable<void> {
  return this.http.delete<void>(`${environment.apiUrl}Foodrequest/${requestId}`, {
    headers: this.headers,
  })  .pipe(catchError(this.handleError));

}

createRequest(request: FoodRequest): Observable<FoodRequest> {
  return this.http.post<FoodRequest>(
    `${environment.apiUrl}Foodrequest/CreateFd`,
    request,
    { headers: this.headers }
  )  .pipe(catchError(this.handleError));

}


createIngredient(ingredient: Ingredient): Observable<Ingredient> {
  return this.http.post<Ingredient>(
    `${environment.apiUrl}Ingridient/CreateIngredient`,
    ingredient,
    { headers: this.headers }
  )  .pipe(catchError(this.handleError));


}


chooseOffer(foodId: string, offerId: string): Observable<any> {
  const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': 'Bearer ' + this.authService.getToken()
    })
  };

  return this.http.put(`${environment.apiUrl}Foodrequest/ChooseOffer?foodId=${foodId}&offerId=${offerId}`, {}, httpOptions)
  .pipe(catchError(this.handleError));

}


cancelRequest(foodId: string): Observable<any> {
  const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': 'Bearer ' + this.authService.getToken()
    })
  };

  return this.http.put(`${environment.apiUrl}Foodrequest/CancelRequest?id=${foodId}`, {}, httpOptions)
  .pipe(catchError(this.handleError));

}

completeRequest(foodId: string): Observable<any> {
  const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': 'Bearer ' + this.authService.getToken()  // uncomment this if you use JWT tokens
    })
  };

  return this.http.put(`${environment.apiUrl}Foodrequest/CompleteRequest?id=${foodId}`, {}, httpOptions)
  .pipe(catchError(this.handleError));

}

private handleError(error: HttpErrorResponse): Observable<never> {
  console.error('Server error:', error);
  let message = '';
  if (error.error instanceof ErrorEvent) {
    message = `An error occurred: ${error.error.message}`;
  } else {
    message = `Server returned code ${error.status}, error message is: ${error.message}`;
  }
  return throwError(message);
}


resubmit(foodId: string): Observable<any> {
  const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
       'Authorization': 'Bearer ' + this.authService.getToken()  // uncomment this if you use JWT tokens
    })
  };

  return this.http.put(`${environment.apiUrl}Foodrequest/Resubmit?id=${foodId}`, {}, httpOptions)
  .pipe(catchError(this.handleError));

}


}
