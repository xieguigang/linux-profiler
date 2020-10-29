Imports System.Data.Linq.Mapping
Imports System.Runtime.CompilerServices
Imports Linux.etc
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.Text.Xml
Imports Microsoft.VisualBasic.Text.Xml.Models

Namespace Report

    Public Module InformationHtml

        <Extension>
        Public Function toHtml(os_release As os_release) As String
            Dim html As New XmlBuilder
            Dim reader = Schema(Of ColumnAttribute).GetSchema(GetType(os_release), Function(c) c.Name, explict:=True)
            Dim getInfo =
                Iterator Function() As IEnumerable(Of XElement)
                    For Each prop As BindProperty(Of ColumnAttribute) In reader.Fields
                        If prop.Identity = NameOf(os_release.metadata) Then
                            Continue For
                        End If

                        Yield <tr>
                                  <td><%= prop.Identity %></td>
                                  <td><%= prop.GetValue(os_release) %></td>
                              </tr>
                    Next

                    If Not os_release.metadata.IsNullOrEmpty Then
                        Yield <tr>
                                  <td>ADDITIONAL</td>
                                  <td>
                                      %s
                                  </td>
                              </tr>
                    End If
                End Function

            html += <thead>
                        <tr>
                            <th>Information</th>
                            <th>Value</th>
                        </tr>
                    </thead>

            html.Wrap(<tbody/>, getInfo().ToArray)

            If Not os_release.metadata.IsNullOrEmpty Then
                html = sprintf(<table class="table">%s</table>, html.Replace("%s", os_release.metadata.metaTable).ToString)
            Else
                html = sprintf(<table class="table">%s</table>, html.ToString)
            End If

            Return html.ToString
        End Function

        <Extension>
        Private Function metaTable(metadata As Dictionary(Of String, String)) As String
            Dim meta As XElement() = metadata _
                .Select(Function(tag)
                            Return <tr>
                                       <td><%= tag.Key %></td>
                                       <td><%= tag.Value %></td>
                                   </tr>
                        End Function) _
                .ToArray

            Return sprintf(<table class="table">
                               %s
                           </table>, meta.Select(Function(xml) xml.ToString).JoinBy(""))
        End Function
    End Module
End Namespace