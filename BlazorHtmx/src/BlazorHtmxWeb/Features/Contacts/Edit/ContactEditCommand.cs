using Carter;
using Microsoft.AspNetCore.Mvc;

namespace BlazorHtmxWeb.Features.Contacts.Edit;

public class ContactEditCommand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("contacts/{id}", (int id, [FromForm] ContactEditViewModel model, ContactsRepository repository) =>
        {
            var contact = repository.Contacts.FirstOrDefault(q => q.Id == id);

            if (contact is null)
            {
                return Results.NotFound();
            }

            contact.Name = model.Name;
            contact.Email = model.Email;
            contact.City = model.City;
            contact.Phone = model.Phone;

            return Results.RedirectToRoute("Contacts");
        })
        .WithName("Contacts.Update");
    }
}