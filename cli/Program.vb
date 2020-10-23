Imports Linux
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/run")>
    <Usage("/run /save <samples.zip> [/interval <seconds, default=15>]")>
    Public Function RunProfiler(args As CommandLine) As Integer
        Dim save$ = args <= "/save"
        Dim interval% = args("/interval") Or 15

        Return New Profiler(save, interval).Run
    End Function

End Module
