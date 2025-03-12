import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faPencil } from '@fortawesome/free-solid-svg-icons';
import { IWorkItem } from '../../../models/workitem.model';
import { WorkItemService } from '../../../services/workitem.service';

declare var bootbox: any;

@Component({
  selector: 'demo-workitem-detail',
  templateUrl: './workitem-detail.component.html',
  styleUrls: ['./workitem-detail.component.css']
})
export class WorkItemDetailComponent {
  id: number = 0;
  workItem: any;
  faPencil = faPencil;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private workItemSvc: WorkItemService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.id = parseInt(params.get('id') ?? '0') ?? 0;
    });
    this.workItemSvc.getWorkItem(this.id).subscribe((workItem: IWorkItem | undefined) => this.workItem = workItem);
  }

  deleteWorkItem() {
    bootbox.confirm('Are you sure you want to delete this WorkItem?', (result: boolean) => {
      if (result) {
        bootbox.alert('TODO: Delete WorkItem');
      }
    });
  }
}
