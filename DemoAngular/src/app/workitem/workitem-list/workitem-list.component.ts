import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faAdd } from '@fortawesome/free-solid-svg-icons';
import { IWorkItem } from '../../../models/workItem.model';
import { WorkItemService } from '../../../services/workitem.service';

@Component({
  selector: 'demo-workitem-list',
  templateUrl: './workitem-list.component.html',
  styleUrls: ['./workitem-list.component.css']
})
export class WorkItemListComponent {
  workItems: any;
  filter: string = '';
  faAdd = faAdd;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private workItemSvc: WorkItemService
  ) { }

  ngOnInit() {
    this.workItemSvc.getWorkItems().subscribe((workItems: IWorkItem[]) => this.workItems = workItems);
    this.route.paramMap.subscribe((params) => {
      this.filter = params.get('filter') ?? '';
    });
  }

  getFilteredWorkItems(): IWorkItem[] {
    return !this.filter
      ? this.workItems
      : this.workItems.filter((workItem: any) => workItem.type.toLowerCase().replace(' ', '-') === this.filter.toLowerCase());
  }

  addWorkItem() {
    alert('TODO: Create WorkItem');
  }
}
