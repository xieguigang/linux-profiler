Namespace Commands.numactl

    Public Class numastat

        Public Property numa_nodes As Dictionary(Of String, numa_node)

    End Class

    Public Class numa_node

        ''' <summary>
        ''' 命中的，也就是为这个节点成功分配本地内存访问的内存大小
        ''' </summary>
        ''' <returns></returns>
        Public Property numa_hit As Double
        ''' <summary>
        ''' 把内存访问分配到另一个node节点的内存大小，这个值和另一个node的numa_foreign相对应。
        ''' </summary>
        ''' <returns></returns>
        Public Property numa_miss As Double
        ''' <summary>
        ''' 另一个Node访问我的内存大小，与对方node的numa_miss相对应
        ''' </summary>
        ''' <returns></returns>
        Public Property numa_foreign As Double
        Public Property interleave_hit As Double
        ''' <summary>
        ''' 这个节点的进程成功在这个节点上分配内存访问的大小
        ''' </summary>
        ''' <returns></returns>
        Public Property local_node As Double
        ''' <summary>
        ''' 这个节点的进程 在其它节点上分配的内存访问大小
        ''' </summary>
        ''' <returns></returns>
        Public Property other_node As Double
    End Class
End Namespace