import { Routes } from '@angular/router';

export const SALES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./sales.list.page').then((m) => m.SalesListPage),
  },
  {
    path: ':id',
    loadComponent: () => import('./sales.detail.page').then((m) => m.SalesDetailPage),
  },
];
