import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { FoodRequestService } from '../services/foodRequestService';
import { FoodRequest, Ingredient } from '../models/foodRequest';
import { map } from 'rxjs';
import { Observable } from 'rxjs';


function isValidUrl(control: AbstractControl) {
  const urlRegex = /^(ftp|http|https):\/\/[^ "]+$/;
  return urlRegex.test(control.value) ? null : { invalidUrl: true };
}

function isNumber(control: AbstractControl) {
  const numberRegex = /^\d+$/;
  return numberRegex.test(control.value) ? null : { notANumber: true };
}




@Component({
  selector: 'app-edit-request',
  templateUrl: './edit-request.component.html',
  styleUrls: ['./edit-request.component.scss']
})
export class EditRequestComponent implements OnInit {
  requestForm: FormGroup;
  currentRequest: FoodRequest = {} as FoodRequest;
  ingredientForm: FormGroup;


  selectedIngredients: Ingredient[] = [];
  availableIngredients: Ingredient[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private foodRequestService: FoodRequestService,
  ) {

    this.requestForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(1000)]],
      payment: ['', [Validators.required, Validators.min(1), isNumber]],
      deliveryoptions: ['', Validators.required],
      pictureURL: ['', isValidUrl],
      ingredients: [],
    });

    this.ingredientForm = this.formBuilder.group({
      name: ['', [Validators.required, this.uniqueIngredientNameValidator()]],
      description: ['', Validators.required]
    });

    this.fetchAvailableIngredients();

  }

  ngOnInit() {
    const requestId = this.route.snapshot.paramMap.get('id');
    if (requestId) {
      this.foodRequestService.getRequestById(requestId).subscribe((request: FoodRequest) => {
        this.currentRequest = request;
        this.initializeForm(request);
      });
    }
  }

  // Initialize form with request data
  initializeForm(request: FoodRequest) {
    this.requestForm = this.formBuilder.group({
      name: [request.name, [Validators.required, Validators.maxLength(100)]],
      description: [request.description, [Validators.required, Validators.minLength(5), Validators.maxLength(1000)]],
      payment: [request.payment, [Validators.required, Validators.min(1)]],
      deliveryoptions: [request.deliveryoptions, Validators.required],
      pictureURL: [request.pictureUrl],
      ingredients: [request.ingredients],
    });
  }

  updateRequest(): void {
    if (this.requestForm.valid) {
      const updatedRequest = {
        id: this.currentRequest.id,
        ...this.requestForm.value
      };
      updatedRequest.ingredients = updatedRequest.ingredients.map((ingredient: { id: any; }) => ingredient.id);

      this.foodRequestService.updateRequest(updatedRequest).subscribe(() => {
        this.router.navigate(['/request', this.currentRequest.id]);
      });
    }
  }

  fetchAvailableIngredients() {
    this.foodRequestService.getIngredients().subscribe((ingredients: Ingredient[]) => {
        const uniqueNames = Array.from(new Set(ingredients.map((ingredient: Ingredient) => ingredient.name)));
        const uniqueIngredients = uniqueNames.map((name: string) => ingredients.find((ingredient: Ingredient) => ingredient.name === name) as Ingredient);
        this.availableIngredients = uniqueIngredients;
    });
  }

  onSelectIngredient(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedOptions = Array.from(selectElement.options)
      .filter(option => option.selected)
      .map(option => this.availableIngredients.find(ingredient => ingredient.id === option.value));

    if (this.requestForm && this.requestForm.controls['ingredients']) {
      this.requestForm.controls['ingredients'].setValue(selectedOptions);
    }
  }

  removeIngredient(ingredientToRemove: Ingredient): void {
    const ingredients = this.requestForm.controls['ingredients'].value;
    const updatedIngredients = ingredients.filter((ingredient: Ingredient) => ingredient !== ingredientToRemove);

    this.requestForm.controls['ingredients'].setValue(updatedIngredients);
  }



  addIngredient(): void {
    if (this.ingredientForm.valid) {
      const newIngredient = this.ingredientForm.value;
      this.foodRequestService.createIngredient(newIngredient).subscribe(() => {
        this.fetchAvailableIngredients();
        this.ingredientForm.reset();
      });
    }
  }

  uniqueIngredientNameValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const isUniqueName = !this.availableIngredients.some(ingredient => ingredient.name.toLowerCase() === control.value.toLowerCase());

      return isUniqueName ? null : { 'uniqueName': { value: control.value } };
    };
  }

}
