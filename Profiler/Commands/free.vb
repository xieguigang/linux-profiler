Imports System.Data.Linq.Mapping

Namespace Commands

    Public Class free

        Public Property Mem As MemoryFree
        Public Property Swap As SwapFree

        Friend Shared Function Parse(stdout As String) As free
            Dim lines As String() = stdout.Trim.LineTokens.Skip(1).ToArray
            Dim mem = lines(Scan0).StringSplit("\s+").Skip(1).Select(AddressOf Val).ToArray
            Dim swap = lines(1).StringSplit("\s+").Skip(1).Select(AddressOf Val).ToArray
            Dim free1 As New MemoryFree With {
                .total = mem(0),
                .used = mem(1),
                .free = mem(2),
                .[shared] = mem(3),
                .buff_cache = mem(4),
                .available = mem(5)
            }
            Dim free2 As New SwapFree With {
                .total = swap(0),
                .used = swap(1),
                .free = swap(2)
            }

            Return New free With {.Mem = free1, .Swap = free2}
        End Function

    End Class

    Public Class SwapFree

        Public Property total As Double
        Public Property used As Double
        Public Property free As Double

    End Class

    Public Class MemoryFree : Inherits SwapFree

        Public Property [shared] As Double

        <Column(Name:="buff/cache")>
        Public Property buff_cache As Double
        Public Property available As Double

    End Class
End Namespace


