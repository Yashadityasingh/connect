import { Component, OnInit } from '@angular/core';
import { CommonModule }       from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AssignmentsService, Assignment } from '../../services/assignments';
import { SubmissionsService, Submission } from '../../services/submissions';
import { RoleService }        from '../../services/role-service';
import { formatISO } from 'date-fns';

@Component({
  selector: 'app-assignment-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './assignment-list.html',
  styleUrls: ['./assignment-list.css']
})
export class AssignmentListComponent implements OnInit {
  assignments: Assignment[] = [];
  submissions: Submission[] = [];
  loading = false;
  role = '';
  showSubmissionsFor: number | null = null;
  assignmentForm: FormGroup;
  submissionForms: { [key: number]: FormGroup } = {};

  constructor(
    private fb: FormBuilder,
    private asgSvc: AssignmentsService,
    private subSvc: SubmissionsService,
    private roleService: RoleService
  ) {
    // Admin/Teacher form
    this.assignmentForm = this.fb.nonNullable.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      dueDate: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.role = this.roleService.getRole();
    this.loadAssignments();
  }

  private loadAssignments() {
    this.loading = true;
    this.asgSvc.getAll().subscribe({
      next: data => {
        this.assignments = data;
        // prepare a submission form per assignment for students
        data.forEach(a => {
          this.submissionForms[a.id] = this.fb.nonNullable.group({
            fileUrl: ['', Validators.required]
          });
        });
      },
      error: () => {},
      complete: () => this.loading = false
    });
  }

  createAssignment() {
    if (this.assignmentForm.invalid) return;
   const { title, description, dueDate } = this.assignmentForm.value;
  const payload = {
    title,
    description,
    dueDate: formatISO(new Date(dueDate)) 
  };
    this.asgSvc.create(payload).subscribe(() => {
      this.assignmentForm.reset();
      this.loadAssignments();
    });
  }

  deleteAssignment(id: number) {
    this.asgSvc.delete(id).subscribe(() => this.loadAssignments());
  }

  toggleSubmissions(assignmentId: number) {
    if (this.showSubmissionsFor === assignmentId) {
      this.showSubmissionsFor = null;
    } else {
      this.showSubmissionsFor = assignmentId;
      this.subSvc.getByAssignment(assignmentId).subscribe(data => {
        this.submissions = data;
      });
    }
  }

  submitAssignment(id: number) {
    const form = this.submissionForms[id];
    if (form.invalid) return;
    const { fileUrl } = form.value;
    this.subSvc.submit(id, fileUrl).subscribe(() => {
      alert('Submitted successfully!');
      form.reset();
    });
  }
}
