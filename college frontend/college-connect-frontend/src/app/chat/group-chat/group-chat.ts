import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormsModule,
  FormBuilder,
  Validators,
  FormGroup,
  NgForm
} from '@angular/forms';
import { forkJoin, switchMap, map } from 'rxjs';

import { ChatGroupService, ChatGroup } from '../../services/chat-group';
import { GroupMemberService } from '../../services/group-member';
import { ChatMessageService, ChatMessage } from '../../services/chat';
import { RoleService } from '../../services/role-service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './group-chat.html',
  styleUrls: ['./group-chat.css']
})
export class ChatComponent implements OnInit {
  role = '';
  username = '';
  isManager = false;

  groups: ChatGroup[] = [];
  selectedGroup: ChatGroup | null = null;
  activeTab: 'chat' | 'members' = 'chat';

  members: string[] = [];
  messages: ChatMessage[] = [];

  selectedFile: File | null = null;

  groupForm: FormGroup;
  memberForm: FormGroup;
  messageForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private grpSvc: ChatGroupService,
    private memSvc: GroupMemberService,
    private msgSvc: ChatMessageService,
    private roleSvc: RoleService
  ) {
    this.groupForm = this.fb.nonNullable.group({
      name: ['', Validators.required],
      type: ['', Validators.required]
    });

    this.memberForm = this.fb.nonNullable.group({
      username: ['', Validators.required]
    });

    this.messageForm = this.fb.nonNullable.group({
      text: ['', Validators.nullValidator]
    });
  }

  ngOnInit() {
    this.role = this.roleSvc.getRole();
    this.username = this.roleSvc.getUsername();
    this.isManager = ['Admin', 'Teacher'].includes(this.role);
    this.loadGroups();
  }

  private loadGroups() {
    if (this.isManager) {
      this.grpSvc.getAll().subscribe(g => this.groups = g);
    } else {
      this.grpSvc.getAll().pipe(
        switchMap(all => forkJoin(
          all.map(g =>
            this.memSvc.getByGroup(g.name).pipe(
              map(list => list.includes(this.username) ? g : null)
            )
          )
        )),
        map(arr => arr.filter((g): g is ChatGroup => g !== null))
      ).subscribe(f => this.groups = f);
    }
  }

  createGroup() {
    if (this.groupForm.invalid) return;
    this.grpSvc.create(this.groupForm.value).subscribe(() => {
      this.groupForm.reset();
      this.loadGroups();
    });
  }

  deleteGroup(id: number) {
    this.grpSvc.delete(id).subscribe(() => {
      this.loadGroups();
      if (this.selectedGroup?.id === id) this.clearSelection();
    });
  }

  selectGroup(g: ChatGroup) {
    this.selectedGroup = g;
    this.activeTab = 'chat';
    this.loadMessages(g.id);
    this.loadMembers(g.name);
  }

  clearSelection() {
    this.selectedGroup = null;
    this.members = [];
    this.messages = [];
  }

  switchToMembers() {
    if (!this.selectedGroup) return;
    this.activeTab = 'members';
    this.loadMembers(this.selectedGroup.name);
  }

  private loadMembers(groupName: string) {
    this.memSvc.getByGroup(groupName)
      .subscribe(list => this.members = list);
  }

  addMember() {
    if (!this.selectedGroup || this.memberForm.invalid) return;
    const dto = {
      groupName: this.selectedGroup.name,
      username: this.memberForm.value.username
    };
    this.memSvc.add(dto).subscribe(() => {
      this.memberForm.reset();
      this.loadMembers(this.selectedGroup!.name);
    });
  }

  removeMember(username: string) {
    if (!this.selectedGroup) return;
    const dto = { groupName: this.selectedGroup.name, username };
    this.memSvc.remove(dto).subscribe(() =>
      this.loadMembers(this.selectedGroup!.name)
    );
  }

  removeMemberByForm(form: NgForm) {
    if (form.invalid || !this.selectedGroup) return;
    const { username } = form.value;
    const dto = { groupName: this.selectedGroup.name, username };
    this.memSvc.remove(dto).subscribe(() => {
      this.loadMembers(this.selectedGroup!.name);
      form.resetForm();
    });
  }

private loadMessages(groupId: number) {
  const baseUrl = 'https://localhost:7057';

  this.msgSvc.getByGroup(groupId).subscribe(m => {
    this.messages = m.map(msg => ({
      ...msg,
      imageUrl: msg.imageUrl
        ? msg.imageUrl.startsWith('http')
          ? msg.imageUrl
          : `${baseUrl}${msg.imageUrl}`
        : undefined
    }));
  });
}

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files && input.files.length
      ? input.files[0]
      : null;
  }

  sendMessage() {
    if (!this.selectedGroup) return;

    const text = this.messageForm.value.text?.trim() || '';

    if (text && !this.selectedFile) {
      this.msgSvc.sendText({
        groupId: this.selectedGroup.id,
        text
      }).subscribe(() => {
        this.messageForm.reset();
        this.loadMessages(this.selectedGroup!.id);
      });
      return;
    }

    if (this.selectedFile) {
      const form = new FormData();
      form.append('GroupId', this.selectedGroup.id.toString());
      form.append('Text', text);
      form.append('Image', this.selectedFile);

      this.msgSvc.sendWithImage(form).subscribe(() => {
        this.messageForm.reset();
        this.selectedFile = null;
        this.loadMessages(this.selectedGroup!.id);
      });
    }
  }
}
