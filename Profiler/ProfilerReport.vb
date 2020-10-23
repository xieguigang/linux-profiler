Imports Linux.Commands
Imports Linux.proc

''' <summary>
''' run ``sysstat`` toolkit installation if the command is not found
''' 
''' ``yum install sysstat -y``
''' </summary>
Public Class ProfilerReport

    Public Property info As Summary
    Public Property profiles As Snapshot()

End Class

Public Class Summary

    ''' <summary>
    ''' the linux system version information
    ''' </summary>
    ''' <returns></returns>
    Public Property version As String
    ''' <summary>
    ''' the CPU information
    ''' </summary>
    ''' <returns></returns>
    Public Property CPU As String

End Class

Public Class Snapshot

    ''' <summary>
    ''' system load and timespan summary
    ''' </summary>
    ''' <returns></returns>
    Public Property uptime As uptime
    Public Property meminfo As meminfo

End Class