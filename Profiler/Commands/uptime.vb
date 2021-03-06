﻿Imports Microsoft.VisualBasic.Linq
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop.CType

Namespace Commands

    ''' <summary>
    ''' On Unix-like operating systems, the uptime command tells you how long the system has been running.
    ''' </summary>
    Public Class uptime : Implements ICTypeList

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

        Public Shared Function Parse(stdout As String) As uptime
            Dim tokens = stdout.StringSplit("(\s*,\s*)|(\s*up\s*)")
            Dim uptime As TimeSpan
            Dim offset As Integer = 0

            If tokens(2).IsPattern("\d+[:]\d+") Then
                Dim hhmm As String() = tokens(2).Split(":"c)

                uptime = TimeSpan.FromHours(Val(hhmm(0))) + TimeSpan.FromMinutes(Val(hhmm(1)))
                offset = -2
            ElseIf tokens(2).IsPattern("\d+\s*min") Then
                uptime = TimeSpan.FromMinutes(Integer.Parse(tokens(2).Match("\d+")))
                offset = -2
            Else
                uptime = TimeSpan.FromDays(Integer.Parse(tokens(2).Match("\d+"))) + ParseTimeSpan(tokens(4))
            End If

            Return New uptime With {
                .time = Strings.Trim(tokens(Scan0)),
                .uptime = uptime,
                .users = tokens(6 + offset).Match("\d+").DoCall(AddressOf Integer.Parse),
                .load1 = tokens(8 + offset).Split(":"c).Last.DoCall(AddressOf Val),
                .load5 = Val(tokens(10 + offset)),
                .load15 = Val(tokens(12 + offset))
            }
        End Function

        Public Function toList() As list Implements ICTypeList.toList
            Return New list With {
                .slots = New Dictionary(Of String, Object) From {
                    {"time", time},
                    {"uptime", uptime},
                    {"users", users},
                    {"load <1min", load1},
                    {"load <5min", load5},
                    {"load <15min", load15}
                }
            }
        End Function
    End Class
End Namespace