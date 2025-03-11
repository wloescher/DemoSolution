import { Component } from '@angular/core';
import { IUser } from '../../../models/user.model';
import { faAdd } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'demo-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent {
  users: IUser[];
  faAdd = faAdd;

  constructor() {
    this.users = [
      {
        id: 1,
        guid: '55004793-5dd1-433f-8091-83dab517556b',
        typeId: 1,
        type: 'Admin',
        isActive: true,
        isDeleted: false,
        emailAddress: 'admin@demo.com',
        firstName: 'Admin',
        middleName: '',
        lastName: 'Demo',
        address: '1234 Main St',
        city: 'Fresno',
        region: 'CA',
        postalCode: '12345',
        country: 'USA',
        phoneNumber: '1234567890',
      },
      {
        id: 2,
        guid: 'efc80328-286e-4b3f-b31a-fe60b919e81b',
        typeId: 2,
        type: 'Client',
        isActive: true,
        isDeleted: false,
        emailAddress: 'client@demo.com',
        firstName: 'Client',
        middleName: '',
        lastName: 'Demo',
        address: '1234 Main St',
        city: 'Fresno',
        region: 'CA',
        postalCode: '12345',
        country: 'USA',
        phoneNumber: '1234567891',
      },
      {
        id: 3,
        guid: 'a38c01a1-21c2-45de-bb6b-92d96ef22841',
        typeId: 3,
        type: 'Sales',
        isActive: true,
        isDeleted: false,
        emailAddress: 'sales@demo.com',
        firstName: 'Sales',
        middleName: '',
        lastName: 'Demo',
        address: '1234 Main St',
        city: 'Fresno',
        region: 'CA',
        postalCode: '12345',
        country: 'USA',
        phoneNumber: '1234567892',
      }
    ];
  }

  addUser() {
    alert('TODO: Create User');
  }
}
