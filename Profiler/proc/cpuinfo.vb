Imports System.Data.Linq.Mapping
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps

Namespace proc

    Public Class cpuinfo

        Public Property processor As Integer
        Public Property vendor_id As String
        <Column(Name:="cpu family")>
        Public Property cpu_family As String
        Public Property model As String
        <Column(Name:="model name")>
        Public Property model_name As String
        Public Property stepping As String
        Public Property microcode As String
        ''' <summary>
        ''' cpu MHz
        ''' </summary>
        ''' <returns></returns>
        ''' 
        <Column(Name:="cpu MHz")>
        Public Property cpu_MHz As Double
        ''' <summary>
        ''' cache size
        ''' </summary>
        ''' <returns></returns>
        ''' 
        <Column(Name:="cache size")>
        Public Property cache_size As String
        <Column(Name:="physical id")>
        Public Property physical_id As Integer
        Public Property siblings As Integer
        <Column(Name:="core id")>
        Public Property core_id As Integer
        <Column(Name:="cpu cores")>
        Public Property cpu_cores As Integer
        Public Property apicid As Integer
        <Column(Name:="initial apicid")>
        Public Property initial_apicid As Integer
        Public Property fpu As String
        Public Property fpu_exception As String
        <Column(Name:="cpuid level")>
        Public Property cpuid_level As Integer
        Public Property wp As String
        Public Property flags As String()
        Public Property bogomips As Double
        <Column(Name:="clflush size")>
        Public Property clflush_size As Integer
        Public Property cache_alignment As Integer
        <Column(Name:="address sizes")>
        Public Property address_sizes As String
        <Column(Name:="power management")>
        Public Property power_management As String

        Friend Shared Iterator Function Parse(stdout As String) As IEnumerable(Of cpuinfo)
            Dim writer = Schema(Of ColumnAttribute).GetSchema(GetType(cpuinfo), Function(p) p.Name, explict:=True)
            Dim data As Dictionary(Of String, String)
            Dim info As cpuinfo

            For Each block As String() In stdout.LineTokens.Split(Function(line) line.StringEmpty, includes:=False)
                info = New cpuinfo
                data = block _
                    .Select(Function(line)
                                Return line.GetTagValue(":", trim:=True)
                            End Function) _
                    .ToDictionary(Function(t) t.Name.Trim,
                                  Function(t)
                                      Return t.Value
                                  End Function)

                For Each [property] As BindProperty(Of ColumnAttribute) In writer.Fields
                    If [property].Identity = "flags" Then
                        info.flags = data!flags.Split(" "c)
                    Else
                        Call [property].WriteScriptValue(info, data([property].Identity))
                    End If
                Next

                Yield info
            Next
        End Function

    End Class

End Namespace

