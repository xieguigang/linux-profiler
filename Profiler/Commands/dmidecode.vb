Imports Microsoft.VisualBasic.Language
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop.CType

Namespace Commands

    Public Class dmidecode : Implements ICTypeList

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

        Public Function toList() As list Implements ICTypeList.toList
            Dim listHandles As New list With {.slots = New Dictionary(Of String, Object)}

            For Each handle As dmiHandle In Me.handles
                listHandles.slots(handle.handle) = handle.toList
            Next

            Return New list With {
                .slots = New Dictionary(Of String, Object) From {
                    {"version", version},
                    {"info", info},
                    {"handles", listHandles}
                }
            }
        End Function
    End Class

    Public Class dmiHandle : Implements ICTypeList

        Public Property handle As String
        Public Property DMItype As Integer
        Public Property bytes As Integer
        Public Property name As String
        Public Property info As Dictionary(Of String, String)

        Friend Shared Function Parse(block As String()) As dmiHandle
            Dim info = block(Scan0).StringSplit(",\s*")
            Dim data As New Dictionary(Of String, String)
            Dim value As New List(Of String)
            Dim name As String = Nothing

            For Each line As String In block.Skip(2)
                If line.IndexOf(": ") > -1 Then
                    If Not name.StringEmpty Then
                        data(name) = value.PopAll.JoinBy("; ")
                    End If

                    With line.GetTagValue(":", trim:=True)
                        name = .Name
                        value.Add(.Value)
                    End With
                Else
                    value.Add(line.Trim)
                End If
            Next

            If Not name.StringEmpty Then
                data(name) = value.PopAll.JoinBy("; ")
            End If

            Return New dmiHandle With {
                .handle = info(Scan0).Split().Last,
                .DMItype = Integer.Parse(info(1).Split.Last),
                .bytes = Integer.Parse(info(2).Split.First),
                .name = block(1)
            }
        End Function

        Public Function toList() As list Implements ICTypeList.toList
            Dim info As New Dictionary(Of String, Object)

            For Each item In Me.info
                info.Add(item.Key, item.Value)
            Next

            Return New list With {
                .slots = New Dictionary(Of String, Object) From {
                    {"handle", handle},
                    {"DMI type", DMItype},
                    {"bytes", bytes},
                    {"name", name},
                    {"info", New list With {.slots = info}}
                }
            }
        End Function
    End Class
End Namespace