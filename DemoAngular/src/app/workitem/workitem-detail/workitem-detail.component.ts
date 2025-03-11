import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { IWorkItem } from '../../../models/workItem.model';
import { WorkItemService } from '../../../services/workitem.service';

@Component({
  selector: 'demo-workitem-detail',
  templateUrl: './workitem-detail.component.html',
  styleUrls: ['./workitem-detail.component.css']
})
export class WorkItemDetailComponent {
  workItem: any;
  id: number = 0;
  faTrash = faTrash;

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
    console.log('TODO: Delete WorkItem');
  }
}
