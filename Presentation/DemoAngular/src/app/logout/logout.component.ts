import { Component } from '@angular/core';
import { faSignIn, faSignOut } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'demo-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent {
  faSignIn = faSignIn;
  faSignOut = faSignOut;
}
