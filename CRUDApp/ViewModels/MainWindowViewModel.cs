using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CRUDApp.Models;
using CRUDApp.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace CRUDApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;
    
    [ObservableProperty] private ObservableCollection<Customer> _customers;
    [ObservableProperty] private Customer? _selectedCustomer;
    
    [ObservableProperty] private string _firstname;
    [ObservableProperty] private string _lastname;
    [ObservableProperty] private string _city;
    [ObservableProperty] private string _address;

    [ObservableProperty] private string _saveBtnState = "Save";
    
    [ObservableProperty] private bool _isLoading;

    public MainWindowViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        _customers = new ObservableCollection<Customer>();
        _ = LoadCustomerData();
    }

    public MainWindowViewModel() : this(null!) { }

    [RelayCommand]
    private async Task LoadCustomerData()
    {
        try
        {
            Customers.Clear();
            IsLoading = true;

            await Task.Delay(2000);
            
            var allCustomer = await _customerService.GetAllCustomersAsync();
            foreach (var customer in allCustomer)
            {
                Customers.Add(customer);
            }
        }
        catch (Exception e)
        {
            throw;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SaveCustomer()
    {
        if (!string.IsNullOrWhiteSpace(Firstname) && !string.IsNullOrWhiteSpace(Lastname))
        {
            if (SelectedCustomer is not null)
            {
                var index = Customers.IndexOf(SelectedCustomer);
                SelectedCustomer.FirstName = Firstname;
                SelectedCustomer.LastName = Lastname;
                SelectedCustomer.City = City;
                SelectedCustomer.Address = Address;

                await _customerService.UpdateCustomerAsync(SelectedCustomer);
            
                var temp = Customers[index];
                Customers[index] = null;
                Customers[index] = temp;
                SelectedCustomer = null;
            
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Customer Updated", "Customer Updated Successfuly");
                await box.ShowAsync();
            }
            else
            {
                var c = new Customer()
                {
                    FirstName = Firstname,
                    LastName = Lastname,
                    City = City,
                    Address = Address
                };

                var cId = await _customerService.AddCustomerAsync(c);
                Customers.Add(c);
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Customer Added", "Customer Added Successfuly");
                await box.ShowAsync();
            }
            ClearFieldsCommand.Execute(null);
        }
        else
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Information Message", "First name and last name are required");
            await box.ShowAsync();
        }
        
    }

    [RelayCommand]
    private void ClearFields()
    {
        Firstname = string.Empty;
        Lastname = string.Empty;
        City = string.Empty;
        Address = string.Empty;
    }

    [RelayCommand]
    private async Task DeleteCustomer()
    {
        if (SelectedCustomer is not null)
        {
            await _customerService.DeleteCustomerAsync(SelectedCustomer);
            Customers.Remove(SelectedCustomer);
            SelectedCustomer = null;
            
            var box = MessageBoxManager
                .GetMessageBoxStandard("Customer Deleted", "Customer Deleted Successfuly");
            await box.ShowAsync();
        }
    }

    partial void OnSelectedCustomerChanged(Customer? value)
    {
        if (value is not null)
        {
            Firstname = value.FirstName;
            Lastname = value.LastName;
            City = value.City;
            Address = value.Address;
            SaveBtnState = "Update";
        }
        else
        {
            SaveBtnState = "Save";
        }
    }
}