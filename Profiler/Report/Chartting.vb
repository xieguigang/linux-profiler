Imports System.Runtime.CompilerServices
Imports SMRUCC.WebCloud.JavaScript.highcharts
Imports Microsoft.VisualBasic.Linq
Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D
Imports Microsoft.VisualBasic.Math.SignalProcessing

Namespace Report

    Public Module Chartting

        <Extension>
        Private Function CubicSpline(data As IEnumerable(Of Double)) As Resampler
            Dim points = data.SeqIterator.Select(Function(a) New PointF(a.i, a.value)).CubicSpline.ToArray
            Dim x = points.Select(Function(p) CDbl(p.X)).ToArray
            Dim y = points.Select(Function(p) CDbl(p.Y)).ToArray

            Return Resampler.CreateSampler(x, y)
        End Function

        <Extension>
        Public Function SystemLoadOverloads(snapshots As Snapshot()) As SynchronizedLines
            Dim base_time = snapshots(Scan0).uptime.uptime.TotalSeconds
            Dim timeline As Double() = snapshots.Select(Function(a) a.uptime.uptime.TotalSeconds - base_time).ToArray
            Dim cpu = snapshots.Select(Function(a) a.FindAllCPUUsage.usr).CubicSpline
            Dim memory = snapshots.Select(Function(a) a.free.Mem.used / 1024 / 1024).CubicSpline
            Dim swap = snapshots.Select(Function(a) a.free.Swap.used / 1024 / 1024).CubicSpline
            Dim ioread = snapshots.Select(Function(a) a.iostat.GetTotalBytesRead).CubicSpline
            Dim iowrite = snapshots.Select(Function(a) a.iostat.GetTotalBytesWrite).CubicSpline

            Return New SynchronizedLines With {
                .xData = timeline,
                .datasets = {
                    New LineDataSet With {.data = cpu(timeline), .name = "CPU", .type = "line", .unit = "%", .valueDecimals = 1},
                    New LineDataSet With {.data = memory(timeline), .name = "Memory", .type = "line", .unit = "GB", .valueDecimals = 1},
                    New LineDataSet With {.data = swap(timeline), .name = "Swap", .type = "line", .unit = "GB", .valueDecimals = 1},
                    New LineDataSet With {.data = ioread(timeline), .name = "I/O read", .type = "line", .unit = "KB/s", .valueDecimals = 1},
                    New LineDataSet With {.data = iowrite(timeline), .name = "I/O write", .type = "line", .unit = "KB/s", .valueDecimals = 1}
                }
            }
        End Function
    End Module
End Namespace