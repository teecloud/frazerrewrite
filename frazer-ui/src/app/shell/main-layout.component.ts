import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import {
  IonApp,
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
  IonRouterOutlet,
  IonSplitPane,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { addIcons } from 'ionicons';
import { documentTextOutline, keyOutline, peopleOutline, receiptOutline, speedometerOutline, swapHorizontalOutline, walletOutline, warningOutline, cloudUploadOutline, constructOutline } from 'ionicons/icons';
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
    IonRouterOutlet,
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
      receiptOutline,
      warningOutline,
      keyOutline,
      walletOutline,
      cloudUploadOutline,
      constructOutline,
    });
  }

  readonly items: NavItem[] = [
    { label: 'Dashboard', icon: 'speedometer-outline', path: '/dashboard' },
    { label: 'Inventory', icon: 'document-text-outline', path: '/inventory' },
    { label: 'Sales', icon: 'swap-horizontal-outline', path: '/sales' },
    { label: 'Titles', icon: 'warning-outline', path: '/titles' },
    { label: 'Insurance', icon: 'key-outline', path: '/insurance' },
    { label: 'Payments', icon: 'wallet-outline', path: '/payments' },
    { label: 'Reports', icon: 'receipt-outline', path: '/reports' },
    { label: 'Admin', icon: 'construct-outline', path: '/admin', roles: ['Admin', 'Manager'] },
    { label: 'Jobs', icon: 'cloud-upload-outline', path: '/jobs', roles: ['Admin', 'Manager'] },
  ];

  readonly filteredItems = computed(() =>
    this.items.filter((item) => !item.roles || item.roles.some((role) => this.auth.snapshot.roles.includes(role)))
  );

  readonly roles$ = this.auth.roles$;

  logout(): void {
    this.auth.logout();
  }
}
