using System.Text;

string secretKey = "yzbqklnj";

var stringsToFind = new List<string>() { new string(Enumerable.Repeat('0', 5).ToArray()), new string(Enumerable.Repeat('0', 6).ToArray()) };

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
                stringsToFind.Remove(str);
                break;
            }
        }
    }
}