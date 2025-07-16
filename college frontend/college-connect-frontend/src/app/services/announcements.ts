import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Announcement {
  id:            number;
  title:         string;
  message:       string;   // matches your GET payload
  postedById:    number;
  postedByName:  string;
  category:      string;
  postedOn:      string;   // ISO timestamp
}

@Injectable({ providedIn: 'root' })
export class AnnouncementsService {
  private apiUrl = 'https://localhost:7057/api/announcement';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Announcement[]> {
    return this.http.get<Announcement[]>(this.apiUrl);
  }

  create(a: {
    title:    string;
    message:  string;
    category: string;
    postedOn: string;
  }): Observable<Announcement> {
    return this.http.post<Announcement>(this.apiUrl, a);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
