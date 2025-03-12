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
  styleUrls: ['./workitem-edit.component.css']
})
export class WorkItemEditComponent {
  id: number = 0;
  workItem: any;
  faTrash = faTrash;
  faX = faX;
  faSave = faSave;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private workItemSvc: WorkItemService
  ) { }

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
      }
    }
    else {
      // Get WorkItem
      this.workItemSvc.getWorkItem(this.id).subscribe((workItem: IWorkItem | undefined) => this.workItem = workItem);
    }
  }

  saveWorkItem() {
    bootbox.alert('TODO: Save WorkItem');
  }

  deleteWorkItem() {
    bootbox.confirm('Are you sure you want to delete this Work Item?', (result: boolean) => {
      if (result) {
        bootbox.alert('TODO: Delete WorkItem');
      }
    });
  }
}
