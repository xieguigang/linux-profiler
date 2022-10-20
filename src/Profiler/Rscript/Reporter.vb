
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
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
End Module
