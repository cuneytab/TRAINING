import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoUserComponent } from './todo-user.component';

describe('TodoUserComponent', () => {
  let component: TodoUserComponent;
  let fixture: ComponentFixture<TodoUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodoUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TodoUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
