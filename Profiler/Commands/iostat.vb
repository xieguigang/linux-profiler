Imports System.Data.Linq.Mapping
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Text
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop.CType

Namespace Commands

    Public Class iostat : Implements ICTypeList

        Public Property user As Double
        Public Property nice As Double
        Public Property system As Double
        Public Property iowait As Double
        Public Property steal As Double
        Public Property idle As Double

        Public Property devices As iodev()

        Friend Shared Function Parse(stdout As String) As iostat
            Dim lines As String() = stdout.Trim(" "c, ASCII.TAB, ASCII.CR, ASCII.LF).LineTokens
            Dim avg As Double() = lines(3) _
                .Trim _
                .StringSplit("\s+") _
                .Select(AddressOf Val) _
                .ToArray
            Dim dev As New List(Of iodev)

            For Each line As String In lines.Skip(6)
                Dim data As String() = line.Trim.StringSplit("\s+")

                Call New iodev With {
                    .Device = data(0),
                    .tps = Val(data(1)),
                    .kB_read_sec = Val(data(2)),
                    .kB_wrtn_sec = Val(data(3)),
                    .kB_read = Val(data(4)),
                    .kB_wrtn = Val(data(5))
                }.DoCall(AddressOf dev.Add)
            Next

            Return New iostat With {
                .devices = dev.ToArray,
                .user = avg(0),
                .nice = avg(1),
                .system = avg(2),
                .iowait = avg(3),
                .steal = avg(4),
                .idle = avg(5)
            }
        End Function

        Public Function GetTotalBytesRead() As Double
            Return Aggregate dev As iodev In devices Into Sum(dev.kB_read_sec)
        End Function

        Public Function GetTotalBytesWrite() As Double
            Return Aggregate dev As iodev In devices Into Sum(dev.kB_wrtn_sec)
        End Function

        Public Function GetTotalTps() As Double
            Return Aggregate dev As iodev In devices Into Sum(dev.tps)
        End Function

        Public Function toList() As list Implements ICTypeList.toList
            Dim devList As New list With {
                .slots = devices _
                    .ToDictionary(Function(dev) dev.Device,
                                  Function(dev)
                                      Return CObj(dev.toList)
                                  End Function)
            }

            Return New list With {
                .slots = New Dictionary(Of String, Object) From {
                    {"%user", user},
                    {"%nice", nice},
                    {"%system", system},
                    {"%iowait", iowait},
                    {"%steal", steal},
                    {"%idle", idle},
                    {"devices", devList}
                }
            }
        End Function
    End Class

    Public Class iodev : Implements ICTypeList

        Public Property Device As String
        Public Property tps As Double

        <Column(Name:="kB_read/s")>
        Public Property kB_read_sec As Double

        <Column(Name:="kB_wrtn/s")>
        Public Property kB_wrtn_sec As Double
        Public Property kB_read As Double
        Public Property kB_wrtn As Double

        Public Overrides Function ToString() As String
            Return $"[{Device}] {kB_read_sec} kB_read/s | {kB_wrtn_sec} kB_wrtn/s"
        End Function

        Public Function toList() As list Implements ICTypeList.toList
            Return New list With {
                .slots = New Dictionary(Of String, Object) From {
                    {"device", Device},
                    {"tps", tps},
                    {"kB_read/s", kB_read_sec},
                    {"kB_wrtn/s", kB_wrtn_sec},
                    {"kB_read", kB_read},
                    {"kB_wrtn", kB_wrtn}
                }
            }
        End Function
    End Class

End Namespace
