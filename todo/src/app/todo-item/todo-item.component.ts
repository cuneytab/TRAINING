import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { ItemListService } from '../item-list.service';
import { TodoItem } from '../todoItem';
import { TodoList } from '../todoList';

@Component({
  selector: 'app-todo-item',
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.css']
})
export class TodoItemComponent implements OnInit {

  todoItems: TodoItem[] = [];
  todoLists: TodoList[] = [];
  list: string = '';
  owner: string = '';
  listId: string = '';

  constructor(
    private itemListService: ItemListService, 
    private route: ActivatedRoute,
    private location: Location
    ) { }
 
  ngOnInit(): void {
    this.owner = this.route.snapshot.params["owner"];
    this.listId = this.route.snapshot.params["listId"];
    this.getTodoItems();
    this.getTodoLists();
  }

  getTodoItems(): void {
    this.itemListService.getTodoItems(this.owner, this.listId)
    .subscribe(todoItems => this.todoItems = todoItems);
  }

  getTodoLists(): void {
    this.itemListService.getTodoLists(this.owner)
    .subscribe(todoLists => this.todoLists = todoLists.filter(todoList => todoList.id === this.listId));
  }

  //async addTodoItem(text: string, dueDate: string) {
  //  const owner = this.owner;
  //  const listId = this.listId;
  //  const list = this.todoLists[0].name;
    // let date = formatDate(new Date(dueDate),'yyyy-MM-dd','en-US');
  //  let date = new Date(dueDate);
  //  if (!text || !dueDate) { return; }
  //  await this.itemListService.addTodoItem({text, dueDate: date, owner, list, listId} as TodoItem);
  //}

  addTodoItem(text: string, dueDate: string): void {
    const owner = this.owner;
    const listId = this.listId;
    const list = this.todoLists[0].name;
    // let date = formatDate(new Date(dueDate),'yyyy-MM-dd','en-US');
    let date = new Date(dueDate);

    if (!text || !dueDate) { return; }
    this.itemListService.addTodoItem({text, dueDate: date, owner, list, listId} as TodoItem)
      .subscribe(todoItem => {
        this.todoItems.push(todoItem);
      });
  }

  updateTodoItem(todoItem: TodoItem): void {
    todoItem.check = !todoItem.check;
    this.itemListService.updateTodoItem(todoItem)
    .subscribe();
  }

  deleteTodoItem(todoItem: TodoItem): void {
    this.todoItems = this.todoItems.filter(t => t !== todoItem);
    this.itemListService.deleteTodoItem(this.owner, this.listId, todoItem.id!).subscribe();
  }

  deleteTodoList(): void { //go on
    this.itemListService.deleteTodoList(this.owner, this.listId).subscribe( () => { 
      this.location.back();
    });
    
  }
}
