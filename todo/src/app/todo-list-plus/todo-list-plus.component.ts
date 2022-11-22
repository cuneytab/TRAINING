import { Component, EventEmitter, OnInit, Output } from '@angular/core';


@Component({
  selector: 'app-todo-list-plus',
  templateUrl: './todo-list-plus.component.html',
  styleUrls: ['./todo-list-plus.component.css']
})
export class TodoListPlusComponent implements OnInit {

  @Output() isPlusEventOutput = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  plusBtnClick() {
    this.isPlusEventOutput.emit();
  }
}