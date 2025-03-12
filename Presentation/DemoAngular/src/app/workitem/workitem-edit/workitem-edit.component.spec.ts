import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkItemEditComponent } from './workitem-edit.component';

describe('WorkItemEditComponent', () => {
  let component: WorkItemEditComponent;
  let fixture: ComponentFixture<WorkItemEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WorkItemEditComponent]
    });
    fixture = TestBed.createComponent(WorkItemEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
