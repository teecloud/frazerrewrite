import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { IonButton, IonCard, IonCardContent, IonCardHeader, IonCardTitle, IonContent, IonHeader, IonInput, IonItem, IonLabel, IonTitle, IonToolbar } from '@ionic/angular/standalone';

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
  private readonly fb = new FormBuilder();

  readonly form = this.fb.nonNullable.group({
    frazerHubUrl: ['', Validators.required],
    textMaxxApiKey: ['', Validators.required],
    siriusXmApiKey: ['', Validators.required],
    cardPointeMerchantId: ['', Validators.required],
    cardPointeApiUser: ['', Validators.required],
    cardPointeApiKey: ['', Validators.required],
  });

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    console.log('Saving admin settings', this.form.getRawValue());
  }
}
