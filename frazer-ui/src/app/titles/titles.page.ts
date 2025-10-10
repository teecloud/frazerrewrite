import { AsyncPipe, DatePipe, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import {
  IonCard,
  IonCardContent,
  IonCardHeader,
  IonCardTitle,
  IonContent,
  IonHeader,
  IonItem,
  IonLabel,
  IonList,
  IonSearchbar,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { combineLatest, map, startWith } from 'rxjs';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { provideFixtures } from '../data/fixtures';

@Component({
  selector: 'app-titles',
  standalone: true,
  imports: [
    IonContent,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonSearchbar,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardContent,
    IonList,
    IonItem,
    IonLabel,
    ReactiveFormsModule,
    AsyncPipe,
    NgFor,
    DatePipe,
  ],
  templateUrl: './titles.page.html',
  styleUrls: ['./titles.page.scss'],
})
export class TitlesPage {
  private readonly fixtures = provideFixtures();
  private readonly fb = new FormBuilder();

  readonly filter = this.fb.nonNullable.control('');

  readonly rows$ = combineLatest([
    this.fixtures.titlesReport$,
    this.filter.valueChanges.pipe(startWith(this.filter.value)),
  ]).pipe(
    map(([rows, term]) =>
      rows.filter((row) => row.customerName.toLowerCase().includes((term ?? '').toLowerCase()))
    )
  );
}
