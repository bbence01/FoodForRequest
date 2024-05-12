import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { FoodRequestService } from '../services/foodRequestService';
import { Ingredient } from '../models/foodRequest';
import { map } from 'rxjs';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';


function isValidUrl(control: AbstractControl) {
  const urlRegex = /^(ftp|http|https):\/\/[^ "]+$/;
  return urlRegex.test(control.value) ? null : { invalidUrl: true };
}

function isNumber(control: AbstractControl) {
  const numberRegex = /^\d+$/;
  return numberRegex.test(control.value) ? null : { notANumber: true };
}

function isNameUnique(foodRequestService: FoodRequestService): ValidatorFn {
  return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
    return foodRequestService.getIngredients().pipe(
      map((ingredients: Ingredient[]) => {
        const names = ingredients.map(ingredient => ingredient.name);
        return names.includes(control.value) ? { nameNotUnique: true } : null;
      })
    );
  };
}


@Component({
  selector: 'app-create-request',
  templateUrl: './create-request.component.html',
  styleUrls: ['./create-request.component.scss']
})
export class CreateRequestComponent {
  requestForm: FormGroup;
  ingredientForm: FormGroup;  // Add this line

  selectedIngredients: Ingredient[] = [];
  availableIngredients: Ingredient[] = [];  // populate this from your ingredient data source


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


  createRequest(): void {
    if (this.requestForm.valid) {
      const newRequest = this.requestForm.value;
      newRequest.ingredients = newRequest.ingredients.map((ingredient: { id: any; }) => ingredient.id);
      this.foodRequestService.createRequest(newRequest).subscribe(() => {
        this.router.navigate(['/request-list']);
      });
    }
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

