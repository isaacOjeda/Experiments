namespace BlazorHtmxWeb.Features.Contacts.List;

public class ContactListModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Phone { get; set; } = null!;

}