import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoUserCreateComponent } from './todo-user-create.component';

describe('TodoUserCreateComponent', () => {
  let component: TodoUserCreateComponent;
  let fixture: ComponentFixture<TodoUserCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodoUserCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TodoUserCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
