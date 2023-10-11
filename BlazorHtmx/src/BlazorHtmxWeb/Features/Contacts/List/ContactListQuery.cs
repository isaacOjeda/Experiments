using BlazorHtmxWeb.Common.Handlers;
using Carter;
using Microsoft.AspNetCore.Components.Endpoints;

namespace BlazorHtmxWeb.Features.Contacts.List;

public class ContactListQuery : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/contacts", (ContactsRepository repository) => 
        {
            var contacts = repository.Contacts
                .Select(s => new ContactListModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    City = s.City,
                    Phone = s.Phone
                })
                .ToList();

            return PageResults.Page<ContactList>(new
            {
                Contacts = contacts
            });
        })
        .WithName("Contacts");
    }
}