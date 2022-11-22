import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
//import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';

import { AppComponent } from './app.component';
import { DashboardPageComponent } from './dashboard-page/dashboard-page.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { TodoItemComponent } from './todo-item/todo-item.component';
import { TodoDetailsComponent } from './todo-details/todo-details.component';
//import { InMemoryDataService } from './in-memory-data.service';
import { FooterPageComponent } from './footer-page/footer-page.component';
import { TodoListAddComponent } from './todo-list-add/todo-list-add.component';
import { TodoListPlusComponent } from './todo-list-plus/todo-list-plus.component';
import { TodoUserComponent } from './todo-user/todo-user.component';
import { TodoUserCreateComponent } from './todo-user-create/todo-user-create.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardPageComponent,
    TodoListComponent,
    TodoItemComponent,
    TodoDetailsComponent,
    FooterPageComponent,
    TodoListAddComponent,
    TodoListPlusComponent,
    TodoUserComponent,
    TodoUserCreateComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    // HttpClientInMemoryWebApiModule.forRoot(
    //   InMemoryDataService, { dataEncapsulation: false }
    // )
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
