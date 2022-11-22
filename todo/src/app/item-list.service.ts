import { Injectable } from '@angular/core';
import { firstValueFrom, Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';

import { TodoItem } from './todoItem';
import { TodoList } from './todoList';
import { TodoUser } from './todoUser';

@Injectable({
  providedIn: 'root'
})
export class ItemListService {

  constructor(private http: HttpClient) { }

  private todoUrl = 'http://localhost:7071/api/v1/todo'

  httpOptions = { headers: new HttpHeaders({ 'content-type': 'application/json' })};

  async controlTodoUser(todoUser: TodoUser) { //ok
    const url = `${this.todoUrl}/login`;
    const response = firstValueFrom(this.http.post<TodoUser[]>(url, todoUser));
    return (await response).shift();
  }

  addTodoItem(todoItem: TodoItem): Observable<TodoItem> { //ok
    const url = `${this.todoUrl}/${todoItem.owner}/lists/${todoItem.listId}/items`;
    return this.http.post<TodoItem>(url, todoItem, this.httpOptions)
  }

  addTodoList(todoList: TodoList): Observable<TodoList> { //ok
    const url = `${this.todoUrl}/${todoList.owner}/lists`;
    return this.http.post<TodoList>(url, todoList, this.httpOptions)
  }

  //async
  addTodoUser(todoUser: TodoUser): Observable<TodoUser> { //ok außer routing
    const url = `${this.todoUrl}/add`;
    //const response = firstValueFrom( 
    return this.http.post<TodoUser>(url, todoUser, this.httpOptions);
    //);

     //(await response).shift();
  }

  deleteTodoItem(owner: string , listId: string , id: string): Observable<TodoItem> {  //ok außer bleibende object in vorheriger Seite 
    const url = `${this.todoUrl}/${owner}/lists/${listId}/items/${id}`;                //async or observable
    return this.http.delete<TodoItem>(url, this.httpOptions);
  }

  async deleteTodoItemAsync (owner: string , listId: string , id: string) { //ok außer bleibende object in vorheriger Seite 
    const url = `${this.todoUrl}/${owner}/lists/${listId}/items/${id}`;     //async or observable
    await this.http.delete(url, this.httpOptions);
  }

  deleteTodoList(owner: string, id: string): Observable<TodoList> { //ok 
    const url = `${this.todoUrl}/${owner}/lists/${id}`;             
    return this.http.delete<TodoList>(url, this.httpOptions);
  }

  deleteTodoUser(owner: string): Observable<TodoUser> { //ok, außer Routing
    const url = `${this.todoUrl}/${owner}/delete`;
    return this.http.delete<TodoUser>(url, this.httpOptions);
  }

  getTodoItem(owner: string, id: string, listId: string): Observable<TodoItem> {  //ok
    const url = `${this.todoUrl}/${owner}/lists/${listId}/items/${id}`;
    return this.http.get<TodoItem>(url);
  }

  getTodoItems(owner: String, listId: String): Observable<TodoItem[]> { //ok
    const url = `${this.todoUrl}/${owner}/lists/${listId}/items`;
    return this.http.get<TodoItem[]>(url); 
  }

  getTodoLists(owner: String): Observable<TodoList[]> { //ok
    const url = `${this.todoUrl}/${owner}/lists`;
    return this.http.get<TodoList[]>(url); 
  }

  updateTodoItem(todoItem: TodoItem): Observable<any> {  //ok, außer list
    const url = `${this.todoUrl}/${todoItem.owner}/lists/${todoItem.listId}/items/${todoItem.id}`
    return this.http.put(url, todoItem, this.httpOptions);
  }

  updateTodoList(todoList: TodoList): Observable<any> { //not ok, because of partition key
    const url = `${this.todoUrl}/${todoList.owner}/lists/${todoList.id}`
    return this.http.put(url, todoList, this.httpOptions);
  }








}
