Imports System
Imports Linux.Commands

Module Program
    Sub Main(args As String())
        Call Console.WriteLine(uptime.Parse("D:\linux-profiler\Rscript\commands\uptime.txt".ReadAllText))
        Call Console.WriteLine(uptime.Parse("D:\linux-profiler\Rscript\commands\uptime2.txt".ReadAllText))
        Call Console.WriteLine(uptime.Parse("D:\linux-profiler\Rscript\commands\uptime3.txt".ReadAllText))

        Pause()
    End Sub
End Module
