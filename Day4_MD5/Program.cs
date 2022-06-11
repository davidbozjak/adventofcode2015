using System.Text;

string secretKey = "yzbqklnj";

var stringsToFind = new[] { 5, 6 }.Select(w => new string(Enumerable.Repeat('0', w).ToArray())).ToList();

var toRemove = new List<string>();

using (var md5 = System.Security.Cryptography.MD5.Create())
{
    for (long i = 1; stringsToFind.Count > 0 ; i++)
    {
        var input = Encoding.ASCII.GetBytes(secretKey + i.ToString());
        var hashbytes = md5.ComputeHash(input);

        var hexRepresentation = Convert.ToHexString(hashbytes);

        foreach (var stringToFind in stringsToFind)
        {
            var str = hexRepresentation[..stringToFind.Length];

            if (str == stringToFind)
            {
                Console.WriteLine($"Found {str} on {i}");
                toRemove.Add(str);
            }
        }

        if (toRemove.Any())
        {
            stringsToFind.RemoveAll(toRemove.Contains);
            toRemove = new();
        }
    }
}