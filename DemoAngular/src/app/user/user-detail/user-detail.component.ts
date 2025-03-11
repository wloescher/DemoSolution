import { Component } from '@angular/core';
import { IUser } from '../../../models/user.model';
import { faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'demo-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent {
  user: IUser;
  faTrash = faTrash;

  constructor() {
    this.user = {
      id: 1,
      guid: '55004793-5dd1-433f-8091-83dab517556b',
      typeId: 1,
      type: 'Admin',
      isActive: true,
      isDeleted: false,
      emailAddress: 'demo@demo.com',
      firstName: 'Admin',
      middleName: '',
      lastName: 'Demo',
      address: '1234 Main St',
      city: 'Anytown',
      region: 'CA',
      postalCode: '12345',
      country: 'USA',
      phoneNumber: '',
    }
  }

  deleteUser() {
    console.log('TODO: Delete User');
  }
}
