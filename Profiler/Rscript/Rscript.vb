Imports System.Runtime.CompilerServices
Imports Linux.proc
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("linux")>
Module Rscript

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <ExportAPI("meminfo")>
    Public Function meminfo(file As String) As meminfo
        Return meminfo.Parse(stdout:=file.SolveStream)
    End Function

End Module
