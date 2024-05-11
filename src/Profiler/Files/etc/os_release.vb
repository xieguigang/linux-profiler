Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop.CType

Namespace etc

    ''' <summary>
    ''' cat /etc/os-release
    ''' </summary>
    Public Class os_release : Implements ICTypeList

        Public Property NAME As String
        Public Property VERSION As String
        Public Property ID As String
        Public Property ID_LIKE As String
        Public Property VERSION_ID As String
        Public Property PRETTY_NAME As String
        Public Property ANSI_COLOR As String
        Public Property CPE_NAME As String
        Public Property HOME_URL As String
        Public Property BUG_REPORT_URL As String

        Public Property metadata As Dictionary(Of String, String)

        Public Function toList() As list Implements ICTypeList.toList
            Return New list With {
                .slots = New Dictionary(Of String, Object) From {
                    {"NAME", NAME},
                    {"VERSION", VERSION},
                    {"ID", ID},
                    {"ID_LIKE", ID_LIKE},
                    {"VERSION_ID", VERSION_ID},
                    {"PRETTY_NAME", PRETTY_NAME},
                    {"ANSI_COLOR", ANSI_COLOR},
                    {"CPE_NAME", CPE_NAME},
                    {"HOME_URL", HOME_URL},
                    {"BUG_REPORT_URL", BUG_REPORT_URL},
                    {"metadata", New list With {
                        .slots = metadata _
                            .ToDictionary(Function(t) t.Key,
                                          Function(t)
                                              Return CObj(t.Value)
                                          End Function)
                        }
                    }
                }
            }
        End Function

        Friend Shared Function Parse(stdout As String) As os_release
            Dim lines As String()() = stdout _
                .LineTokens _
                .Split(Function(line) line.StringEmpty, includes:=False) _
                .ToArray
            Dim writer = Schema(Of ColumnAttribute).GetSchema(GetType(os_release), Function(c) c.Name, explict:=True)
            Dim info As New os_release
            Dim onError As Boolean = False

            For Each tag As NamedValue(Of String) In lines(Scan0) _
                .Select(Function(line)
                            Return line.GetTagValue("=", trim:=""""c)
                        End Function)

                If Not writer.Write(tag.Name, info, tag.Value) Then
                    onError = True
                    VBDebugger.Echo($"error while set value to: {tag.Name}={tag.Value}")
                End If
            Next

            If lines.Length > 1 Then
                ' centos
                info.metadata = lines(1) _
                    .Select(Function(line)
                                Return line.GetTagValue("=", trim:=""""c)
                            End Function) _
                    .ToDictionary(Function(m) m.Name,
                                  Function(m)
                                      Return m.Value
                                  End Function)
            End If

            If onError Then
                Call $"set value error for os_release:{vbCrLf}{vbCrLf}{stdout}".Warning
            End If

            Return info
        End Function
    End Class
End Namespace