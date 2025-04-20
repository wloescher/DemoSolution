import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Subscription } from 'rxjs';
import { faAngular } from '@fortawesome/free-brands-svg-icons';
import { faSearch, faSignOut } from '@fortawesome/free-solid-svg-icons';

declare var bootbox: any;

@Component({
  selector: 'demo-site-header',
  templateUrl: './site-header.component.html',
  styleUrls: ['./site-header.component.css'],
})
export class SiteHeaderComponent implements OnInit, OnDestroy {
  faAngular = faAngular;
  faSearch = faSearch;
  faSignOut = faSignOut;
  isLoggedIn: boolean = false;
  private authSubscription?: Subscription;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authSubscription = this.authService.isLoggedIn.subscribe(
      (isLoggedIn) => {
        this.isLoggedIn = isLoggedIn;
      }
    );
  }

  ngOnDestroy(): void {
    if (this.authSubscription) {
      this.authSubscription.unsubscribe();
    }
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/logout']);
  }

  search() {
    bootbox.alert('TODO: Implement search');
  }
}
