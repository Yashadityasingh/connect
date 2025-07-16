import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ChatGroup {
  id: number;
  name: string;
  type: string;
  createdById: number;
  createdByUser: string;
}

@Injectable({ providedIn: 'root' })
export class ChatGroupService {
  private baseUrl = 'https://localhost:7057/api/ChatGroups';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ChatGroup[]> {
    return this.http.get<ChatGroup[]>(this.baseUrl);
  }

  create(group: { name: string; type: string }): Observable<ChatGroup> {
    return this.http.post<ChatGroup>(this.baseUrl, group);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
