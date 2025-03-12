import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { SiteHeaderComponent } from './site-header/site-header.component';
import { HomeComponent } from './home/home.component';
import { GenericTableComponent } from './generic-table/generic-table.component';

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

@NgModule({
  declarations: [
    AppComponent,
    SiteHeaderComponent,
    HomeComponent,

    // Clients
    ClientListComponent,
    ClientDetailComponent,
    ClientEditComponent,

    // Users
    UserListComponent,
    UserDetailComponent,
    UserEditComponent,

    // WorkItems
    WorkItemListComponent,
    WorkItemDetailComponent,
    WorkItemEditComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    FontAwesomeModule,
    AppRoutingModule,
    HttpClientModule,
    GenericTableComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
