Namespace Commands

    Public Class dmidecode

        Public Property version As String
        Public Property info As String()
        Public Property [handles] As dmiHandle()

        Friend Shared Function Parse(stdout As String) As dmidecode
            Dim data = stdout.Trim.LineTokens.Split(Function(line) line.StringEmpty, includes:=False).ToArray
            Dim info As String() = data(Scan0)
            Dim [handles] As New List(Of dmiHandle)

            For Each block As String() In data.Skip(1)
                Call [handles].Add(dmiHandle.Parse(block))
            Next

            Return New dmidecode With {
                .[handles] = [handles].ToArray,
                .info = info.Skip(1).ToArray,
                .version = info(Scan0).Match("\d+(\.\d+)+")
            }
        End Function
    End Class

    Public Class dmiHandle
        Public Property handle As String
        Public Property DMItype As Integer
        Public Property bytes As Integer
        Public Property name As String
        Public Property info As Dictionary(Of String, String)

        Friend Shared Function Parse(block As String()) As dmiHandle
            Dim info = block(Scan0).StringSplit(",\s*")

            Return New dmiHandle With {
                .handle = info(Scan0).Split().Last,
                .DMItype = info(1).Split.Last,
                .bytes = info(2).Split.First
            }
        End Function
    End Class
End Namespace