import { AsyncPipe, CurrencyPipe, DatePipe, NgFor } from '@angular/common';
import { Component } from '@angular/core';
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
import { provideFixtures } from '../data/fixtures';

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
    DatePipe,
    CurrencyPipe,
  ],
  templateUrl: './payments.page.html',
  styleUrls: ['./payments.page.scss'],
})
export class PaymentsPage {
  private readonly fixtures = provideFixtures();
  readonly payments$ = this.fixtures.payments$;
}
