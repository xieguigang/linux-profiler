
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.ApplicationServices.Zip
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' report helper
''' </summary>
<Package("report")>
<RTypeExport("profiler", GetType(Profiler))>
Module Reporter

    ''' <summary>
    ''' create a new profiler session
    ''' </summary>
    ''' <param name="save">
    ''' the file path location that used for save current profiler session result.
    ''' </param>
    ''' <param name="seconds">
    ''' the sampling update interval
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("profiler")>
    Public Function create_profiler_session(save$, Optional seconds% = 15, Optional title$ = "benchmark") As Profiler
        Return New Profiler(save, seconds, title)
    End Function

    ''' <summary>
    ''' start run the specific sampling profiler session
    ''' </summary>
    ''' <param name="profiler"></param>
    ''' <returns></returns>
    <ExportAPI("start.session")>
    Public Function start_session(profiler As Profiler) As Profiler
        Call "Linux system performance profiler session is started, press Ctrl + C for stop.".__INFO_ECHO
        Call profiler.Run()
        Return profiler
    End Function

    <ExportAPI("create_report")>
    Public Function doReport(snapshotsZip As String, out As String, template As String)
        Dim tmp As String = TempFileSystem.GetAppSysTempFile("~", App.PID, "snapshots")

        If Not template.DirectoryExists Then
            template = TempFileSystem.GetAppSysTempFile("~", App.PID, "template")
            Throw New NotImplementedException
        End If

        Call UnZip.ImprovedExtractToDirectory(snapshotsZip, tmp, Overwrite.Always, extractToFlat:=True)

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
