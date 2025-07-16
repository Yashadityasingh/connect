import { Component, OnInit } from '@angular/core';
import { CommonModule }      from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  Validators,
  FormGroup
} from '@angular/forms';
import {
  startOfMonth,
  endOfMonth,
  startOfWeek,
  endOfWeek,
  eachDayOfInterval,
  addMonths,
  subMonths,
  format,
  isSameMonth,
  isSameDay
} from 'date-fns';

import { EventsService, EventItem } from '../../services/events';
import { RoleService }               from '../../services/role-service';

@Component({
  selector: 'app-event-calendar',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './event-list.html',
  styleUrls: ['./event-list.css']
})
export class EventCalendarComponent implements OnInit {
  showCalendar = false;
  month: Date = new Date();
  weeks: Date[][] = [];
  events: EventItem[] = [];
  eventsMap = new Map<string, EventItem[]>();
  selectedDate: Date | null = null;
  dayEvents: EventItem[] = [];
  role = '';
  form: FormGroup;

  // Expose these functions to the template
  isSameMonth = isSameMonth;
  isSameDay = isSameDay;
  formatDate = format;

  constructor(
    private svc: EventsService,
    private roleService: RoleService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.nonNullable.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      eventDate: ['', Validators.required],
      targetAudienceType: ['', Validators.required],
      specificGroupId: [null]
    });
  }

  ngOnInit() {
    this.role = this.roleService.getRole();
    this.loadAll();
  }
   /** Toggle & rebuild calendar when data changes */
  toggleCalendar() {
    this.showCalendar = !this.showCalendar;
    if (this.showCalendar) {
      this.buildCalendar();
    }}

  private loadAll() {
    this.svc.getAll().subscribe(list => {
      this.events = list;
      this.buildMap();
      this.buildCalendar();
      if (this.selectedDate) {
        this.onDateClick(this.selectedDate);
      }
    });
  }

  private buildMap() {
    this.eventsMap.clear();
    this.events.forEach(e => {
      const day = format(new Date(e.eventDate), 'yyyy-MM-dd');
      const arr = this.eventsMap.get(day) || [];
      arr.push(e);
      this.eventsMap.set(day, arr);
    });
  }

  private buildCalendar() {
    const start = startOfWeek(startOfMonth(this.month));
    const end   = endOfWeek(endOfMonth(this.month));
    const days  = eachDayOfInterval({ start, end });
    this.weeks = [];
    for (let i = 0; i < days.length; i += 7) {
      this.weeks.push(days.slice(i, i + 7));
    }
  }

  prevMonth() {
    this.month = subMonths(this.month, 1);
    this.buildCalendar();
  }
  nextMonth() {
    this.month = addMonths(this.month, 1);
    this.buildCalendar();
  }

  onDateClick(day: Date) {
    this.selectedDate = day;
    const key = format(day, 'yyyy-MM-dd');
    this.dayEvents = this.eventsMap.get(key) || [];
  }

  createEvent() {
    if (this.form.invalid) return;
    const val = this.form.value;
    const payload = {
      title: val.title,
      description: val.description,
      eventDate: new Date(val.eventDate).toISOString(),
      targetAudienceType: val.targetAudienceType,
      specificGroupId: val.specificGroupId
    };
    this.svc.create(payload).subscribe(() => {
      this.form.reset();
      this.loadAll();
    });
  }

  deleteEvent(id: number) {
    this.svc.delete(id).subscribe(() => this.loadAll());
  }
}
