
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("report")>
<RTypeExport("profiler", GetType(Profiler))>
Module Reporter

    <ExportAPI("profiler")>
    Public Function create_profiler_session(save$, Optional seconds% = 15) As Profiler
        Return New Profiler(save, seconds)
    End Function

    <ExportAPI("start.session")>
    Public Function start_session(profiler As Profiler) As Profiler
        Call "Linux system performance profiler session is started, press Ctrl + C for stop.".__INFO_ECHO
        Call profiler.Run()
        Return profiler
    End Function
End Module
