import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkItemDetailComponent } from './workitem-detail.component';

describe('WorkItemDetailComponent', () => {
  let component: WorkItemDetailComponent;
  let fixture: ComponentFixture<WorkItemDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WorkItemDetailComponent]
    });
    fixture = TestBed.createComponent(WorkItemDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
