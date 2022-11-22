import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ItemListService } from '../item-list.service';
import { TodoList } from '../todoList';

@Component({
  selector: 'app-todo-list-add',
  templateUrl: './todo-list-add.component.html',
  styleUrls: ['./todo-list-add.component.css']
})
export class TodoListAddComponent implements OnInit {

  @Output() isPlusEventOutput = new EventEmitter();
  todoLists: TodoList[] = [];
  owner: string = '';

  constructor(
    private itemListService: ItemListService,
    private route: ActivatedRoute
    ) { }

  ngOnInit(): void {
    this.owner = this.route.snapshot.params["owner"];
    //this.getTodoLists();
  }

  // getTodoLists(): void {
  //   this.itemListService.getTodoLists(this.owner)
  //     .subscribe(todoLists => this.todoLists = todoLists);
  // }

  addTodoList(name: string): void {
    const owner = this.owner;
    name = name.trim();

    if (!name) { return; }
    this.itemListService.addTodoList({ name, owner } as TodoList)
      .subscribe();

      this.refreshPage();
  }

  refreshPage() {
    window.location.reload();
  }

  addBtnClick() {
    this.isPlusEventOutput.emit()
  }

}
