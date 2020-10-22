Imports System.Runtime.CompilerServices
Imports Linux.proc
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("linux")>
Module Rscript

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="file">use for debug</param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <ExportAPI("meminfo")>
    Public Function meminfo(Optional file As String = Nothing) As meminfo
        If file.StringEmpty Then
            Return meminfo.Parse(Interaction.cat("/proc/meminfo"))
        Else
            Return meminfo.Parse(stdout:=file.SolveStream)
        End If
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <ExportAPI("cpuinfo")>
    Public Function cpuinfo(Optional file As String = Nothing) As cpuinfo()
        If file.StringEmpty Then
            Return proc.cpuinfo.Parse(Interaction.cat("/proc/cpuinfo")).ToArray
        Else
            Return proc.cpuinfo.Parse(stdout:=file.SolveStream).ToArray
        End If
    End Function
End Module
