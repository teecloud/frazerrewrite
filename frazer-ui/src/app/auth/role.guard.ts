import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { Role } from '../shared/models';
import { AuthService } from './auth.service';

export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  const required = route.data['roles'] as Role[] | undefined;

  if (!required || auth.hasRole(required)) {
    return true;
  }

  router.navigate(['/']);
  return false;
};
