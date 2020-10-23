
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("report")>
<RTypeExport("profiler", GetType(Profiler))>
Module Report

    <ExportAPI("start.session")>
    Public Function start_session(profiler As Profiler) As Profiler
        Call "Linux system performance profiler session is started, press Ctrl + C for stop.".__INFO_ECHO
        Call profiler.Run()
        Return profiler
    End Function
End Module
