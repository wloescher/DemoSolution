import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ClientListComponent } from './client/client-list/client-list.component';
import { ClientDetailComponent } from './client/client-detail/client-detail.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { WorkItemListComponent } from './workitem/workitem-list/workitem-list.component';
import { WorkItemDetailComponent } from './workitem/workitem-detail/workitem-detail.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent, title: "Home - DemoAngular" },
  { path: 'clients', component: ClientListComponent, title: "Client List - DemoAngular" },
  { path: 'client/:id', component: ClientDetailComponent, title: "Client Detail - DemoAngular" },
  { path: 'users', component: UserListComponent, title: "User List - DemoAngular" },
  { path: 'user/:id', component: UserDetailComponent, title: "User Detail - DemoAngular" },
  { path: 'workitems', component: WorkItemListComponent, title: "Work Item List - DemoAngular" },
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
