import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  IonApp,
  IonButton,
  IonButtons,
  IonContent,
  IonHeader,
  IonIcon,
  IonItem,
  IonLabel,
  IonList,
  IonMenu,
  IonMenuButton,
  IonMenuToggle,
  IonSplitPane,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { addIcons } from 'ionicons';
import {
  documentTextOutline,
  keyOutline,
  peopleOutline,
  personAddOutline,
  receiptOutline,
  speedometerOutline,
  swapHorizontalOutline,
  walletOutline,
  warningOutline,
  cloudUploadOutline,
  constructOutline,
} from 'ionicons/icons';
import { AuthService } from '../auth/auth.service';
import { Role } from '../shared/models';

interface NavItem {
  label: string;
  icon: string;
  path: string;
  roles?: Role[];
}

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    IonApp,
    IonSplitPane,
    IonMenu,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    IonList,
    IonItem,
    IonLabel,
    IonIcon,
    IonMenuToggle,
    IonButton,
    IonButtons,
    IonMenuButton,
    RouterLink,
    RouterLinkActive,
    RouterOutlet,
    NgFor,
    NgIf,
    AsyncPipe,
  ],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss'],
})
export class MainLayoutComponent {
  private readonly auth = inject(AuthService);

  constructor() {
    addIcons({
      speedometerOutline,
      documentTextOutline,
      peopleOutline,
      swapHorizontalOutline,
      personAddOutline,
      receiptOutline,
      warningOutline,
      keyOutline,
      walletOutline,
      cloudUploadOutline,
      constructOutline,
    });
  }

  private readonly dealerItems: NavItem[] = [
    { label: 'Dashboard', icon: 'speedometer-outline', path: '/dashboard' },
    { label: 'Inventory', icon: 'document-text-outline', path: '/inventory' },
    { label: 'Customers', icon: 'people-outline', path: '/customers' },
    { label: 'Prospects', icon: 'person-add-outline', path: '/prospects' },
    { label: 'Sales', icon: 'swap-horizontal-outline', path: '/sales' },
    { label: 'Titles', icon: 'warning-outline', path: '/titles' },
    { label: 'Insurance', icon: 'key-outline', path: '/insurance' },
    { label: 'Payments', icon: 'wallet-outline', path: '/payments' },
    { label: 'Reports', icon: 'receipt-outline', path: '/reports' },
    { label: 'Admin', icon: 'construct-outline', path: '/admin', roles: ['Admin', 'Manager'] },
    { label: 'Jobs', icon: 'cloud-upload-outline', path: '/jobs', roles: ['Admin', 'Manager'] },
  ];

  private readonly customerItems: NavItem[] = [
    { label: 'Payments', icon: 'wallet-outline', path: '/payments' },
  ];

  get filteredItems(): NavItem[] {
    const roles = this.auth.snapshot.roles;

    if (roles.includes('Customer') && roles.length === 1) {
      return this.customerItems;
    }

    return this.dealerItems.filter(
      (item) => !item.roles || item.roles.some((role) => this.auth.snapshot.roles.includes(role))
    );
  }

  get menuTitle(): string {
    return this.isCustomer ? "Customer Payments" : "Jackson's Dealer Suite";
  }

  get toolbarTitle(): string {
    return this.isCustomer ? "Jen Cares — Payment Portal" : "Jackson's Dealer Suite Console";
  }

  private get isCustomer(): boolean {
    return this.auth.snapshot.roles.includes('Customer');
  }

  readonly roles$ = this.auth.roles$;

  logout(): void {
    this.auth.logout();
  }
}
