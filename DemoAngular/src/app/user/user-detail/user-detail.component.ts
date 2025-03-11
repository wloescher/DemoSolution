import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { IUser } from '../../../models/user.model';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'demo-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent {
  user: any;
  id: number = 0;
  faTrash = faTrash;

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
    console.log('TODO: Delete User');
  }
}
