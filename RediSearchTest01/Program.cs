// See https://aka.ms/new-console-template for more information
using Redis.OM;
using Redis.OM.Modeling;

var provider = new RedisConnectionProvider("redis://localhost:6379");
provider.Connection.CreateIndex(typeof(Customer));

var customers = provider.RedisCollection<Customer>();

// // Insert customer
// customers.Insert(new Customer()
// {
//     FullName = "Isaac Ojeda Quintana",
//     Age = 31,
//     Email = "isaac.ojeda@intelectix.com",
//     FirstName = "Isaac",
//     LastName = "Ojeda",
//     NickNames = new string[] { "balunatic", "balu", "balundor" },
//     Description = " magna id feugiat. Suspendisse ullamcorper lectus magna, sed mollis neque ullamcorper aliquet. Donec id nisi nisi. Nullam pharetra, arcu sit amet pharetra fringilla, purus enim molestie mauris, eu viverra ante nunc quis arcu. Aliquam vel lacus eget urna bibendum condimentum et "
// });

// Find all customers whose last name is "Bond"
var results = customers.Where(x => x.Description.Contains("%%mcor%%")).ToList();

Console.WriteLine(results.Count);

[Document(StorageType = StorageType.Json, IndexName = "customer-newidx", Prefixes = new string[] { "customers" })]
public class Customer
{
    [Searchable] public string FullName { get; set; }
    [Searchable] public string Description { get; set; }
    [Indexed] public string FirstName { get; set; }
    [Indexed] public string LastName { get; set; }
    public string Email { get; set; }
    [Indexed(Sortable = true)] public int Age { get; set; }
    [Indexed] public string[] NickNames { get; set; }
}