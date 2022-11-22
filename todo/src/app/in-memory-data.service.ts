import { Injectable } from '@angular/core';

import { InMemoryDbService } from 'angular-in-memory-web-api';

@Injectable({
  providedIn: 'root'
})
export class InMemoryDataService implements InMemoryDbService {
  createDb() {
    const todoItems = [
      {
        id: 11,
        text: 'Cleanup the room',
        //dueDate: new Date(2018, 0O5, 0O5, 17, 23, 42, 11),
        dueDate: new Date("2021-12-30"),
        // dueDate: formatDate(new Date("2021-12-30"),'yyyy-MM-dd','en-US'),
        owner: 'Cuneyt',
        list: 'Tasks',
        notes: '',
        checked: false
      },
      {
        id: 12,
        text: 'Cleanup the bathroom',
        dueDate: new Date("2021-12-25"),
        owner: 'Cuneyt',
        list: 'Tasks',
        notes: '',
        checked: false
      },
      {
        id: 13,
        text: 'Buy detergent',
        dueDate: new Date("2021-12-20"),
        owner: 'Cuneyt',
        list: 'Shopping',
        notes: 'As mark of ABC',
        checked: false
      },
      {
        id: 14,
        text: 'Buy Cheese',
        dueDate: new Date("2021-12-15"),
        owner: 'Cuneyt',
        list: 'Shopping',
        notes: 'From Spar',
        checked: true
      },
      {
        id: 15,
        text: 'Rom',
        dueDate: new Date("2022-12-13"),
        owner: 'Cuneyt',
        list: 'Travel',
        notes: '',
        checked: false
      },
      {
        id: 16,
        text: 'Ankara',
        dueDate: new Date("2022-12-14"),
        owner: 'Cuneyt',
        list: 'Travel',
        notes: '',
        checked: true
      },
      {
        id: 17,
        text: 'Moscow',
        dueDate: new Date("2022-12-15"),
        owner: 'Cuneyt',
        list: 'Travel',
        notes: '',
        checked: true
      }
    ];

    const todoLists = [
      { id: 11, name: 'Tasks', owner: 'Cuneyt' },
      { id: 12, name: 'Shopping', owner: 'Cuneyt' },
      { id: 13, name: 'Travel', owner: 'Cuneyt' }
    ];

    return { todoItems, todoLists };

  }

  genId(items: any[]): number {
    return items.length > 0 ? Math.max(...items.map(hero => hero.id)) + 1 : 11;
  }
}
