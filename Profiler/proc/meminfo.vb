Imports System.Data.Linq.Mapping
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps

Namespace proc

    Public Class meminfo

        Public Property MemTotal As Double
        Public Property MemFree As Double
        Public Property MemAvailable As Double
        Public Property Buffers As Double
        Public Property Cached As Double
        Public Property SwapCached As Double
        Public Property Active As Double
        Public Property Inactive As Double
        <Column(Name:="Active(anon)")>
        Public Property Active_anon As Double
        <Column(Name:="Inactive(anon)")>
        Public Property Inactive_anon As Double
        <Column(Name:="Active(file)")>
        Public Property Active_file As Double
        <Column(Name:="Inactive(file)")>
        Public Property Inactive_file As Double
        Public Property Unevictable As Double
        Public Property Mlocked As Double
        Public Property SwapTotal As Double
        Public Property SwapFree As Double
        Public Property Dirty As Double
        Public Property Writeback As Double
        Public Property AnonPages As Double
        Public Property Mapped As Double
        Public Property Shmem As Double
        Public Property Slab As Double
        Public Property SReclaimable As Double
        Public Property SUnreclaim As Double
        Public Property KernelStack As Double
        Public Property PageTables As Double
        Public Property NFS_Unstable As Double
        Public Property Bounce As Double
        Public Property WritebackTmp As Double
        Public Property CommitLimit As Double
        Public Property Committed_AS As Double
        Public Property VmallocTotal As Double
        Public Property VmallocUsed As Double
        Public Property VmallocChunk As Double
        Public Property HardwareCorrupted As Double
        Public Property AnonHugePages As Double
        Public Property CmaTotal As Double
        Public Property CmaFree As Double
        Public Property HugePages_Total As Double
        Public Property HugePages_Free As Double
        Public Property HugePages_Rsvd As Double
        Public Property HugePages_Surp As Double
        Public Property Hugepagesize As Double
        Public Property DirectMap4k As Double
        Public Property DirectMap2M As Double
        Public Property DirectMap1G As Double

        Friend Shared Function Parse(stdout As String) As meminfo
            Dim outputLines As String() = stdout.LineTokens
            Dim table As Dictionary(Of String, Double) = outputLines _
                .Select(Function(line)
                            Return line.GetTagValue(":", trim:=True)
                        End Function) _
                .ToDictionary(Function(a) a.Name,
                              Function(a)
                                  Return Val(a.Value.Split.First)
                              End Function)
            Dim mem As Object = New meminfo
            Dim writer = Schema(Of ColumnAttribute).GetSchema(GetType(meminfo), Function(c) c.Name, explict:=True)

            For Each [property] As BindProperty(Of ColumnAttribute) In writer.Fields
                [property].SetValue(mem, table([property].Identity))
            Next

            Return mem
        End Function

    End Class
End Namespace