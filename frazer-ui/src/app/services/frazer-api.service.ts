import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { FrazerRecord, FrazerRecordInput } from '../models/frazer-record';

@Injectable({ providedIn: 'root' })
export class FrazerApiService {
  private readonly baseUrl = `${environment.apiUrl}/records`;

  constructor(private readonly http: HttpClient) {}

  getRecords(): Observable<FrazerRecord[]> {
    return this.http.get<FrazerRecord[]>(this.baseUrl).pipe(map(this.sortByUpdated));
  }

  getRecord(id: number): Observable<FrazerRecord> {
    return this.http.get<FrazerRecord>(`${this.baseUrl}/${id}`);
  }

  createRecord(payload: FrazerRecordInput): Observable<FrazerRecord> {
    return this.http.post<FrazerRecord>(this.baseUrl, payload);
  }

  updateRecord(id: number, payload: FrazerRecordInput): Observable<FrazerRecord> {
    return this.http.put<FrazerRecord>(`${this.baseUrl}/${id}`, payload);
  }

  deleteRecord(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  private sortByUpdated = (records: FrazerRecord[]): FrazerRecord[] =>
    [...records].sort((a, b) => new Date(b.updatedAt).getTime() - new Date(a.updatedAt).getTime());
}
