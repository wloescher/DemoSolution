import { Component } from '@angular/core';
import { IClient } from '../../models/client.model';
import { ClientService } from '../../services/client.service';
import { IUser } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { IWorkItem } from '../../models/workItem.model';
import { WorkItemService } from '../../services/workitem.service';

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
    console.log('Client clicked:', client);
  }

  handleUserRowClick(user: any) {
    console.log('User clicked:', user);
  }

  handleWorkItemRowClick(workItem: any) {
    console.log('Work Item clicked:', workItem);
  }
}
