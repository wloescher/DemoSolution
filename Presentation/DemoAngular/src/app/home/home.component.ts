import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IClient } from '../../models/client.model';
import { ClientService } from '../../services/client.service';
import { IUser } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { IWorkItem } from '../../models/workitem.model';
import { WorkItemService } from '../../services/workitem.service';

declare var bootbox: any;

@Component({
  selector: 'demo-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  // Clients table
  clients: any;
  clientHeaders = [
    { key: 'name', displayName: 'Name' },
    { key: 'type', displayName: 'Type' },
    { key: 'isActive', displayName: 'Active' },
  ];
  clientClickableColumns = ['name']

  // Users table
  users: any;
  userHeaders = [
    { key: 'lastName', displayName: 'Last Name' },
    { key: 'firstName', displayName: 'First Name' },
    { key: 'emailAddress', displayName: 'Email Address' },
    { key: 'type', displayName: 'Type' },
    { key: 'isActive', displayName: 'Active' },
  ];
  userClickableColumns = ['lastName'];

  // Work Items table
  workItems: any;
  workItemHeaders = [
    { key: 'title', displayName: 'Title' },
    { key: 'clientName', displayName: 'Client' },
    { key: 'type', displayName: 'Type' },
    { key: 'status', displayName: 'Status' },
    { key: 'isActive', displayName: 'Active' },
    { key: 'subTitle', displayName: 'Sub-Title' }
  ];
  workItemClickableColumns = ['title'];

  constructor(
    private router: Router,
    private clientSvc: ClientService,
    private userSvc: UserService,
    private workItemSvc: WorkItemService,
  ) { }

  ngOnInit() {
    this.clientSvc.getClients().subscribe((clients: IClient[]) => this.clients = clients);
    this.userSvc.getUsers().subscribe((users: IUser[]) => this.users = users);
    this.workItemSvc.getWorkItems().subscribe((workItems: IWorkItem[]) => this.workItems = workItems);
  }

  handleClientRowClick(client: any) {
    this.router.navigate(['client', client.id]);
  }

  handleUserRowClick(user: any) {
    this.router.navigate(['user', user.id]);
  }

  handleWorkItemRowClick(workItem: any) {
    this.router.navigate(['workitem', workItem.id]);
  }
}
