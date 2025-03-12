import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
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
  { path: 'home', component: HomeComponent, title: "Home - DemoAngular" },

  // Clients
  { path: 'clients/:filter', component: ClientListComponent, title: "Client List - DemoAngular" },
  { path: 'clients', component: ClientListComponent, title: "Client List - DemoAngular" },
  { path: 'client/add', component: ClientEditComponent, title: "Client Add - DemoAngular" },
  { path: 'client/edit/:id', component: ClientEditComponent, title: "Client Edit - DemoAngular" },
  { path: 'client/:id', component: ClientDetailComponent, title: "Client Detail - DemoAngular" },

  // Users
  { path: 'users/:filter', component: UserListComponent, title: "User List - DemoAngular" },
  { path: 'users', component: UserListComponent, title: "User List - DemoAngular" },
  { path: 'user/add', component: UserEditComponent, title: "User Add - DemoAngular" },
  { path: 'user/edit/:id', component: UserEditComponent, title: "User Edit - DemoAngular" },
  { path: 'user/:id', component: UserDetailComponent, title: "User Detail - DemoAngular" },

  // WorkItems
  { path: 'workitems/:filter', component: WorkItemListComponent, title: "Work Item List - DemoAngular" },
  { path: 'workitems', component: WorkItemListComponent, title: "Work Item List - DemoAngular" },
  { path: 'workitem/add', component: WorkItemEditComponent, title: "Work Item Add - DemoAngular" },
  { path: 'workitem/edit/:id', component: WorkItemEditComponent, title: "Work Item Edit - DemoAngular" },
  { path: 'workitem/:id', component: WorkItemDetailComponent, title: "Work Item Detail - DemoAngular" },
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
