import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { faX } from '@fortawesome/free-solid-svg-icons';
import { faSave } from '@fortawesome/free-solid-svg-icons';
import { IUser } from '../../../models/user.model';
import { UserService } from '../../../services/user.service';

declare var bootbox: any;

@Component({
  selector: 'demo-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent {
  id: number = 0;
  user: any;
  faTrash = faTrash;
  faX = faX;
  faSave = faSave;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userSvc: UserService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.id = parseInt(params.get('id') ?? '0') ?? 0;
    });

    if (this.id === 0) {
      // Create new User
      this.user = {
        id: 0,
        guid: '',
        typeId: 0,
        type: '',
        isActive: true,
        isDeleted: false,
        firstName: '',
        middleName: '',
        lastName: '',
        addressLine1: '',
        addressLine2: '',
        city: '',
        region: '',
        postalCode: '',
        country: '',
        phoneNumber: '',
      }
    }
    else {
      // Get User
      this.userSvc.getUser(this.id).subscribe((user: IUser | undefined) => this.user = user);
    }
  }

  saveUser() {
    bootbox.alert('TODO: Save User');
  }

  deleteUser() {
    bootbox.confirm('Are you sure you want to delete this User?', (result: boolean) => {
      if (result) {
        bootbox.alert('TODO: Delete User');
      }
    });
  }
}
