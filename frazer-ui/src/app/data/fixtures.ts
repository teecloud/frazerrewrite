import { inject } from '@angular/core';
import { of } from 'rxjs';
import {
  InsuranceProvider,
  InventoryReportRow,
  JobStatus,
  PaymentDashboardItem,
  SaleSummary,
  TitlesPendingReportRow,
  VehicleSummary,
} from '../shared/models';
import { ApiClientService } from '../core/api-client.service';

export const mockVehicles: VehicleSummary[] = [
  {
    id: '1',
    stockNumber: 'FZ-1001',
    vin: '1FATP8UH3K5123456',
    year: '2023',
    make: 'Ford',
    model: 'Mustang',
    isSold: false,
  },
  {
    id: '2',
    stockNumber: 'FZ-1002',
    vin: '1C4RJFBG8LC123456',
    year: '2022',
    make: 'Jeep',
    model: 'Grand Cherokee',
    isSold: true,
  },
];

export const mockSales: SaleSummary[] = [
  {
    id: 'sale-1',
    vehicleId: '1',
    customerId: 'customer-1',
    subtotal: 42000,
    feesTotal: 800,
    paymentsTotal: 3000,
    status: 'Draft',
    createdOn: new Date().toISOString(),
  },
];

export const mockPayments: PaymentDashboardItem[] = [
  {
    id: 'payment-1',
    saleId: 'sale-1',
    amount: 3000,
    method: 'Card',
    status: 'Settled',
    collectedOn: new Date().toISOString(),
    externalReference: 'AUTH12345',
  },
];

export const mockJobs: JobStatus[] = [
  {
    id: 'job-1',
    jobType: 'AutoUploads',
    status: 'Completed',
    createdOn: new Date().toISOString(),
    completedOn: new Date().toISOString(),
    message: 'Last run succeeded.',
  },
];

export const mockProviders: InsuranceProvider[] = [
  {
    id: 'provider-1',
    name: 'Frazer Insurance Group',
    phone: '555-0100',
    email: 'support@frazerinsure.test',
    notes: 'Primary carrier',
    isActive: true,
  },
];

export const mockTitlesReport: TitlesPendingReportRow[] = [
  {
    stockNumber: 'FZ-1002',
    vin: '1C4RJFBG8LC123456',
    customerName: 'Jamie Wheeler',
    saleDate: new Date().toISOString(),
    status: 'Pending'
  },
];

export const mockInventoryReport: InventoryReportRow[] = mockVehicles.map((vehicle) => ({
  stockNumber: vehicle.stockNumber,
  vin: vehicle.vin,
  year: vehicle.year,
  make: vehicle.make,
  model: vehicle.model,
  dateArrived: new Date().toISOString(),
  isSold: vehicle.isSold,
  dateSold: vehicle.isSold ? new Date().toISOString() : undefined,
}));

export function provideFixtures(api = inject(ApiClientService)) {
  return {
    vehicles$: of(mockVehicles),
    sales$: of(mockSales),
    payments$: of(mockPayments),
    jobs$: of(mockJobs),
    providers$: of(mockProviders),
    inventoryReport$: of(mockInventoryReport),
    titlesReport$: of(mockTitlesReport),
  };
}
