import { AsyncPipe, DatePipe, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { IonButton, IonCard, IonCardContent, IonCardHeader, IonCardTitle, IonContent, IonHeader, IonItem, IonLabel, IonList, IonTitle, IonToolbar } from '@ionic/angular/standalone';
import { provideFixtures } from '../data/fixtures';

@Component({
  selector: 'app-reports',
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
  ],
  templateUrl: './reports.page.html',
  styleUrls: ['./reports.page.scss'],
})
export class ReportsPage {
  private readonly fixtures = provideFixtures();

  readonly inventory$ = this.fixtures.inventoryReport$;
  readonly titles$ = this.fixtures.titlesReport$;
  readonly insurance$ = this.fixtures.providers$;

  exportCsv(reportName: string): void {
    console.log(`Export ${reportName} report to CSV`);
  }
}
