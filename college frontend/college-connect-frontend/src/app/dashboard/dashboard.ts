import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../services/auth';
import { RoleService } from '../services/role-service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,   // for *ngIf, *ngFor etc.
    RouterLink     // for [routerLink]
  ],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class DashboardComponent implements OnInit {
  username = '';
  role = '';
   showProfileDropdown = false;

  constructor(
    private auth: AuthService,
    private router: Router,
    private roleService: RoleService
  ) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      this.username = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || '';
      this.role = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || '';
    }
  }
  toggleProfileDropdown() {
  this.showProfileDropdown = !this.showProfileDropdown;
}
  logout() {
    this.auth.logout();
    this.router.navigate(['/auth/login']);
  }
}
