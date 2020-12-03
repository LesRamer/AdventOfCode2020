<Query Kind="Statements" />

var lines = File.ReadAllLines("input03.txt");
int len = lines[0].Length;

var rightDownTuples = new [] { (1,1), (3,1), (5, 1), (7,1), (1, 2) };
rightDownTuples.Select(x => Trees(x.Item1, x.Item2)).Aggregate((a,n) => a*n).Dump("product");

long Trees(int right, int down) =>
    lines
        .Where((_, i) => i % down == 0)
        .Select((x, i) => x[(i * right) % len])
        .Count(x => x == '#')
        .Dump($"r = {right}, d = {down}");
