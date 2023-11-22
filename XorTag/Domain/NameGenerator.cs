namespace XorTag.Domain;

public interface INameGenerator
{
    string GenerateName(IEnumerable<string> existingNames);
}

public class NameGenerator : INameGenerator
{
    public static IEnumerable<string> AllNames => allNames;
    private static readonly List<string> allNames = new List<string> { "Gimli", "Fred", "Ralph", "George", "Frodo", "Gandalf", "Henry", "Lloyd", "Nina", "Verlene", "Tanika", "Corrine", "Tamra", "Racquel", "Luz", "Charla", "Zelma", "Harriette", "Onie", "Colin", "Lisbeth", "Hyman", "Apolonia", "Theda", "Paulita", "Kathryne", "Cathrine", "Sharolyn", "Nan", "Humberto", "Zachery", "Lura", "Christoper", "Sixta", "Amos", "Coreen", "Remedios", "Lyman", "Gustavo", "Aleen", "Dora", "Dewey", "Ricky", "Darth Vader", "Boba Fett", "Han Solo", "Godzilla", "Optimus", "Chewbacca", "Yoda", "Leia", "Spock", "Bones", "Kirk", "HAL-9000", "Data", "Riker", "Worf", "Picard", "Obi-Wan", "Venkman", "Ray", "Egon", "Winston", "Dr. Brown", "Bilbo", "Link", "Zelda", "Scotty", "Mario", "Luigi", "Yoshi", "Bowser", "Q", "R2-D2", "C-3PO", "Saruman", "Samwise", "Arwen", "Gollum", "Legolas", "Thorin", "Merlin", "Elrond", "Eowyn", "Eomer", "Dobby", "Johnny 5", "KITT", "Mega Man", "Marvin", "Astro", "ASIMO", "GLaDOS", "Wall-E" };
    private static readonly Random rand = new Random();

    public string GenerateName(IEnumerable<string> existingNames)
    {
        var randomizedNames = allNames.OrderBy(x => rand.Next(allNames.Count)).ToList();
        string generatedName = randomizedNames.First();
        foreach (var name in randomizedNames)
        {
            generatedName = name;
            if (!existingNames.Contains(generatedName)) break;
        }
        return generatedName;
    }
}