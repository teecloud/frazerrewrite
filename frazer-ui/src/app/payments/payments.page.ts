import { AsyncPipe, CurrencyPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  IonButton,
  IonCard,
  IonCardContent,
  IonCardHeader,
  IonCardTitle,
  IonContent,
  IonHeader,
  IonItem,
  IonLabel,
  IonList,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { map } from 'rxjs';
import { provideFixtures } from '../data/fixtures';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-payments',
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
    IonList,
    IonItem,
    IonLabel,
    IonButton,
    AsyncPipe,
    NgFor,
    NgIf,
    DatePipe,
    CurrencyPipe,
  ],
  templateUrl: './payments.page.html',
  styleUrls: ['./payments.page.scss'],
})
export class PaymentsPage {
  private readonly fixtures = provideFixtures();
  private readonly auth = inject(AuthService);
  readonly payments$ = this.fixtures.payments$;
  readonly isCustomer$ = this.auth.roles$.pipe(map((roles) => roles.includes('Customer')));

  readonly customerPlan = {
    name: 'Jen Cares',
    vehicle: '2004 Ford Five Hundred',
    amount: 150,
    frequency: 'Every two weeks',
  };
}
