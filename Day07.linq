<Query Kind="Statements" />

var query =
    from line in File.ReadLines(@"c:\windows\temp\input07.txt")
    let match = Regex.Match(line, @"(?<top>[\w ]+) bags (contain ((?<qty>\d+) (?<contained>[\w ]+) bags?(, )?)+|contain no other bags)")
    let top = match.Groups["top"].Value
    let contained = match.Groups["contained"].Captures.Cast<Capture>().Select(c => c.Value)
    let qty = match.Groups["qty"].Captures.Cast<Capture>().Select(c => int.Parse(c.Value))
    select new { top, contained, qty, line };

var holding = query.ToDictionary(x => x.top, x => x.contained.ToHashSet());

var searched = new HashSet<string>();
var bags = new HashSet<string> { "shiny gold" };
var containedBy = new HashSet<string>();
bool done;
do
{
    foreach(var target in bags)
    {
       containedBy.UnionWith(holding.Where(kv => kv.Value.Contains(target)).Select(kv => kv.Key));
    }
    bags = containedBy.Except(searched).ToHashSet();
    searched.UnionWith(containedBy);
    done = !bags.Any();
}
while (!done);

//holding.Count().Dump();
searched.Count().Dump("part1");


query.Dump();
