Imports System.Runtime.CompilerServices
Imports SMRUCC.WebCloud.JavaScript.highcharts
Imports Microsoft.VisualBasic.Linq
Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D
Imports Microsoft.VisualBasic.Math.SignalProcessing
Imports stdNum = System.Math

Namespace Report

    Public Module Chartting

        <Extension>
        Private Function CubicSpline(data As IEnumerable(Of Double), timeline As Double()) As Resampler
            Dim points = data.Select(Function(a, i) New PointF(timeline(i), a)).CubicSpline.ToArray
            Dim x = points.Select(Function(p) CDbl(p.X)).ToArray
            Dim y = points.Select(Function(p) CDbl(p.Y)).ToArray

            Return Resampler.CreateSampler(x, y)
        End Function

        <Extension>
        Private Function Data(sample As Resampler, timeline As Double()) As Double()
            Return sample(timeline).Select(Function(x) stdNum.Round(x, 1)).ToArray
        End Function

        <Extension>
        Public Function SystemLoadOverloads(snapshots As Snapshot()) As SynchronizedLines
            Dim base_time = snapshots(Scan0).uptime.uptime.TotalSeconds
            Dim timeline As Double() = snapshots.Select(Function(a) a.uptime.uptime.TotalSeconds - base_time).ToArray
            Dim cpuRaw = snapshots.Select(Function(a) a.ps.Sum(Function(p) p.CPU)).ToArray
            Dim cpu = cpuRaw.CubicSpline(timeline).Data(timeline)
            Dim memory = snapshots.Select(Function(a) a.free.Mem.used / 1024 / 1024).CubicSpline(timeline).Data(timeline)
            Dim swap = snapshots.Select(Function(a) a.free.Swap.used / 1024 / 1024).CubicSpline(timeline).Data(timeline)
            Dim ioread = snapshots.Select(Function(a) stdNum.Round(a.iostat.GetTotalBytesRead, 1)).ToArray
            Dim iowrite = snapshots.Select(Function(a) stdNum.Round(a.iostat.GetTotalBytesWrite, 1)).ToArray

            Return New SynchronizedLines With {
                .xData = timeline,
                .datasets = {
                    New LineDataSet With {.data = cpu, .name = "CPU", .type = "area", .unit = "%", .valueDecimals = 1},
                    New LineDataSet With {.data = memory, .name = "Memory", .type = "area", .unit = "GB", .valueDecimals = 1},
                    New LineDataSet With {.data = swap, .name = "Swap", .type = "area", .unit = "GB", .valueDecimals = 1},
                    New LineDataSet With {.data = ioread, .name = "I/O read per sec", .type = "line", .unit = "KB/s", .valueDecimals = 1},
                    New LineDataSet With {.data = iowrite, .name = "I/O write per sec", .type = "line", .unit = "KB/s", .valueDecimals = 1}
                }
            }
        End Function
    End Module
End Namespace