import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiClientService } from '../core/api-client.service';
import {
  IonButton,
  IonCard,
  IonCardContent,
  IonCardHeader,
  IonCardTitle,
  IonContent,
  IonHeader,
  IonInput,
  IonItem,
  IonLabel,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    IonContent,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardContent,
    IonItem,
    IonLabel,
    IonInput,
    IonButton,
    ReactiveFormsModule,
  ],
  templateUrl: './admin.page.html',
  styleUrls: ['./admin.page.scss'],
})
export class AdminPage {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(ApiClientService);

  readonly form = this.fb.nonNullable.group({
    frazerHubUrl: ['', Validators.required],
    textMaxxApiKey: ['', Validators.required],
    siriusXmApiKey: ['', Validators.required],
    cardPointeMerchantId: ['', Validators.required],
    cardPointeApiUser: ['', Validators.required],
    cardPointeApiKey: ['', Validators.required],
  });

  readonly inventoryForm = this.fb.nonNullable.group({
    stockNumber: ['', Validators.required],
    vin: ['', Validators.required],
    year: ['', Validators.required],
    make: ['', Validators.required],
    model: ['', Validators.required],
    trim: ['', Validators.required],
    price: [0, [Validators.required, Validators.min(0)]],
    cost: [0, [Validators.required, Validators.min(0)]],
    dateArrived: [''],
  });

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    console.log('Saving admin settings', this.form.getRawValue());
  }

  addInventory(): void {
    if (this.inventoryForm.invalid) {
      this.inventoryForm.markAllAsTouched();
      return;
    }

    const value = this.inventoryForm.getRawValue();
    const payload = {
      stockNumber: value.stockNumber,
      vin: value.vin,
      year: value.year,
      make: value.make,
      model: value.model,
      trim: value.trim,
      price: Number(value.price),
      cost: Number(value.cost),
      dateArrived: value.dateArrived ? new Date(value.dateArrived).toISOString() : null,
    };

    this.api.post('/api/vehicles', payload).subscribe({
      next: (response) => {
        console.log('Inventory created', response);
        this.inventoryForm.reset({
          stockNumber: '',
          vin: '',
          year: '',
          make: '',
          model: '',
          trim: '',
          price: 0,
          cost: 0,
          dateArrived: '',
        });
      },
      error: (error) => {
        console.error('Failed to create inventory', error);
      },
    });
  }
}
