import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ItemListService } from '../item-list.service';
import { TodoUserComponent } from '../todo-user/todo-user.component';

import { TodoUser } from '../todoUser';

@Component({
  selector: 'app-todo-user-create',
  templateUrl: './todo-user-create.component.html',
  styleUrls: ['./todo-user-create.component.css']
})
export class TodoUserCreateComponent implements OnInit {

  todoUser: TodoUser[] = []; 

  constructor(
    private itemListService: ItemListService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  create(user: string): void {
    const id = "";
    user = user.trim();

    if (!user) { return; }

    try {
      this.itemListService.addTodoUser({ id, user } as TodoUser)
      .subscribe(todoUser => {
        this.todoUser.push(todoUser);
      });

      //console.log(this.todoUser[0].user + "1");

      if (this.todoUser.find((u) => u.user == user)) {
        //console.log(this.todoUser[0].user + "2");
        //console.log("before route");
        //this.router.navigateByUrl('/login');
        this.router.navigate(['/login']);
        //console.log("after route");
      } else {
        console.log("A problem occurs by creating user!");
      }
    } catch (error){
      console.log("server error", error);
    }

  }

  // async create(user: string) {
  //   const id = "";
  //   user = user.trim();

  //   if (!user) { return; }

  //   try {
  //     const response = this.itemListService.addTodoUser({ id, user } as TodoUser);
  //     console.log(response?.user + "1");

  //     if (response?.user === user) {
  //       console.log(response.user + "2");
  //       console.log("before route");
  //       await this.router.navigateByUrl('/login');
  //       //await this.router.navigate(['../login']);
  //       console.log("after route");
  //     } else {
  //       console.log("A problem occurs by creating user!");
  //     }
  //   } catch (error){
  //     console.log("server error", error);
  //   }

  // }

}
