import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faAdd } from '@fortawesome/free-solid-svg-icons';
import { IClient } from '../../../models/client.model';
import { ClientService } from '../../../services/client.service';

@Component({
  selector: 'demo-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css'],
})
export class ClientListComponent {
  clients: any;
  filter: string = '';
  faAdd = faAdd;
  headers = [
    { key: 'name', displayName: 'Name' },
    { key: 'type', displayName: 'Type' },
    { key: 'isActive', displayName: 'Active' },
    { key: 'city', displayName: 'City' },
    { key: 'region', displayName: 'Region' },
  ];
  clickableColumns = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private clientSvc: ClientService,
  ) { }

  ngOnInit() {
    this.clientSvc.getClients().subscribe((clients: IClient[]) => this.clients = clients);
    this.route.paramMap.subscribe((params) => {
      this.filter = params.get('filter') ?? '';
    });
  }

  getFilteredClients(): IClient[] {
    return !this.filter
      ? this.clients
      : this.clients.filter((client: any) => client.type.toLowerCase().replace(' ', '-') === this.filter.toLowerCase())
  }

  handleRowClick(client: any) {
    this.router.navigate(['client', client.id]);
  }

  addClient() {
    console.log('TODO: Create Client');
  }
}
