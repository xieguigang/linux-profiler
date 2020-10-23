Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.genomics.GCModeller.Workbench.ReportBuilder.HTML
Imports SMRUCC.WebCloud.JavaScript.highcharts

Namespace Report

    <HideModuleName>
    Public Module RunReportHandler

        Sub New()

        End Sub

        Public Sub RunReport(template As String, output As String, summary As ProfilerReport, snapshots As Snapshot())
            Dim overview As SynchronizedLines = snapshots.SystemLoadOverloads
            Dim overviewName = App.GetNextUniqueName("overviews_")

            Using html As HTMLReport = HTMLReport.CopyTemplate(template, output, SearchOption.SearchTopLevelOnly)
                html("version") = summary.version
                html("release") = summary.release
                html("release_details") = summary.osinfo.toHtml
                html("overviews_js") = overviewName

                Call overview.dataJs(overviewName).SaveTo($"{output}/data/overviews.js")
            End Using
        End Sub

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

