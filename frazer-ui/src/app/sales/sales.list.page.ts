import { AsyncPipe, CurrencyPipe, DatePipe, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
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
  selector: 'app-sales-list',
  standalone: true,
  imports: [
    IonContent,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonList,
    IonItem,
    IonLabel,
    IonButton,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardContent,
    RouterLink,
    AsyncPipe,
    NgFor,
    DatePipe,
    CurrencyPipe,
  ],
  templateUrl: './sales.list.page.html',
  styleUrls: ['./sales.page.scss'],
})
export class SalesListPage {
  private readonly fixtures = provideFixtures();
  readonly sales$ = this.fixtures.sales$;
}
