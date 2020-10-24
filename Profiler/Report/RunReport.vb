﻿Imports System.Runtime.CompilerServices
Imports jsTree
Imports Linux.proc
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.genomics.GCModeller.Workbench.ReportBuilder.HTML
Imports SMRUCC.WebCloud.JavaScript.highcharts

Namespace Report

    <HideModuleName>
    Public Module RunReportHandler

        Sub New()

        End Sub

        Public Sub RunReport(template As String, output As String, summary As ProfilerReport, snapshots As Snapshot())
            Dim overview As SynchronizedLines = snapshots.SystemLoadOverloads(summary.meminfo)
            Dim overviewName = App.GetNextUniqueName("overviews_")
            Dim cpuTreeName = App.GetNextUniqueName("cpu_")

            Using html As HTMLReport = HTMLReport.CopyTemplate(
                source:=template,
                output:=output,
                searchLevel:=SearchOption.SearchTopLevelOnly,
                minify:=True
            )
                html("version") = summary.version
                html("release") = summary.release
                html("release_details") = summary.osinfo.toHtml
                html("overviews_js") = overviewName
                html("cpuinfo") = cpuTreeName
                html("title") = summary.title Or "No title".AsDefault
                html("time") = summary.time.ToString
                html("time_span") = (snapshots.Last.uptime.uptime - snapshots.First.uptime.uptime).FormatTime
                html("dmidecode_version") = summary.dmidecode.version
                html("logs") = summary.dmidecode.info.Select(AddressOf Strings.Trim).JoinBy("<br />")

                Call summary.cpuinfo.cpuTree(cpuTreeName).SaveTo($"{output}/data/cpuinfo.js")
                Call overview.dataJs(overviewName).SaveTo($"{output}/data/overviews.js")
            End Using
        End Sub

        Private Function dmidecodeTree() As String

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
*/
function {name}() {{
    return {data.GetJson};
}}"
        End Function
    End Module
End Namespace

