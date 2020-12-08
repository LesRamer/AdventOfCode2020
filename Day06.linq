<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
</Query>

// start 11:51
var query =
    from g in File.ReadLines(@"c:\windows\temp\input06.txt").Split("")
    let count = g.Count()
    from c in g.Aggregate<IEnumerable<char>>((a, b) => a.Union(b))
    group count by c into g2
    select new { c = g2.Key, sum = g2.Count() };

query.Sum(x => x.sum).Dump("part 1"); // end 12:13

var query2 =
    from g in File.ReadLines(@"c:\windows\temp\input06.txt").Split("")
    let count = g.Count()
    from c in g.Aggregate<IEnumerable<char>>((a, b) => a.Intersect(b))
    group count by c into g2
    select new { c = g2.Key, sum = g2.Count() };

query2.Sum(x => x.sum).Dump("part 2"); // end 12:15


// cleanup
var counts =
    File
        .ReadLines(@"c:\windows\temp\input06.txt")
        .Split("")
//        .Select(g => g.Aggregate<IEnumerable<char>>((a, b) => a.Union(b)).Count()) // part 1
        .Select(g => g.Aggregate<IEnumerable<char>>((a, b) => a.Intersect(b)).Count()) // part 2
        .Sum();

counts.Dump();
