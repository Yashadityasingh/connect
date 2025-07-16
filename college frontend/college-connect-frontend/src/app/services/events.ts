import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface EventItem {
  id: number;
  title: string;
  description: string;
  eventDate: string;          // ISO
  createdById: number;
  createdByUser: string;
  targetAudienceType: string;
  specificGroupId: number | null;
}

@Injectable({ providedIn: 'root' })
export class EventsService {
  private baseUrl = 'https://localhost:7057/api/Events';

  constructor(private http: HttpClient) {}

  getAll(): Observable<EventItem[]> {
    return this.http.get<EventItem[]>(this.baseUrl);
  }

  create(e: {
    title: string;
    description: string;
    eventDate: string;
    targetAudienceType: string;
    specificGroupId: number | null;
  }): Observable<EventItem> {
    return this.http.post<EventItem>(this.baseUrl, e);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
