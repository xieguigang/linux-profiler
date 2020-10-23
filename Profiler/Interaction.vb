Imports System.Runtime.CompilerServices

Public Class Interaction

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function Shell(command As String， args As String) As String
        Return CommandLine.Call(command, args)
    End Function

    Public Shared Function cat(file As String) As String
        Return Shell("cat", args:=file)
    End Function
End Class
