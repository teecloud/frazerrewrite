import { AsyncPipe, CurrencyPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  IonBackButton,
  IonButton,
  IonButtons,
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
import { map, switchMap } from 'rxjs';
import { provideFixtures } from '../data/fixtures';

@Component({
  selector: 'app-sales-detail',
  standalone: true,
  imports: [
    IonContent,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonButtons,
    IonBackButton,
    IonButton,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardContent,
    IonList,
    IonItem,
    IonLabel,
    AsyncPipe,
    NgIf,
    NgFor,
    DatePipe,
    CurrencyPipe,
  ],
  templateUrl: './sales.detail.page.html',
  styleUrls: ['./sales.page.scss'],
})
export class SalesDetailPage {
  private readonly fixtures = provideFixtures();
  private readonly route = inject(ActivatedRoute);

  readonly sale$ = this.route.paramMap.pipe(
    map((params) => params.get('id')),
    switchMap((id) => this.fixtures.sales$.pipe(map((sales) => sales.find((sale) => sale.id === id))))
  );
}
