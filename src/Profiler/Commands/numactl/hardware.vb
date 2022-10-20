Namespace Commands.numactl

    ''' <summary>
    ''' ``numactl --hardware``
    ''' </summary>
    Public Class hardware

        Public Property numa_nodes As Dictionary(Of String, hardware_node)
        Public Property node_distances As Double()()

    End Class

    Public Class hardware_node

        Public Property cpus As Integer()
        Public Property size As Double
        Public Property free As Double

    End Class
End Namespace