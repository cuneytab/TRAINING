import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TodoItemComponent } from './todo-item/todo-item.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { TodoDetailsComponent } from './todo-details/todo-details.component';
import { TodoUserComponent } from './todo-user/todo-user.component';
import { TodoUserCreateComponent } from './todo-user-create/todo-user-create.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: TodoUserComponent},
  { path: 'new', component: TodoUserCreateComponent},
  { path: ':owner/lists', component: TodoListComponent },
  { path: ':owner/lists/:listId/items', component: TodoItemComponent},
  { path: ':owner/lists/:listId/items/:id', component: TodoDetailsComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
