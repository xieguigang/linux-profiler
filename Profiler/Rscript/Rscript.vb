Imports System.Runtime.CompilerServices
Imports Linux.Commands
Imports Linux.proc
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

''' <summary>
''' package module for get performance data from linux system. 
''' </summary>
<Package("linux")>
Module Rscript

    <ExportAPI("uptime")>
    Public Function uptime(Optional file As String = Nothing) As uptime
        If file.StringEmpty Then
            Return uptime.Parse(Interaction.Shell("uptime"))
        Else
            Return uptime.Parse(stdout:=file.SolveStream)
        End If
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <ExportAPI("meminfo")>
    Public Function meminfo(file As String) As meminfo
        Return meminfo.Parse(stdout:=file.SolveStream)
    End Function

End Module
