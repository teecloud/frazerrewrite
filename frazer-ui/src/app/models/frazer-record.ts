export interface FrazerRecord {
  id: number;
  customerName: string;
  contactNumber?: string | null;
  vehicle?: string | null;
  status: string;
  balance: number;
  notes?: string | null;
  createdAt: string;
  updatedAt: string;
}

export type FrazerRecordInput = Omit<FrazerRecord, 'id' | 'createdAt' | 'updatedAt'>;
