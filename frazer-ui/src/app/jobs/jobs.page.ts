import { AsyncPipe, DatePipe, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
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
  IonList,
  IonSelect,
  IonSelectOption,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { provideFixtures } from '../data/fixtures';

@Component({
  selector: 'app-jobs',
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
    IonSelect,
    IonSelectOption,
    IonInput,
    IonButton,
    ReactiveFormsModule,
    AsyncPipe,
    NgFor,
    DatePipe,
  ],
  templateUrl: './jobs.page.html',
  styleUrls: ['./jobs.page.scss'],
})
export class JobsPage {
  private readonly fixtures = provideFixtures();
  private readonly fb = new FormBuilder();

  readonly form = this.fb.nonNullable.group({
    jobType: ['', Validators.required],
    argument: [''],
  });

  readonly jobs$ = this.fixtures.jobs$;

  enqueue(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    console.log('Enqueue job', this.form.getRawValue());
  }
}
