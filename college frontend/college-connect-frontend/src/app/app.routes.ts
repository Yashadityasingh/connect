import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login'
import { DashboardComponent } from './dashboard/dashboard';
import { AuthGuard } from './auth/auth-guard';  
import { AnnouncementListComponent } from './announcements/announcement-list/announcement-list';
import { AssignmentListComponent } from './assignments/assignment-list/assignment-list';
import { EventCalendarComponent } from './events/event-list/event-list';
import { ChatGroupService } from './services/chat-group';
import { ChatComponent } from './chat/group-chat/group-chat';


export const routes: Routes = [
      { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  { path: 'auth/login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent,canActivate: [AuthGuard]   },
  {path: 'announcements',component: AnnouncementListComponent,canActivate: [AuthGuard] },
  {path: 'assignments',component: AssignmentListComponent,canActivate: [AuthGuard] },
   {path: 'events',component: EventCalendarComponent,canActivate: [AuthGuard]},
    { path: 'chat', component: ChatComponent, canActivate: [AuthGuard] }
];
