<Query Kind="Statements" />

// Day 05
// start:  10:20
// finish: 10:24 part 1, 10:27 part 2


var query = 
    from line in File.ReadAllLines(@"c:\windows\temp\input05.txt")
    let binary = line.Replace('B', '1').Replace('F', '0').Replace('R', '1').Replace('L', '0')
    select Convert.ToInt32(binary, 2);
    
query.Max().Dump("part 1");


int min = query.Min();
int max = query.Max();

Enumerable.Range(min, max - min + 1).First(x => !query.Contains(x)).Dump("part 2");




