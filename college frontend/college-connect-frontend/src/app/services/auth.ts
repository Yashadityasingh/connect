import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7057/api/Login'; 
  constructor(private http: HttpClient) {}

  login(data: { username: string, password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, data);
  }

  logout() {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}
