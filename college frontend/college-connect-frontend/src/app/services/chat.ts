import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';

export interface ChatMessage {
  id: number;
  groupId: number;
  senderId: number;
  senderUser: string;
  text: string;
  sentOn: string;
  imageUrl?: string;
}

@Injectable({ providedIn: 'root' })
export class ChatMessageService {
  private baseUrl = 'https://localhost:7057/api/ChatMessages';

  constructor(private http: HttpClient) {}

  /** JSON‑only text send */
  sendText(dto: { groupId: number, text: string }): Observable<ChatMessage> {
    return this.http.post<ChatMessage>(`${this.baseUrl}/text`, dto);
  }

  /** multipart/form‑data send (text + optional image) */
  sendWithImage(form: FormData): Observable<ChatMessage> {
    return this.http.post<ChatMessage>(this.baseUrl, form);
  }

 getByGroup(groupId: number): Observable<ChatMessage[]> {
  return this.http.get<ChatMessage[]>(`${this.baseUrl}/group/${groupId}`).pipe(
    map(messages =>
      messages.map(m => ({
        ...m,
        imageUrl: m.imageUrl ? `https://localhost:7057${m.imageUrl}` : undefined
      }))
    )
  );
}
}


