import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  IonButton,
  IonButtons,
  IonChip,
  IonContent,
  IonFab,
  IonFabButton,
  IonHeader,
  IonIcon,
  IonInput,
  IonItem,
  IonLabel,
  IonList,
  IonModal,
  IonSelect,
  IonSelectOption,
  IonText,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { BehaviorSubject, combineLatest, firstValueFrom, map } from 'rxjs';
import { provideFixtures } from '../data/fixtures';
import { Prospect, ProspectVehicleSummary, VehicleSummary } from '../shared/models';

@Component({
  selector: 'app-prospects',
  standalone: true,
  imports: [
    IonContent,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonList,
    IonItem,
    IonLabel,
    IonChip,
    IonFab,
    IonFabButton,
    IonIcon,
    IonModal,
    IonButtons,
    IonButton,
    IonInput,
    IonSelect,
    IonSelectOption,
    IonText,
    ReactiveFormsModule,
    AsyncPipe,
    NgFor,
    NgIf,
  ],
  templateUrl: './prospects.page.html',
  styleUrls: ['./prospects.page.scss'],
})
export class ProspectsPage {
  private readonly fixtures = provideFixtures();
  private readonly fb = new FormBuilder();
  private readonly addedProspects$ = new BehaviorSubject<Prospect[]>([]);

  readonly vehicles$ = this.fixtures.vehicles$;
  readonly baseProspects$ = this.fixtures.prospects$;
  readonly prospects$ = combineLatest([this.baseProspects$, this.addedProspects$]).pipe(
    map(([existing, added]) => [...existing, ...added])
  );

  readonly form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: ['', Validators.required],
    vehicleIds: this.fb.nonNullable.control<string[]>([]),
  });

  isModalOpen = false;

  openModal(): void {
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  handleModalDismiss(): void {
    this.form.reset({ name: '', email: '', phone: '', vehicleIds: [] });
  }

  async saveProspect(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const vehicles = await firstValueFrom(this.vehicles$);
    const selectedVehicleIds = this.form.controls.vehicleIds.value;
    const selectedVehicles: ProspectVehicleSummary[] = vehicles
      .filter((vehicle) => selectedVehicleIds.includes(vehicle.id))
      .map((vehicle) => this.toProspectVehicle(vehicle));

    const prospect: Prospect = {
      id: this.generateId(),
      name: this.form.controls.name.value,
      email: this.form.controls.email.value,
      phone: this.form.controls.phone.value,
      vehicles: selectedVehicles,
    };

    const current = this.addedProspects$.value;
    this.addedProspects$.next([...current, prospect]);

    this.closeModal();
    this.handleModalDismiss();
  }

  private toProspectVehicle(vehicle: VehicleSummary): ProspectVehicleSummary {
    return {
      id: vehicle.id,
      stockNumber: vehicle.stockNumber,
      year: vehicle.year,
      make: vehicle.make,
      model: vehicle.model,
    };
  }

  private generateId(): string {
    return typeof crypto !== 'undefined' && 'randomUUID' in crypto
      ? crypto.randomUUID()
      : Math.random().toString(36).slice(2);
  }
}
