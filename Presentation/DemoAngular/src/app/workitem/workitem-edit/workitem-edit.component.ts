import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { faX } from '@fortawesome/free-solid-svg-icons';
import { faSave } from '@fortawesome/free-solid-svg-icons';
import { IWorkItem } from '../../../models/workitem.model';
import { WorkItemService } from '../../../services/workitem.service';

declare var bootbox: any;

@Component({
  selector: 'demo-workitem-edit',
  templateUrl: './workitem-edit.component.html',
  styleUrls: ['./workitem-edit.component.css'],
})
export class WorkItemEditComponent {
  id: number = 0;
  workItem: any;
  faTrash = faTrash;
  faX = faX;
  faSave = faSave;

  workItemTypes = [
    { value: '0', label: 'Select...' },
    { value: '1', label: 'User Story' },
    { value: '2', label: 'Task' },
    { value: '3', label: 'Bug' },
    { value: '4', label: 'Epic' },
    { value: '5', label: 'Feature' },
  ];

  workItemStatuses = [
    { value: '0', label: 'Select...' },
    { value: '1', label: 'New' },
    { value: '2', label: 'In Planning' },
    { value: '3', label: 'In Progress' },
    { value: '4', label: 'Approved' },
    { value: '5', label: 'Rejected' },
    { value: '6', label: 'Staged' },
    { value: '7', label: 'Completed' },
    { value: '8', label: 'On Hold' },
  ];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private workItemSvc: WorkItemService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.id = parseInt(params.get('id') ?? '0') ?? 0;
    });

    if (this.id === 0) {
      // Create new WorkItem
      this.workItem = {
        id: 0,
        guid: '',
        typeId: 0,
        type: '',
        statusId: 0,
        status: '',
        isActive: true,
        isDeleted: false,
        title: '',
        subTitle: '',
        summary: '',
        body: '',
      };
    } else {
      // Get WorkItem
      this.workItemSvc
        .getWorkItem(this.id)
        .subscribe(
          (workItem: IWorkItem | undefined) => (this.workItem = workItem)
        );
    }
  }

  saveWorkItem() {
    bootbox.alert('TODO: Save WorkItem');
  }

  deleteWorkItem() {
    bootbox.confirm(
      'Are you sure you want to delete this Work Item?',
      (result: boolean) => {
        if (result) {
          bootbox.alert('TODO: Delete WorkItem');
        }
      }
    );
  }
}
