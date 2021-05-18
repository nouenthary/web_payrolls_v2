using System;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for AliasRouting
/// </summary>
public class AliasRouting
{
    public static int RoutedAliasId
    {
        get { return 100001; } //it meant Date: 2012-12-12
    }

    public static int GetOriginalCode(int productRouted)
    {
        return productRouted - RoutedAliasId;
    }
    public static int GetCodeRouting(int productCode)
    {
        return productCode + RoutedAliasId;
    }
    public static string RegSpecialChar(string input)
    {
        var r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        var str = r.Replace(input, String.Empty);
        return Regex.Replace(str, @"\s+", "-").ToLower();
    }
    public static string LowerString(string input)
    {
        return input.ToLower();
    }
}