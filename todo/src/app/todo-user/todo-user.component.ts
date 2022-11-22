import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ItemListService } from '../item-list.service';
import { TodoList } from '../todoList';
import { TodoUser } from '../todoUser';

@Component({
  selector: 'app-todo-user',
  templateUrl: './todo-user.component.html',
  styleUrls: ['./todo-user.component.css']
})
export class TodoUserComponent implements OnInit {

  title = 'Cuneyt´s ToDo App';
  //todoLists: TodoList[] = [];

  constructor(
    private itemListService: ItemListService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  async login(user: string) {
    const id = "";
    user = user.trim();

    if (!user) { return; }

    try {
      const response = await this.itemListService.controlTodoUser({ id, user } as TodoUser);
      console.log(response?.user + "1");

      if (response?.user === user) {
        console.log("no error");
        await this.router.navigate([user, "lists"])
      } else {
        console.log("no user");
      }
    } catch {
      console.log("server error");
    }    
  }
}
