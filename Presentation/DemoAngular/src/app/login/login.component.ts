import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { faAngular } from '@fortawesome/free-brands-svg-icons';
import { faSignIn, faSpinner, faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';

declare var bootbox: any;

@Component({
  selector: 'demo-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  faAngular = faAngular;
  faSignIn = faSignIn;
  faSpinner = faSpinner;
  faTriangleExclamation = faTriangleExclamation;

  username = '';
  password = '';
  rememberMe = false;
  authenticating = false;
  authenticationFailed = false;

  constructor(private authService: AuthService, private router: Router) { }

  onSubmit() {
    this.authenticating = true;
    this.authenticationFailed = false;
    this.authService.login(this.username, this.password, this.rememberMe).subscribe((isAuthenticated: boolean) => {
      this.authenticating = false;
      if (isAuthenticated) {
        this.authenticationFailed = false;
        this.router.navigate(['/home']);
      }
      else {
        this.authenticationFailed = true;
      }
    });
  }
}
