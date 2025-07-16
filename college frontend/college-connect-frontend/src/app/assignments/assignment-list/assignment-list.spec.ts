import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignmentList } from './assignment-list';

describe('AssignmentList', () => {
  let component: AssignmentList;
  let fixture: ComponentFixture<AssignmentList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignmentList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignmentList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
