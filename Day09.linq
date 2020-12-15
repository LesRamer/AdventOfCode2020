<Query Kind="Statements" />

int preamble = 25;

var input = File.ReadLines(@"c:\windows\temp\input09.txt").Select(long.Parse).ToArray();

var alg =
    input.Skip(preamble).Select((x,i) => (x, xs:input.Skip(i).Take(preamble)));
    
var part1 = alg.First(z => !z.xs.Any(x => z.xs.Contains(z.x-x))).x.Dump("Part1");
input.Skip(386).Take(403-386).Dump().Sum().Dump();


for(int i = 0; i < input.Length; i++)
{
    long sum = input[i];
    for(int j = i + 1; j < input.Length; j++)
    {
        sum += input[j];
        if (sum == part1)
        {
            var xs = input.Skip(i).Take(j - i + 1);
            (xs.Min() + xs.Max()).Dump("Part 2");
            return;
        }
        if (sum > part1)
        {
            break;
        }
    }
}

