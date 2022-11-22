import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { ItemListService } from '../item-list.service';
import { TodoList } from '../todoList';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {

  title = '';
  todoLists: TodoList[] = [];
  isPlus: boolean = true;
  owner: string = '';
  listId: string = '';

  constructor(
    private itemListService: ItemListService,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.owner = this.route.snapshot.params["owner"];
    this.title = this.owner + "´s ToDo Lists"
    this.getTodoLists();
  }

  getTodoLists(): void {
    this.itemListService.getTodoLists(this.owner)
      .subscribe(todoLists => this.todoLists = todoLists);
  }

  // addTodoList(name: string): void {
  //   const owner = this.owner;
  //   name = name.trim();

  //   if (!name) { return; }
  //   this.itemListService.addTodoList({ name, owner } as TodoList)
  //     .subscribe(todoList => {
  //       this.todoLists.push(todoList);
  //     });
  // }

  deleteTodoList(todoList: TodoList): void {
    this.todoLists = this.todoLists.filter(t => t !== todoList);
    this.itemListService.deleteTodoList(this.owner, todoList.id).subscribe(
    );
  }

  deleteTodoUser(): void {
    this.itemListService.deleteTodoUser(this.owner).subscribe(
      () => {this.location.back();}
    );

    this.location.back();
  }

  plusBtnClick() {
    this.isPlus = false;
  }

  addBtnClick() {
    this.isPlus = true;
  }
} 
