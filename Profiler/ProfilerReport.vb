Imports Linux.Commands
Imports Linux.etc
Imports Linux.proc

''' <summary>
''' run ``sysstat`` toolkit installation if the command is not found
''' 
''' ``yum install sysstat -y``
''' </summary>
Public Class ProfilerReport

    ''' <summary>
    ''' the linux system version information
    ''' </summary>
    ''' <returns></returns>
    Public Property version As String
    Public Property release As String

    ''' <summary>
    ''' the CPU information
    ''' </summary>
    ''' <returns></returns>
    Public Property cpuinfo As cpuinfo()
    Public Property meminfo As meminfo
    Public Property osinfo As os_release

End Class

Public Class Snapshot

    ''' <summary>
    ''' system load and timespan summary
    ''' </summary>
    ''' <returns></returns>
    Public Property uptime As uptime

    Public Property mpstat As mpstat()
    Public Property free As free
    Public Property iostat As iostat
    Public Property ps As ps()

End Class