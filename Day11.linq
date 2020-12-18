<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
</Query>

string[] input =
#if false
@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL".Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
#else
File.ReadAllLines(@"c:\windows\temp\input11.txt");
#endif

Display(input, "initial");
input[0].Length.Dump("width");
input.Length.Dump("height");

void Display(IEnumerable<string> text, string message = "") => Util.FixedFont(string.Join("\n", text)).Dump(message);

const char floor = '.';
const char emptySeat = 'L';
const char occupied = '#';

string[] prev, next = input;
int looped = 0;
do
{
    prev = next;
    next = SeatPart1(prev);
    ++looped;
}
while (!next.SequenceEqual(prev));

prev.SelectMany(c => c).Count(c => c == occupied).Dump("Part 1");
looped.Dump("looped");

/*
Note: my initial implementation used IEnumerable<string> instead of string[]
for all of the directional seat info. Instead of array indexer expressions
it used LINQ's ElementAt.

While functional, the above required a very long time (to me at least) to
run -- maybe a minute. Adding .ToArray() calls to all the LINQ expressions
reduced the run times to about 4.5 seconds which still seemed like a long time.

Finally, I switched all the expressions to arrays, eliminated most of the
explicit ToArray calls and used array indexer expressions instead of ElementAt. 
The run time was reduced to about 0.6 seconds for the large input file which
requires 118 iterations to reach equilibrium. While this shortened the code, 
it also shifted from pure functional types and expressions to types that 
at least for my sensibilities felt more imperative even though this code
does not mutate them.
*/
string[] SeatPart1(string[] plane)
{
    int width = plane.First().Length;
    string floorEdge = new string(floor, width);
    var left = plane.Select(x => floor + x.Substring(0, width - 1)).ToArray();
    var right = plane.Select(x => x.Substring(1) + floor).ToArray();
    string[] getUp(IEnumerable<string> rows) => rows.Take(width-1).Prepend(floorEdge).ToArray();
    string[] getDown(IEnumerable<string> rows) => rows.Skip(1).Append(floorEdge).ToArray();
    var up = getUp(plane);
    var down = getDown(plane);
    var UL = getUp(left);
    var UR = getUp(right);
    var DL = getDown(left);
    var DR = getDown(right);
    string[][] dirs = { UL, up, UR, right, DR, down, DL, left };
    var query =
        from row in plane.Select((_, i) => i)
        from col in Enumerable.Range(0, width)
        let seat = plane[row][col]
        let occupiedAdj = dirs.Select(dir => dir[row][col]).Count(x => x == occupied)
        group seat == emptySeat && occupiedAdj == 0 ? occupied : seat == occupied && occupiedAdj >= 4 ? emptySeat : seat by row;

    return query.Select(g => string.Concat(g.Select(g1 => g1))).ToArray();
}


/*
   Rewrote seating algorithm for part2. Instead of stopping at the next adjacent
   position (which could be a floor cell), had to continue searching in a ray
   to the next seat in the 8 directions. Accomplished this with tuples that
   function as vectors and positions on the seating grid.
*/
string[] SeatPart2(string[] plane)
{
    int width = plane[0].Length;
    int height = plane.Length;
    (int dr, int dc)[] dirs = new[]
    { 
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1) 
    };
    
    var seats =
        from r in Enumerable.Range(0, height)
        from c in Enumerable.Range(0, width)
        let pos = (r,c)
        let seat = GetSeat(pos)
        let count = dirs.Count(d => GetSeatInDir(pos, d) == occupied)
        group
            seat == floor ? seat :
            seat == occupied ?
                (count >= 5 ? emptySeat : occupied)
                : (count == 0 ? occupied : emptySeat)
        by r;

    return seats.Select(g => string.Concat(g)).ToArray();
    
    char GetSeatInDir((int,int) pos, (int,int) dir)
    {
        var newPos = Add(pos, dir);
        var s = GetSeat(newPos);
        if (s == floor)
        {
            return GetSeatInDir(newPos, dir);
        }
        return s;
    }
        
    (int,int) Add((int,int) a, (int,int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2);
    
    char GetSeat((int, int) pos)
    {
        var (row, col) = pos;
        if (row < 0 || row >= height || col < 0 || col >= width)
        {
            return emptySeat;
        }
        return plane[row][col];
    }
}

next = input;
looped = 0;
do
{
    prev = next;
    next = SeatPart2(prev);
    ++looped;
}
while (!next.SequenceEqual(prev));

prev.SelectMany(c => c).Count(c => c == occupied).Dump("Part 2");
looped.Dump("looped - part 2");

