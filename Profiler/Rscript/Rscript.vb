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
