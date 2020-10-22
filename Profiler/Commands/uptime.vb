Namespace Commands

    Public Class uptime

        ''' <summary>
        ''' The current system time 
        ''' </summary>
        ''' <returns></returns>
        Public Property time As String
        ''' <summary>
        ''' The system has been up
        ''' </summary>
        ''' <returns></returns>
        Public Property uptime As TimeSpan
        ''' <summary>
        ''' user sessions are active on the system.
        ''' </summary>
        ''' <returns></returns>
        Public Property users As Integer

        ''' <summary>
        ''' The average CPU load for the last 1 minute
        ''' </summary>
        ''' <returns></returns>
        Public Property load1 As Double
        ''' <summary>
        ''' The average CPU load for the last 5 minutes
        ''' </summary>
        ''' <returns></returns>
        Public Property load5 As Double
        ''' <summary>
        ''' The average CPU load for the last 15 minutes
        ''' </summary>
        ''' <returns></returns>
        Public Property load15 As Double

        Public Overrides Function ToString() As String
            Return $" {time} up {uptime}, {users} users,  load average: {load1}, {load5}, {load15}"
        End Function

    End Class
End Namespace