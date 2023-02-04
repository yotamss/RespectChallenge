namespace Respect.Server.Models;

public class Petition: Entity
{
    public ApplicationUser Owner { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public ICollection<Vote> Votes { get; private set; }

    public Petition()
    {
        
    }
    
    public Petition(ApplicationUser owner, string name, string description)
    {
        Owner = owner;
        Name = name;
        Description = description;
        Votes = new List<Vote>();
    }
}