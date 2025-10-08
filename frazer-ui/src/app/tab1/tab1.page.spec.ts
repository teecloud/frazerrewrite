import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { Tab1Page } from './tab1.page';
import { FrazerApiService } from '../services/frazer-api.service';
import { FrazerRecord } from '../models/frazer-record';

describe('Tab1Page', () => {
  let component: Tab1Page;
  let fixture: ComponentFixture<Tab1Page>;
  let apiSpy: jasmine.SpyObj<FrazerApiService>;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj<FrazerApiService>(
      'FrazerApiService',
      ['getRecords', 'createRecord', 'updateRecord', 'deleteRecord']
    );
    apiSpy.getRecords.and.returnValue(of([]));
    apiSpy.createRecord.and.returnValue(of({} as FrazerRecord));
    apiSpy.updateRecord.and.returnValue(of({} as FrazerRecord));
    apiSpy.deleteRecord.and.returnValue(of(void 0));

    await TestBed.configureTestingModule({
      imports: [Tab1Page],
      providers: [{ provide: FrazerApiService, useValue: apiSpy }],
    }).compileComponents();

    fixture = TestBed.createComponent(Tab1Page);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
