import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faPencil } from '@fortawesome/free-solid-svg-icons';
import { IUser } from '../../../models/user.model';
import { UserService } from '../../../services/user.service';

declare var bootbox: any;

@Component({
  selector: 'demo-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent {
  id: number = 0;
  user: any;
  faPencil = faPencil;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userSvc: UserService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.id = parseInt(params.get('id') ?? '0') ?? 0;
    });
    this.userSvc.getUser(this.id).subscribe((user: IUser | undefined) => this.user = user);
  }

  deleteUser() {
    bootbox.confirm('Are you sure you want to delete this User?', (result: boolean) => {
      if (result) {
        bootbox.alert('TODO: Delete User');
      }
    });
  }
}
