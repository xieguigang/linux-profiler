Imports System.Runtime.CompilerServices
Imports Linux.Commands
Imports Linux.proc
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports Rdataframe = SMRUCC.Rsharp.Runtime.Internal.Object.dataframe

''' <summary>
''' package module for get performance data from linux system. 
''' </summary>
<Package("linux")>
Module Rscript

    Sub New()
        Internal.Object.Converts.makeDataframe.addHandler(GetType(ps()), AddressOf Rscript.ps)
    End Sub

    Private Function ps(list As ps(), args As list, env As Environment) As Rdataframe
        Dim table As New Dictionary(Of String, Array)

        table("USER") = list.Select(Function(p) p.USER).ToArray
        table("PID") = list.Select(Function(p) p.PID).ToArray
        table("%CPU") = list.Select(Function(p) p.CPU).ToArray
        table("%MEM") = list.Select(Function(p) p.MEM).ToArray
        table("VSZ") = list.Select(Function(p) p.VSZ).ToArray
        table("RSS") = list.Select(Function(p) p.RSS).ToArray
        table("TTY") = list.Select(Function(p) p.TTY).ToArray
        table("STAT") = list.Select(Function(p) p.STAT).ToArray
        table("START") = list.Select(Function(p) p.START).ToArray
        table("TIME") = list.Select(Function(p) p.TIME).ToArray
        table("COMMAND") = list.Select(Function(p) p.COMMAND).ToArray

        Return New Rdataframe With {
            .columns = table,
            .rownames = list _
                .Select(Function(p) p.PID.ToHexString.ToUpper) _
                .ToArray
        }
    End Function

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

    <ExportAPI("ps")>
    Public Function ps(Optional file As String = Nothing) As ps()
        If file.StringEmpty Then
            Return Commands.ps.Parse(Interaction.Shell("ps u")).ToArray
        Else
            Return Commands.ps.Parse(stdout:=file.SolveStream).ToArray
        End If
    End Function
End Module
