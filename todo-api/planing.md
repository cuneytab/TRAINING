# todo-api

## Resources

### user control     -POST*  + http://localhost:7071/api/v1/todo/login
### add user         -POST   + http://localhost:7071/api/v1/todo/new
### delete user      -DELETE + http://localhost:7071/api/v1/todo/delete/{id}
### get todo lists   -GET    + http://localhost:7071/api/v1/todo/{owner}/lists
### add todo list    -POST   + http://localhost:7071/api/v1/todo/{owner}/lists
### update todo list -PUT    + http://localhost:7071/api/v1/todo/{owner}/lists/{id}
### delete todo list -DELETE + http://localhost:7071/api/v1/todo/{owner}/lists/{id}
### get todo items   -GET    + http://localhost:7071/api/v1/todo/{owner}/lists/{listId}/items ?
### add todo item    -POST   + http://localhost:7071/api/v1/todo/{owner}/lists/{listId}/items ?
### get todo item    -GET    + http://localhost:7071/api/v1/todo/{owner}/lists/{listId}/items/{id} ?
### delete todo item -DELETE + http://localhost:7071/api/v1/todo/{owner}/lists/{listId}/items/{id} ?
### update todo item -PUT    + http://localhost:7071/api/v1/todo/{owner}/lists/{listId}/items/{id} ?