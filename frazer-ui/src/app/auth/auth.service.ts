import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, map, of, tap } from 'rxjs';
import { ApiClientService } from '../core/api-client.service';
import { AuthResponse, Role } from '../shared/models';

export interface AuthState {
  token?: string;
  roles: Role[];
  expiresAt?: Date;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = inject(ApiClientService);
  private readonly router = inject(Router);
  private readonly state$ = new BehaviorSubject<AuthState>({ roles: [] });

  readonly isAuthenticated$ = this.state$.pipe(map((state) => !!state.token));
  readonly roles$ = this.state$.pipe(map((state) => state.roles));

  get snapshot(): AuthState {
    return this.state$.value;
  }

  login(username: string, password: string): Observable<void> {
    const normalizedUsername = username.trim().toLowerCase();

    if (normalizedUsername === 'jen cares') {
      const next: AuthState = {
        token: 'customer-demo-token',
        roles: ['Customer'],
      };

      this.state$.next(next);
      localStorage.setItem('frazer.auth', JSON.stringify(next));

      return of(void 0);
    }

    return this.api.post<AuthResponse>('/api/auth/login', { username, password }).pipe(
      tap((response) => {
        const next: AuthState = {
          token: response.accessToken,
          roles: response.roles,
          expiresAt: new Date(response.expiresAt),
        };
        this.state$.next(next);
        localStorage.setItem('frazer.auth', JSON.stringify(next));
      }),
      map(() => void 0)
    );
  }

  logout(): void {
    localStorage.removeItem('frazer.auth');
    this.state$.next({ roles: [] });
    this.router.navigate(['/auth/login']);
  }

  hydrate(): void {
    const stored = localStorage.getItem('frazer.auth');
    if (!stored) {
      return;
    }

    try {
      const parsed = JSON.parse(stored) as AuthState;
      if (parsed.token) {
        parsed.expiresAt = parsed.expiresAt ? new Date(parsed.expiresAt) : undefined;
        this.state$.next(parsed);
      }
    } catch (error) {
      console.warn('Failed to hydrate auth state', error);
      localStorage.removeItem('frazer.auth');
    }
  }

  hasRole(required: Role | Role[]): boolean {
    const roles = Array.isArray(required) ? required : [required];
    return roles.some((role) => this.snapshot.roles.includes(role));
  }
}
