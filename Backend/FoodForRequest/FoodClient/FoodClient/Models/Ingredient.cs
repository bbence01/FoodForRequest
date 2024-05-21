public class Ingredient : IEquatable<Ingredient>
{

    public string Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }


    public string FoodId { get; set; }
    public bool Equals(Ingredient other)
    {
        if (other == null) return false;
        return this.Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
