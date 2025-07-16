import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Submission {
  id: number;
  assignmentId: number;
  student: string;
  fileUrl: string;
  submittedOn: string;
  marks: number | null;
}

@Injectable({ providedIn: 'root' })
export class SubmissionsService {
  private baseUrl = 'https://localhost:7057/api/submission';

  constructor(private http: HttpClient) {}

  getByAssignment(assignmentId: number): Observable<Submission[]> {
    return this.http.get<Submission[]>(`${this.baseUrl}/${assignmentId}`);
  }

  submit(assignmentId: number, fileUrl: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${assignmentId}`, { fileUrl });
  }
}
