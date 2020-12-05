<Query Kind="Statements" />

var lines = File.ReadAllLines(@"c:\windows\temp\input04.txt").ToList();

if (!string.IsNullOrWhiteSpace(lines.Last()))
{
    lines.Add(""); // add trailing blank to trigger processing
}

var passport = new Dictionary<string,string>();

var validColors = new HashSet<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
bool IsValidYear(string s, int low, int high) => s.Length == 4 && s.All(char.IsDigit) && int.Parse(s) >= low && int.Parse(s) <= high;
bool IsValidHeight(string s)
{
    if (!char.IsDigit(s[0])) return false;
    bool isCm = s.EndsWith("cm");
    bool isIn = s.EndsWith("in");
    if (!isCm && !isIn) return false;
    int digits = s.TakeWhile(char.IsDigit).Count();
    int x =int.Parse(s.Substring(0, digits));
    if (isCm)
    {
        return x >= 150 && x <= 193;
    }
    else // isIn
    {
        return x >= 59 && x <= 76;
    }
}

var validators = new Dictionary<string, Func<string, bool>>
{
    {"byr", s => IsValidYear(s, 1920, 2002) },
    {"iyr", s => IsValidYear(s, 2010, 2020) },
    {"eyr", s => IsValidYear(s, 2020, 2030) },
    {"hgt", IsValidHeight },
    {"hcl", s => Regex.IsMatch(s, "^#[a-fA-F0-9]{6}$")},
    {"ecl", s => validColors.Contains(s) },
    {"pid", s => Regex.IsMatch(s, @"^\d{9}$") }, 
};

int valid = 0;

foreach(var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        if (validators.All(kv => passport.TryGetValue(kv.Key, out var pvalue) && kv.Value(pvalue)))
        {
            valid++;
        }
        passport.Clear();
    }
    else
    {
        var xs = line.Split(' ').Select(x => (key: x.Substring(0, x.IndexOf(':')), value: x.Substring(x.IndexOf(':')+1)));
        foreach(var x in xs)
        {
            passport[x.key] = x.Value;
        }
        
    }
}
valid.Dump(); 