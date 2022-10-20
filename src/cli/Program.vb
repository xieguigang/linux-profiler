Imports System.ComponentModel
Imports Linux
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.ApplicationServices.Zip
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/run")>
    <Usage("/run /save <samples.zip> [/title <benchmark> /interval <seconds, default=15>]")>
    <Description("Run benchmark test")>
    Public Function RunProfiler(args As CommandLine) As Integer
        Dim save$ = args <= "/save"
        Dim interval% = args("/interval") Or 15
        Dim title$ = args("/title") Or "benchmark"

        Call Console.WriteLine(Interaction.release)
        Call Console.WriteLine(Interaction.version)

        Return New Profiler(save, interval, title).Run
    End Function

    <ExportAPI("/report")>
    <Usage("/report /snapshots <samples.zip> [/template <template_directory> /out <report_directory>]")>
    Public Function RunReport(args As CommandLine) As Integer
        Dim in$ = args <= "/snapshots"
        Dim out$ = args("/out") Or $"{[in].TrimSuffix}.profilers_report/"
        Dim tmp As String = TempFileSystem.GetAppSysTempFile("~", App.PID, "snapshots")
        Dim template As String = args <= "/template"

        If Not template.DirectoryExists Then
            template = TempFileSystem.GetAppSysTempFile("~", App.PID, "template")
            Throw New NotImplementedException
        End If

        Call UnZip.ImprovedExtractToDirectory(in$, tmp, Overwrite.Always, extractToFlat:=True)

        Dim summary As ProfilerReport = $"{tmp}/index.json".LoadJSON(Of ProfilerReport)
        Dim snapshots As Snapshot() = tmp _
            .EnumerateFiles("*.json") _
            .Where(Function(file) Not file.FileName = "index.json") _
            .Select(Function(file) file.LoadJSON(Of Snapshot)) _
            .ToArray

        Call Report.RunReport(template, out, summary, snapshots)

        Return 0
    End Function
End Module
