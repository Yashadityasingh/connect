import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AnnouncementsService, Announcement } from '../../services/announcements';
import { RoleService } from '../../services/role-service';

@Component({
  selector: 'app-announcement-list',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './announcement-list.html',
  styleUrls: ['./announcement-list.css']
})
export class AnnouncementListComponent implements OnInit {
  announcements: Announcement[] = [];
  loading = true;
  role = '';
  form: FormGroup;

  constructor(
    private svc: AnnouncementsService,
    private roleService: RoleService,
    private fb: FormBuilder
  ) {
    // Use nonNullable to ensure form values are always strings
    this.form = this.fb.nonNullable.group({
      title: ['', Validators.required],
      message: ['', Validators.required],
      category: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.role = this.roleService.getRole();
    this.loadAll();
  }

  private loadAll() {
    this.loading = true;
    this.svc.getAll().subscribe({
      next: data => {
        this.announcements = data;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  create() {
    if (this.form.invalid) {
      return;
    }
    const { title, message, category } = this.form.value;
    const payload = {
      title,
      message,
      category,
      postedOn: new Date().toISOString()
    };
    this.svc.create(payload).subscribe(() => {
      this.form.reset();
      this.loadAll();
    });
  }

  delete(id: number) {
    this.svc.delete(id).subscribe(() => this.loadAll());
  }
}
