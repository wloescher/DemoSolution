import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faAdd } from '@fortawesome/free-solid-svg-icons';
import { IUser } from '../../../models/user.model';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'demo-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent {
  users: any;
  filter: string = '';
  faAdd = faAdd;
  headers = [
    { key: 'lastName', displayName: 'Last Name' },
    { key: 'firstName', displayName: 'First Name' },
    { key: 'emailAddress', displayName: 'Email Address' },
    { key: 'type', displayName: 'Type' },
    { key: 'isActive', displayName: 'Active' },
    { key: 'city', displayName: 'City' },
    { key: 'region', displayName: 'Region' },
    { key: 'phoneNumber', displayName: 'Phone Number' }  ];
  clickableColumns = ['name'];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userSvc: UserService
  ) { }

  ngOnInit() {
    this.userSvc.getUsers().subscribe((users: IUser[]) => this.users = users);
    this.route.paramMap.subscribe((params) => {
      this.filter = params.get('filter') ?? '';
    });
  }

  getFilteredUsers(): IUser[] {
    return !this.filter
      ? this.users
      : this.users.filter((user: any) => user.type.toLowerCase().replace(' ', '-') === this.filter.toLowerCase())
  }

  handleRowClick(data: any) {
    console.log('Row clicked:', data);
  }

  addUser() {
    alert('TODO: Create User');
  }
}
