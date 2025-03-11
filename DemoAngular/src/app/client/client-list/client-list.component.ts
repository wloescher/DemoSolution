import { Component } from '@angular/core';
import { IClient } from '../../../models/client.model';
import { faAdd } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'demo-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css']
})
export class ClientListComponent {
  clients: IClient[];
  faAdd = faAdd;

  constructor() {
    this.clients = [
      {
        id: 1,
        guid: 'f01a5647-cec8-4531-a376-32386244e142',
        typeId: 1,
        type: 'Internal',
        isActive: true,
        isDeleted: false,
        name: 'Internal Client',
        address: '1234 Main St',
        city: 'Fresno',
        region: 'CA',
        postalCode: '12345',
        country: 'USA',
        url: 'https://internal.demo.com',
      },
      {
        id: 2,
        guid: '85374721-a546-4ca7-b303-8b9a5bf0e7aa',
        typeId: 2,
        type: 'External',
        isActive: true,
        isDeleted: false,
        name: 'External Client',
        address: '5678 MLK Blvd',
        city: 'Yonkers',
        region: 'NY',
        postalCode: '12345',
        country: 'USA',
        url: 'https://external.demo.com',
      },
      {
        id: 3,
        guid: '5b499196-7bdc-48d6-be13-2f633c5d6e9f',
        typeId: 3,
        type: 'Lead',
        isActive: true,
        isDeleted: false,
        name: 'Lead Client',
        address: '1234 Washington Ave',
        city: 'Des Moines',
        region: 'IA',
        postalCode: '12345',
        country: 'USA',
        url: 'https://lead.demo.com',
      }
    ];
  }

  addClient() {
    alert('TODO: Create Client');
  }
}
