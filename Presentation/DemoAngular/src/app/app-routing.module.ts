import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../guards/auth.guard';

import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ErrorComponent } from './error/error.component';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { HomeComponent } from './home/home.component';

// Clients
import { ClientListComponent } from './client/client-list/client-list.component';
import { ClientDetailComponent } from './client/client-detail/client-detail.component';
import { ClientEditComponent } from './client/client-edit/client-edit.component';

// Users
import { UserListComponent } from './user/user-list/user-list.component';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';

// WorkItems
import { WorkItemListComponent } from './workitem/workitem-list/workitem-list.component';
import { WorkItemDetailComponent } from './workitem/workitem-detail/workitem-detail.component';
import { WorkItemEditComponent } from './workitem/workitem-edit/workitem-edit.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  { path: 'access-denied', component: AccessDeniedComponent },
  { path: 'home', component: HomeComponent, title: "Home - DemoAngular", canActivate: [AuthGuard] },

  // Clients
  { path: 'clients/:filter', component: ClientListComponent, title: "Client List - DemoAngular", canActivate: [AuthGuard] },
  { path: 'clients', component: ClientListComponent, title: "Client List - DemoAngular", canActivate: [AuthGuard] },
  { path: 'client/add', component: ClientEditComponent, title: "Client Add - DemoAngular", canActivate: [AuthGuard] },
  { path: 'client/edit/:id', component: ClientEditComponent, title: "Client Edit - DemoAngular", canActivate: [AuthGuard] },
  { path: 'client/:id', component: ClientDetailComponent, title: "Client Detail - DemoAngular", canActivate: [AuthGuard] },

  // Users
  { path: 'users/:filter', component: UserListComponent, title: "User List - DemoAngular", canActivate: [AuthGuard] },
  { path: 'users', component: UserListComponent, title: "User List - DemoAngular", canActivate: [AuthGuard] },
  { path: 'user/add', component: UserEditComponent, title: "User Add - DemoAngular", canActivate: [AuthGuard] },
  { path: 'user/edit/:id', component: UserEditComponent, title: "User Edit - DemoAngular", canActivate: [AuthGuard] },
  { path: 'user/:id', component: UserDetailComponent, title: "User Detail - DemoAngular", canActivate: [AuthGuard] },

  // WorkItems
  { path: 'workitems/:filter', component: WorkItemListComponent, title: "Work Item List - DemoAngular", canActivate: [AuthGuard] },
  { path: 'workitems', component: WorkItemListComponent, title: "Work Item List - DemoAngular", canActivate: [AuthGuard] },
  { path: 'workitem/add', component: WorkItemEditComponent, title: "Work Item Add - DemoAngular", canActivate: [AuthGuard] },
  { path: 'workitem/edit/:id', component: WorkItemEditComponent, title: "Work Item Edit - DemoAngular", canActivate: [AuthGuard] },
  { path: 'workitem/:id', component: WorkItemDetailComponent, title: "Work Item Detail - DemoAngular, canActivate: [AuthGuard]" },

  { path: 'error', component: ErrorComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
