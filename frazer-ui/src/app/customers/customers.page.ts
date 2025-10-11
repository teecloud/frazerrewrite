import { AsyncPipe, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import {
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
  selector: 'app-customers',
  standalone: true,
  imports: [IonContent, IonHeader, IonToolbar, IonTitle, IonList, IonItem, IonLabel, AsyncPipe, NgFor],
  templateUrl: './customers.page.html',
  styleUrls: ['./customers.page.scss'],
})
export class CustomersPage {
  private readonly fixtures = provideFixtures();
  readonly customers$ = this.fixtures.customers$;
}
