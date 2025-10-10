import { AsyncPipe, CurrencyPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import {
  IonCard,
  IonCardContent,
  IonCardHeader,
  IonCardSubtitle,
  IonCardTitle,
  IonCol,
  IonGrid,
  IonItem,
  IonLabel,
  IonList,
  IonRow,
} from '@ionic/angular/standalone';
import { combineLatest, map } from 'rxjs';
import { provideFixtures } from '../data/fixtures';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    IonGrid,
    IonRow,
    IonCol,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardContent,
    IonCardSubtitle,
    IonList,
    IonItem,
    IonLabel,
    AsyncPipe,
    NgIf,
    NgFor,
    CurrencyPipe,
    DatePipe,
  ],
  templateUrl: './dashboard.page.html',
  styleUrls: ['./dashboard.page.scss'],
})
export class DashboardPage {
  private readonly fixtures = provideFixtures();

  readonly metrics$ = combineLatest([this.fixtures.vehicles$, this.fixtures.sales$, this.fixtures.payments$]).pipe(
    map(([vehicles, sales, payments]) => ({
      inventoryCount: vehicles.length,
      soldCount: vehicles.filter((v) => v.isSold).length,
      openSales: sales.length,
      collected: payments.reduce((acc, payment) => acc + payment.amount, 0),
    }))
  );

  readonly jobs$ = this.fixtures.jobs$;
}
