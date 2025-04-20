import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

declare var bootbox: any;
const cookieName = 'UserToken';

interface AuthResponse {
  token: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'YOUR_BACKEND_API_URL';
  private currentUserSubject: BehaviorSubject<string | null>;
  public currentUser: Observable<string | null>;
  private loggedIn = new BehaviorSubject<boolean>(false);

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  constructor(private http: HttpClient, private cookieService: CookieService) {
    this.currentUserSubject = new BehaviorSubject<string | null>(localStorage.getItem('token'));
    this.currentUser = this.currentUserSubject.asObservable();
    if (this.isAuthenticated()) {
      this.loggedIn.next(true);
    }
  }

  public get currentUserValue(): string | null {
    return this.currentUserSubject.value;
  }

  login(username: string, password: string, rememberMe: boolean): Observable<boolean> {
    // Replace this with your actual authentication logic
    let isValidLogin = false;

    // Create dummy user token
    let dummyToken = {
      NameIdentifier: 0,
      Name: username,
      GivenName: 'test',
      Surname: username,
      Role: username,
      RememberMe: rememberMe
    }

    // Set NameIdentifier to UserId
    if (username == 'admin' && password == 'admin') {
      dummyToken.NameIdentifier = 1;
    }
    else if (username == 'developer' && password == 'developer') {
      dummyToken.NameIdentifier = 2;
    }
    else if (username == 'sales' && password == 'sales') {
      dummyToken.NameIdentifier = 3;
    }
    else if (username == 'marketing' && password == 'marketing') {
      dummyToken.NameIdentifier = 4;
    }
    else if (username == 'accouting' && password == 'accouting') {
      dummyToken.NameIdentifier = 5;
    }
    else if (username == 'executive' && password == 'executive') {
      dummyToken.NameIdentifier = 6;
    }
    else if (username == 'client' && password == 'client') {
      dummyToken.NameIdentifier = 7;
    }

    // Check for valid login and set cookie
    isValidLogin = dummyToken.NameIdentifier != 0;
    if (isValidLogin) {
      let now = new Date();
      let expirationDate = new Date(now.getTime() + 20 * 60 * 1000);
      this.cookieService.set(cookieName, JSON.stringify(dummyToken), expirationDate, '/');
      this.loggedIn.next(true);
    }

    return of(isValidLogin);
  }

  logout() {
    this.cookieService.delete(cookieName);
    this.currentUserSubject.next(null);
    this.loggedIn.next(false);
  }

  isAuthenticated(): boolean {
    const cookieValue = this.cookieService.get(cookieName);
    if (cookieValue) {
      const cookie = JSON.parse(cookieValue);
      return !!cookie;
    }
    return false;
  }
}
