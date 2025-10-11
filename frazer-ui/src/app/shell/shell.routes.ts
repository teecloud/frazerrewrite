import { Routes } from '@angular/router';
import { MainLayoutComponent } from './main-layout.component';
import { roleGuard } from '../auth/role.guard';

export const SHELL_ROUTES: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('../dashboard/dashboard.page').then((m) => m.DashboardPage),
      },
      {
        path: 'inventory',
        loadComponent: () => import('../inventory/inventory.page').then((m) => m.InventoryPage),
      },
      {
        path: 'customers',
        loadComponent: () => import('../customers/customers.page').then((m) => m.CustomersPage),
      },
      {
        path: 'prospects',
        loadComponent: () => import('../prospects/prospects.page').then((m) => m.ProspectsPage),
      },
      {
        path: 'sales',
        loadChildren: () => import('../sales/sales.routes').then((m) => m.SALES_ROUTES),
      },
      {
        path: 'titles',
        loadComponent: () => import('../titles/titles.page').then((m) => m.TitlesPage),
      },
      {
        path: 'insurance',
        loadComponent: () => import('../insurance/insurance.page').then((m) => m.InsurancePage),
      },
      {
        path: 'payments',
        loadComponent: () => import('../payments/payments.page').then((m) => m.PaymentsPage),
      },
      {
        path: 'reports',
        loadComponent: () => import('../reports/reports.page').then((m) => m.ReportsPage),
      },
      {
        path: 'admin',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'Manager'] },
        loadComponent: () => import('../admin/admin.page').then((m) => m.AdminPage),
      },
      {
        path: 'jobs',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'Manager'] },
        loadComponent: () => import('../jobs/jobs.page').then((m) => m.JobsPage),
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard',
      },
    ],
  },
];
