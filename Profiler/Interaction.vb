Public Class Interaction

    Public Shared Function Shell(command As String) As String

    End Function

    Public Shared Function cat(file As String) As String
        Return Shell($"cat ""{file}""")
    End Function
End Class
