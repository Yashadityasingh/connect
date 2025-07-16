import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Assignment {
  id: number;
  title: string;
  description: string;
  dueDate: string;
}

@Injectable({ providedIn: 'root' })
export class AssignmentsService {
  private baseUrl = 'https://localhost:7057/api/assignment';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Assignment[]> {
    return this.http.get<Assignment[]>(this.baseUrl);
  }

  create(a: { title: string; description: string; dueDate: string }): Observable<Assignment> {
    return this.http.post<Assignment>(this.baseUrl, a);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
