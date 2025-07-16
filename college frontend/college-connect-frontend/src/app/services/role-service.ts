// src/app/services/role.service.ts
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class RoleService {
  getRole(): string {
    const token = localStorage.getItem('token');
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? '';
    }
    return '';
  }
   getUsername(): string {
    const token = localStorage.getItem('token')!;
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || '';
  }
}
