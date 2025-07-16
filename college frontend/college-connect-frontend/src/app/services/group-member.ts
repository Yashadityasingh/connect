import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface GroupMember {
  groupName: string;
  username:  string;
}

@Injectable({ providedIn: 'root' })
export class GroupMemberService {
  private baseUrl = 'https://localhost:7057/api/GroupMembers';
  private jsonHeaders = new HttpHeaders({
    'Content-Type': 'application/json'
  });

  constructor(private http: HttpClient) {}

  add(member: GroupMember): Observable<void> {
    return this.http.post<void>(
      this.baseUrl,
      member,
      { headers: this.jsonHeaders }
    );
  }

  remove(member: GroupMember): Observable<void> {
    // send exactly the PascalCase keys your DTO expects
    const body = {
      GroupName: member.groupName,
      Username:  member.username
    };

    // use the built-in delete() overload that supports a body
    return this.http.delete<void>(
      this.baseUrl,
      {
        headers: this.jsonHeaders,
        body
      }
    );
  }

   getByGroup(groupName: string): Observable<string[]> {
    // no mapping needed – the API really is string[]
    return this.http.get<string[]>(`${this.baseUrl}/${encodeURIComponent(groupName)}`);
  }
}
