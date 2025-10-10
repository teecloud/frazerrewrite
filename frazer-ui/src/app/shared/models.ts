export type Role = 'Admin' | 'Manager' | 'Sales' | 'Clerk' | 'Service';

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  roles: Role[];
}

export interface VehicleSummary {
  id: string;
  stockNumber: string;
  vin: string;
  year: string;
  make: string;
  model: string;
  isSold: boolean;
}

export interface VehicleDetail extends VehicleSummary {
  trim: string;
  price: number;
  cost: number;
  dateArrived?: string;
  dateSold?: string;
  currentSaleId?: string;
}

export interface CustomerSummary {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
}

export interface CustomerDetail extends CustomerSummary {
  address: string;
  city: string;
  state: string;
  postalCode: string;
}

export interface SaleSummary {
  id: string;
  vehicleId: string;
  customerId: string;
  subtotal: number;
  feesTotal: number;
  paymentsTotal: number;
  status: string;
  createdOn: string;
}

export interface PaymentDashboardItem {
  id: string;
  saleId: string;
  amount: number;
  method: string;
  status: string;
  collectedOn: string;
  externalReference?: string;
}

export interface InsuranceProvider {
  id: string;
  name: string;
  phone: string;
  email: string;
  notes: string;
  isActive: boolean;
}

export interface JobStatus {
  id: string;
  jobType: string;
  status: string;
  createdOn: string;
  completedOn?: string;
  message?: string;
}

export interface InventoryReportRow {
  stockNumber: string;
  vin: string;
  year: string;
  make: string;
  model: string;
  dateArrived?: string;
  isSold: boolean;
  dateSold?: string;
}

export interface TitlesPendingReportRow {
  stockNumber: string;
  vin: string;
  customerName: string;
  saleDate?: string;
  status: string;
}

export interface InsuranceReportRow {
  provider: string;
  activePolicies: number;
  premiumsDue: number;
  lastUpdated?: string;
}
