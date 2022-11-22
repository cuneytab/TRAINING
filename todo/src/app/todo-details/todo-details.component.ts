import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { ItemListService } from '../item-list.service';
import { TodoItem } from '../todoItem';
import { TodoList } from '../todoList';

@Component({
  selector: 'app-todo-details',
  templateUrl: './todo-details.component.html',
  styleUrls: ['./todo-details.component.css']
})
export class TodoDetailsComponent implements OnInit {
  todoItem: TodoItem | undefined;
  todoLists: TodoList[] = [];
  owner: string = '';
  id: string = '';
  listId: string = '';

  constructor(
    private itemListService: ItemListService,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.owner = String(this.route.snapshot.paramMap.get('owner'));
    this.id = String(this.route.snapshot.paramMap.get('id'));
    this.listId = String(this.route.snapshot.paramMap.get('listId'));
    this.getTodoItem();
    this.getTodoLists();

  }

  getTodoItem(): void {
    //const id = Number(this.route.snapshot.params["id"]);
    
    this.itemListService.getTodoItem(this.owner, this.id, this.listId)
      .subscribe(todoItem => {this.todoItem = todoItem;});
  }

  getTodoLists(): void {
    this.itemListService.getTodoLists(this.owner)
    .subscribe(todoLists => this.todoLists = todoLists);
  }

  save(): void {
    if (this.todoItem) {
      this.itemListService.updateTodoItem(this.todoItem).subscribe();
    }
  }

  updateTodoItem(todoItem: TodoItem): void {
    todoItem.check = !todoItem.check;
    this.itemListService.updateTodoItem(todoItem)
    .subscribe();
  }

  deleteTodoItem(): void {
    this.itemListService.deleteTodoItem(this.owner, this.listId, this.id).subscribe();
    this.location.back();
  }

  async deleteTodoItemAsync() {
    await this.itemListService.deleteTodoItemAsync(this.owner, this.listId, this.id);
    await this.location.back();
  }

  change(dueDate: string) {
    (this.todoItem as TodoItem).dueDate = new Date(dueDate);
  }

}
