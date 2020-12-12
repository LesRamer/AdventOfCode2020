<Query Kind="Statements" />

string[] input = {
    "shiny gold bags contain 2 dark red bags.",
    "dark red bags contain 2 dark orange bags.",
    "dark orange bags contain 2 dark yellow bags.",
    "dark yellow bags contain 2 dark green bags.",
    "dark green bags contain 2 dark blue bags.",
    "dark blue bags contain 2 dark violet bags.",
    "dark violet bags contain no other bags."
    };


var query =
    //from line in input
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

var containedBagQty = query.ToLookup(x => x.top, x => x.contained.Zip(x.qty, (bag, qty) => (bag, qty)));

// query.Dump();

CountBags("shiny gold").Dump("part2");
long CountBags(string bagKind) =>
    containedBagQty[bagKind]
        .SelectMany(x => x)
        .Sum(x => x.qty + x.qty * CountBags(x.bag));
        

    
