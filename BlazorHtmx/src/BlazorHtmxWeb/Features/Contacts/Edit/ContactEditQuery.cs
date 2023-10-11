using BlazorHtmxWeb.Common.Handlers;
using Carter;
using Microsoft.AspNetCore.Components.Endpoints;

namespace BlazorHtmxWeb.Features.Contacts.Edit;

public class ContactEditQuery : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.Map("contacts/{id}", (int id, ContactsRepository repository) =>
        {
            var viewModel = repository.Contacts
                .Where(q => q.Id == id)
                .Select(s => new ContactEditViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    City = s.City,
                    Phone = s.Phone
                })
                .FirstOrDefault();

            if (viewModel is null)
            {
                return Results.NotFound();
            }

            return PageResults.Page<ContactEdit>(new
            {
                Contact = viewModel
            });
        })
        .WithName("Contacts.Edit");
    }
}