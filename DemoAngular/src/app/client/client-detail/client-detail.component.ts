import { Component } from '@angular/core';
import { IClient } from '../../../models/client.model';
import { faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'demo-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.css']
})
export class ClientDetailComponent {
  client: IClient;
  faTrash = faTrash;

  constructor() {
    this.client = {
      id: 1,
      guid: 'f01a5647-cec8-4531-a376-32386244e142',
      typeId: 1,
      type: 'Internal',
      isActive: true,
      isDeleted: false,
      name: 'Internal Client',
      address: '1234 Main St',
      city: 'Anytown',
      region: 'CA',
      postalCode: '12345',
      country: 'USA',
      url: 'https://internal.demo.com',
    }
  }

  deleteClient() {
    console.log('TODO: Delete Client');
  }
}
