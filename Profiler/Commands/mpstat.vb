Namespace Commands

    ''' <summary>
    ''' ``mpstat -P ALL``
    ''' </summary>
    Public Class mpstat

        Public Property time As DateTime
        Public Property CPU As String
        Public Property usr As Double
        Public Property nice As Double
        Public Property sys As Double
        Public Property iowait As Double
        Public Property irq As Double
        Public Property soft As Double
        Public Property steal As Double
        Public Property guest As Double
        Public Property gnice As Double
        Public Property idle As Double

        Friend Shared Iterator Function Parse(stdout As String) As IEnumerable(Of mpstat)
            For Each line As String In stdout.Trim.LineTokens.Skip(3)
                Dim data As String() = line.StringSplit("\s+")

                Yield New mpstat With {
                    .time = DateTime.Parse(data(0) & " " & data(1)),
                    .CPU = data(2),
                    .usr = Val(data(3)),
                    .nice = Val(data(4)),
                    .sys = Val(data(5)),
                    .iowait = Val(data(6)),
                    .irq = Val(data(7)),
                    .soft = Val(data(8)),
                    .steal = Val(data(9)),
                    .guest = Val(data(10)),
                    .gnice = Val(data(11)),
                    .idle = Val(data(12))
                }
            Next
        End Function

    End Class

End Namespace
