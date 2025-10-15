import { AsyncPipe, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import {
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
  IonImg,
  IonList,
  IonSearchbar,
  IonThumbnail,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { combineLatest, map, startWith } from 'rxjs';
import { provideFixtures } from '../data/fixtures';
import { VehicleSummary } from '../shared/models';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [
    IonContent,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonSearchbar,
    IonList,
    IonItem,
    IonLabel,
    IonImg,
    IonButtons,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardContent,
    IonButton,
    IonThumbnail,
    ReactiveFormsModule,
    AsyncPipe,
    NgFor,
  ],
  templateUrl: './inventory.page.html',
  styleUrls: ['./inventory.page.scss'],
})
export class InventoryPage {
  private readonly fixtures = provideFixtures();
  private readonly fb = new FormBuilder();

  readonly search = this.fb.nonNullable.control('');

  readonly vehicles$ = this.fixtures.vehicles$;

  readonly filtered$ = combineLatest([
    this.vehicles$,
    this.search.valueChanges.pipe(startWith(this.search.value)),
  ]).pipe(
    map(([vehicles, term]) => this.filterVehicles(vehicles, term))
  );

  private filterVehicles(vehicles: VehicleSummary[], term: string): VehicleSummary[] {
    const value = term?.toLowerCase() ?? '';
    if (!value) {
      return vehicles;
    }

    return vehicles.filter((vehicle) =>
      [vehicle.stockNumber, vehicle.vin, vehicle.make, vehicle.model, vehicle.year]
        .join(' ')
        .toLowerCase()
        .includes(value)
    );
  }
}
