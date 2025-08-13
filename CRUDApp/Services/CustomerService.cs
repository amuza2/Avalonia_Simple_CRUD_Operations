using System.Collections.Generic;
using System.Threading.Tasks;
using CRUDApp.Data;
using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.Services;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<int> AddCustomerAsync(Customer customer)
    {
        var c = _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return c.Entity.Id;
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(Customer customer)
    {
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}