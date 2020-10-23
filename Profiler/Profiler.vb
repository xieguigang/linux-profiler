Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.Utility
Imports Microsoft.VisualBasic.ComponentModel

Public Class Profiler : Implements ITaskDriver

    Dim cancel As Boolean = False
    Dim save As String
    Dim seconds As Integer

    Sub New(save As String, Optional seconds As Integer = 15)
        Me.save = save
        Me.cancel = False
        Me.seconds = seconds
    End Sub

    Public Function Run() As Integer Implements ITaskDriver.Run
        Dim cancel As New UserTaskCancelAction(Sub() Me.cancel = True)

        Do While Not Me.cancel
            Call sampling()
            Call Thread.Sleep(seconds * 1000)
        Loop

        Return flush()
    End Function

    Private Function flush() As Integer

    End Function

    Private Sub sampling()

    End Sub
End Class
