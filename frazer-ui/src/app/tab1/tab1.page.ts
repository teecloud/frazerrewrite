import { CommonModule, CurrencyPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, OnInit, computed, signal } from '@angular/core';
import {
  IonButton,
  IonButtons,
  IonCard,
  IonCardContent,
  IonCardHeader,
  IonCardTitle,
  IonCol,
  IonContent,
  IonGrid,
  IonHeader,
  IonIcon,
  IonInput,
  IonItem,
  IonLabel,
  IonList,
  IonRow,
  IonTextarea,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { addIcons } from 'ionicons';
import { addCircle, create, refresh, trash } from 'ionicons/icons';
import { FrazerApiService } from '../services/frazer-api.service';
import { FrazerRecord, FrazerRecordInput } from '../models/frazer-record';

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss'],
  standalone: true,
  imports: [
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    IonList,
    IonItem,
    IonLabel,
    IonButton,
    IonButtons,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardContent,
    IonInput,
    IonTextarea,
    IonIcon,
    IonGrid,
    IonRow,
    IonCol,
    CommonModule,
    NgIf,
    NgFor,
    ReactiveFormsModule,
    CurrencyPipe,
    DatePipe,
  ],
})
export class Tab1Page implements OnInit {
  records = signal<FrazerRecord[]>([]);
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  isEditing = signal(false);
  activeRecordId = signal<number | null>(null);

  readonly totals = computed(() => {
    const all = this.records();
    const outstanding = all.filter((r) => r.balance > 0);
    const byStatus = all.reduce<Record<string, number>>((acc, record) => {
      acc[record.status] = (acc[record.status] ?? 0) + 1;
      return acc;
    }, {});

    return {
      total: all.length,
      outstandingCount: outstanding.length,
      outstandingBalance: outstanding.reduce((sum, record) => sum + record.balance, 0),
      byStatus,
    };
  });

  readonly recordForm = this.fb.nonNullable.group({
    customerName: ['', [Validators.required, Validators.maxLength(100)]],
    contactNumber: ['', [Validators.maxLength(100)]],
    vehicle: ['', [Validators.maxLength(120)]],
    status: ['Pending', [Validators.required, Validators.maxLength(40)]],
    balance: [0, [Validators.required, Validators.min(0)]],
    notes: ['', [Validators.maxLength(500)]],
  });

  constructor(
    private readonly api: FrazerApiService,
    private readonly fb: FormBuilder,
  ) {
    addIcons({ trash, create, refresh, 'add-circle': addCircle });
  }

  ngOnInit(): void {
    this.startCreate();
    this.loadRecords();
  }

  trackById(_: number, record: FrazerRecord): number {
    return record.id;
  }

  loadRecords(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.api.getRecords().subscribe({
      next: (records) => {
        this.records.set(records);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Unable to load records from the Frazer service.');
        this.isLoading.set(false);
      },
    });
  }

  startCreate(): void {
    this.isEditing.set(false);
    this.activeRecordId.set(null);
    this.recordForm.reset({
      customerName: '',
      contactNumber: '',
      vehicle: '',
      status: 'Pending',
      balance: 0,
      notes: '',
    });
  }

  startEdit(record: FrazerRecord): void {
    this.isEditing.set(true);
    this.activeRecordId.set(record.id);
    this.recordForm.reset({
      customerName: record.customerName,
      contactNumber: record.contactNumber ?? '',
      vehicle: record.vehicle ?? '',
      status: record.status,
      balance: record.balance,
      notes: record.notes ?? '',
    });
  }

  submit(): void {
    if (this.recordForm.invalid) {
      this.recordForm.markAllAsTouched();
      return;
    }

    const payload = this.buildPayload();
    this.errorMessage.set(null);
    this.isLoading.set(true);

    const request$ = this.isEditing()
      ? this.api.updateRecord(this.activeRecordId()!, payload)
      : this.api.createRecord(payload);

    request$.subscribe({
      next: () => {
        this.loadRecords();
        this.startCreate();
      },
      error: () => {
        this.errorMessage.set('Unable to save the record. Please try again.');
        this.isLoading.set(false);
      },
    });
  }

  deleteRecord(record: FrazerRecord, event: Event): void {
    event.stopPropagation();
    if (!confirm(`Delete record for ${record.customerName}?`)) {
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.api.deleteRecord(record.id).subscribe({
      next: () => {
        this.loadRecords();
        if (this.activeRecordId() === record.id) {
          this.startCreate();
        }
      },
      error: () => {
        this.errorMessage.set('Unable to delete the record.');
        this.isLoading.set(false);
      },
    });
  }

  private buildPayload(): FrazerRecordInput {
    const raw = this.recordForm.getRawValue();
    return {
      customerName: raw.customerName.trim(),
      contactNumber: raw.contactNumber?.trim() || undefined,
      vehicle: raw.vehicle?.trim() || undefined,
      status: raw.status.trim(),
      balance: Number(raw.balance),
      notes: raw.notes?.trim() || undefined,
    };
  }
}
