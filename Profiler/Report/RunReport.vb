Imports System.Runtime.CompilerServices
Imports Linux.Commands
Imports Linux.proc
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.ValueTypes
Imports SMRUCC.genomics.GCModeller.Workbench.ReportBuilder.HTML
Imports SMRUCC.WebCloud.JavaScript.highcharts
Imports SMRUCC.WebCloud.JavaScript.jsTree

Namespace Report

    <HideModuleName>
    Public Module RunReportHandler

        Sub New()

        End Sub

        Public Sub RunReport(template As String, output As String, summary As ProfilerReport, snapshots As Snapshot())
            Using html As HTMLReport = HTMLReport.CopyTemplate(
                source:=template,
                output:=output,
                searchLevel:=SearchOption.SearchTopLevelOnly,
                minify:=True
            )

                Call html.RunReport(summary, snapshots.OrderBy(Function(p) p.timestamp).ToArray)
            End Using
        End Sub

        <Extension>
        Public Sub RunReport(html As HTMLReport, summary As ProfilerReport, snapshots As Snapshot())
            Dim overview As SynchronizedLines = snapshots.SystemLoadOverloads(summary.meminfo)
            Dim system_load = snapshots.system_load
            Dim overviewName = App.GetNextUniqueName("overviews_")
            Dim cpuTreeName = App.GetNextUniqueName("cpu_")
            Dim dmitreeName = App.GetNextUniqueName("dmidecode_")
            Dim system_loadjs = App.GetNextUniqueName("systemload_")
            Dim psName = App.GetNextUniqueName("process_snapshots_")
            Dim output As String = html.directory

            With html
                !version = summary.version
                !release = summary.release
                !release_details = summary.osinfo.toHtml
                !overviews_js = overviewName
                !cpuinfo = cpuTreeName
                !title = summary.title Or "No title".AsDefault
                !time = summary.time.ToString
                !time_span = (snapshots.Last.uptime.uptime - snapshots.First.uptime.uptime).Lanudry
                !dmidecode_version = summary.dmidecode.version
                !logs = summary.dmidecode.info.Select(AddressOf Strings.Trim).JoinBy("<br />")
                !dmidecode = dmitreeName
                !systemload_js = system_loadjs
                !ps_js = psName
            End With

            Call overview.dataJs(overviewName).SaveTo($"{output}/data/overviews.js")
            Call system_load.dataJs(system_loadjs).SaveTo($"{output}/data/system_load.js")
            Call snapshots.psJs(psName).SaveTo($"{output}/data/ps.js")

            Call summary.dmidecode.handles.dmidecodeTree(dmitreeName).SaveTo($"{output}/data/dmidecode.js")
            Call summary.cpuinfo.cpuTree(cpuTreeName).SaveTo($"{output}/data/cpuinfo.js")
        End Sub

        <Extension>
        Private Function psJs(snapshots As Snapshot(), name As String) As String
            Dim base_time = DateTimeHelper.FromUnixTimeStamp(snapshots(Scan0).timestamp)
            Dim psdata = snapshots _
                .Select(Function(a)
                            ' 过滤掉一些轻量的进程，减少视图数据文件大小
                            Dim ps = a.ps _
                                .Where(Function(p) p.CPU > 1 OrElse p.MEM > 1) _
                                .ToArray
                            Dim stamp As Double = (DateTimeHelper.FromUnixTimeStamp(a.timestamp) - base_time).TotalSeconds

                            Return New JsFrame(Of ps()) With {.data = ps, .timeframe = stamp}
                        End Function) _
                .ToArray

            Return psdata.dataJs(name)
        End Function

        <Extension>
        Private Function dmidecodeTree([handles] As dmiHandle(), name$) As String
            Dim tree As New TreeNode With {
                .text = "# dmidecode",
                .icon = "images/gparted.png"
            }

            tree.children = [handles] _
                .Select(Function(handle)
                            Return New TreeNode With {
                                .text = handle.handle,
                                .icon = "images/folder-documents.png",
                                .children = {
                                    New TreeNode With {.icon = "images/application-x-object.png", .text = $"name: {handle.name}"},
                                    New TreeNode With {.icon = "images/application-x-object.png", .text = $"DMI type: {handle.DMItype}"},
                                    New TreeNode With {.icon = "images/application-x-object.png", .text = $"bytes: {handle.bytes}"},
                                    New TreeNode With {
                                        .icon = "images/folder-documents.png",
                                        .text = $"{handle.info.Count} attributes",
                                        .children = handle.info _
                                            .Select(Function(a)
                                                        Return New TreeNode With {
                                                            .icon = "images/application-x-object.png",
                                                            .text = $"{a.Key} = {a.Value}"
                                                        }
                                                    End Function) _
                                            .ToArray
                                    }
                                }
                            }
                        End Function) _
                .ToArray

            Return tree.dataJs(name)
        End Function

        <Extension>
        Private Function cpuTree(cpuinfo As cpuinfo(), name As String) As String
            Dim tree As New TreeNode With {.text = "/proc/cpuinfo", .icon = "images/gparted.png"}
            Dim cpugroup = cpuinfo.GroupBy(Function(c) c.physical_id).ToArray

            tree.children = cpugroup _
                .Select(Function(group)
                            Return New TreeNode With {
                                .text = $"#NUMA node {group.Key}",
                                .icon = "images/folder-documents.png",
                                .children = group _
                                    .OrderBy(Function(cpu) cpu.core_id) _
                                    .Select(Function(cpu)
                                                Return New TreeNode With {
                                                    .text = $"#{cpu.core_id}",
                                                    .icon = "images/folder-documents.png",
                                                    .children = cpu _
                                                        .EnumerateInfo _
                                                        .Select(Function(a)
                                                                    Return New TreeNode With {
                                                                        .text = $"{a.Name}: {a.Value}",
                                                                        .icon = "images/application-x-object.png"
                                                                    }
                                                                End Function) _
                                                        .ToArray
                                                }
                                            End Function) _
                                    .ToArray
                            }
                        End Function) _
                .ToArray

            Return tree.dataJs(name)
        End Function

        <Extension>
        Private Function dataJs(Of T)(data As T, name As String) As String
            Return $"
/**
 * @data {GetType(T).FullName}
 * @assembly {GetType(T).Assembly.FullName}
 * @dll {GetType(T).Assembly.Location.FileName}
*/
function {name}() {{
    return {data.GetJson(knownTypes:={GetType(String), GetType(Double())})};
}}"
        End Function
    End Module
End Namespace

