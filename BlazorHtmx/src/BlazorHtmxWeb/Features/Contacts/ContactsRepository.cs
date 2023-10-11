namespace BlazorHtmxWeb.Features.Contacts;

public class ContactsRepository
{
    public List<Contact> Contacts { get; set;} = new();
}

public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Phone { get; set; } = null!;
}