Imports Linux
Imports Linux.Report
Imports Microsoft.VisualBasic.ApplicationServices.Zip
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.genomics.GCModeller.Workbench.ReportBuilder.HTML

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

    <ExportAPI("/report")>
    <Usage("/report /snapshots <samples.zip> [/template <template_directory> /out <report_directory>]")>
    Public Function Report(args As CommandLine) As Integer
        Dim in$ = args <= "/snapshots"
        Dim out$ = args("/out") Or $"{[in].TrimSuffix}.profilers_report/"
        Dim tmp As String = App.GetAppSysTempFile("~", App.PID, "snapshots")
        Dim template As String = args <= "/template"

        If Not template.DirectoryExists Then
            template = App.GetAppSysTempFile("~", App.PID, "template")
            Throw New NotImplementedException
        End If

        Call UnZip.ImprovedExtractToDirectory(in$, tmp, Overwrite.Always, extractToFlat:=True)

        Dim summary As ProfilerReport = $"{tmp}/index.json".LoadJSON(Of ProfilerReport)

        Using html As HTMLReport = HTMLReport.CopyTemplate(template, out, SearchOption.SearchTopLevelOnly)
            html("version") = summary.version
            html("release") = summary.release
            html("release_details") = summary.osinfo.toHtml
        End Using

        Return 0
    End Function
End Module
