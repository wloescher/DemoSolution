import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkItemListComponent } from './workitem-list.component';

describe('WorkItemListComponent', () => {
  let component: WorkItemListComponent;
  let fixture: ComponentFixture<WorkItemListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WorkItemListComponent]
    });
    fixture = TestBed.createComponent(WorkItemListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
