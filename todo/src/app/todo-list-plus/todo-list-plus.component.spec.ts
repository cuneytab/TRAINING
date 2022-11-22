import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoListPlusComponent } from './todo-list-plus.component';

describe('TodoListPlusComponent', () => {
  let component: TodoListPlusComponent;
  let fixture: ComponentFixture<TodoListPlusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodoListPlusComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TodoListPlusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
