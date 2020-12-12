<Query Kind="Statements" />

var progInput =
    (from line in File.ReadLines(@"c:\windows\temp\input08.txt")
    let s = line.Split(' ')
    select (op:s[0], arg:int.Parse(s[1]))).ToArray();
    
Execute(progInput).First(x => x.infinite).acc.Dump("Part 1");

var modifiedPrograms = 
    from index in progInput.Select((x,i) => x.op == "nop" || x.op == "jmp" ? i : -1)
    where index >= 0
    select Execute(progInput.Select((x,i) => index == i ? (x.op == "nop" ? "jmp" : "nop", x.arg) : x).ToArray()).Last();

modifiedPrograms.First(x => !x.infinite).acc.Dump("Part 2");


IEnumerable<(int ip, int acc, string op, int val, bool infinite)> Execute(IEnumerable<(string op, int arg)> prog)
{
    int length = prog.Count();
    var visited = new HashSet<int>();
    int acc = 0;
    for(int instructionPointer = 0; instructionPointer != length;)
    {
        var (op, arg) = prog.ElementAt(instructionPointer);
        bool infinite = !visited.Add(instructionPointer);
        yield return (instructionPointer, acc, op, arg, infinite);
        if (infinite)
        {
            yield break;
        }
        switch (op)
        {
            case "acc": acc += arg; instructionPointer++; break;
            case "jmp": instructionPointer += arg; break;
            default: instructionPointer++; break;
        }
    }
}


