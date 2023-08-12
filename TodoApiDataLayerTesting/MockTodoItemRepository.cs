using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApiDataLayerTesting
{
    public class MockTodoItemRepository: ITodoItemRepository
    {
        private List<TodoItem> _todoItems;
        public MockTodoItemRepository(DbContextOptions<TodoContext> options) 
        { 
            _todoItems = new List<TodoItem>();
            using(var context = new TodoContext(options)) 
            { 
             context.Database.EnsureCreated();
            }
        }
        public Task<List<TodoItem>> GetAllTodoItemsAsync() 
        { 
          return Task.FromResult(this._todoItems);
        }
        public Task<TodoItem> GetTodoItemByIdAsync(int id) 
        { 
           var todoItem = _todoItems.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(todoItem);
        }
        public Task<TodoItem> CreateTodoItemAsync(TodoItem todoitem) 
        {
            _todoItems.Add(todoitem);
            return Task.FromResult(todoitem);   
        }
        public Task<TodoItem> UpdateTodoItemAsync(TodoItem todoitem)
        { 
            var existingTodoItem = _todoItems.FirstOrDefault(x=>x.Id == todoitem.Id);
            if(existingTodoItem != null) 
            { 
             _todoItems.Remove(existingTodoItem);
                _todoItems.Add(todoitem);
            }
            return Task.FromResult(todoitem);
        
        }
        public Task<bool> DeleteTodoItemAsync(int id) 
        { 
        
        var todoItem = _todoItems.FirstOrDefault(x=> x.Id == id);
            if(todoItem != null) 
            { 
            
            _todoItems.Remove(todoItem);
            return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        public void Reset()
        {
            _todoItems.Clear();
        }



    }
}
