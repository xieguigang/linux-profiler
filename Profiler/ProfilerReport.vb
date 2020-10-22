Imports Linux.Commands
Imports Linux.proc

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