using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Administrators;

public class Administrator : Entity<string>
{
    public string AuthProviderId { get; }
    public string Email { get; }
    public string Name { get;  }

    public Administrator(string authProviderId, string email, string name)
    {
        AuthProviderId = authProviderId;
        Email = email;
        Name = name;
    }

    private Administrator() 
    {
        AuthProviderId = default!;
        Email = default!;
        Name = default!;
    }

}
