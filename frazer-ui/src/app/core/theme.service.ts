import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  initialize(): void {
    document.body.classList.add('frazer-theme');
  }
}
