﻿#If netcore5 = 0 Then
Imports System.Data.Linq.Mapping
#Else
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
#End If

Namespace Commands

    ''' <summary>
    ''' ``ps u``
    ''' </summary>
    Public Class ps

        Public Property USER As String
        Public Property PID As Long

        <Column(Name:="%CPU")>
        Public Property CPU As Double

        <Column(Name:="%MEM")>
        Public Property MEM As Double
        Public Property VSZ As Double
        Public Property RSS As Double
        Public Property TTY As String
        Public Property STAT As String
        Public Property START As String
        Public Property TIME As TimeSpan
        Public Property COMMAND As String

        Public Overrides Function ToString() As String
            Return $"[{PID}] cpu: {CPU}%; {COMMAND}"
        End Function

        Friend Shared Iterator Function Parse(stdout As String) As IEnumerable(Of ps)
            For Each line As String In stdout.LineTokens.Skip(1)
                Dim data As String() = line.StringSplit("\s+")

                Yield New ps With {
                    .USER = data(Scan0),
                    .PID = Long.Parse(data(1)),
                    .CPU = Val(data(2)),
                    .MEM = Val(data(3)),
                    .VSZ = Val(data(4)),
                    .RSS = Val(data(5)),
                    .TTY = data(6),
                    .STAT = data(7),
                    .START = data(8),
                    .TIME = ParseTimeSpan(data(9)),
                    .COMMAND = data.Skip(10).JoinBy(" ")
                }
            Next
        End Function
    End Class
End Namespace
