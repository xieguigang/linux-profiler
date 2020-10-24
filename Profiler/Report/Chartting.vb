Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Linux.proc
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.SignalProcessing
Imports SMRUCC.WebCloud.JavaScript.highcharts
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
        Public Function SystemLoadOverloads(snapshots As Snapshot(), meminfo As meminfo) As SynchronizedLines
            Dim base_time = snapshots(Scan0).uptime.uptime.TotalSeconds
            Dim timeline As Double() = snapshots.Select(Function(a) a.uptime.uptime.TotalSeconds - base_time).ToArray
            Dim cpuRaw = snapshots.Select(Function(a) a.ps.Sum(Function(p) p.CPU)).ToArray
            Dim cpu = cpuRaw.CubicSpline(timeline).Data(timeline)
            Dim memory = snapshots.Select(Function(a) a.free.Mem.used / 1024 / 1024).CubicSpline(timeline).Data(timeline)
            Dim swap = snapshots.Select(Function(a) a.free.Swap.used / 1024 / 1024).CubicSpline(timeline).Data(timeline)
            Dim ioread = snapshots.Select(Function(a) stdNum.Round(a.iostat.GetTotalBytesRead, 1)).ToArray
            Dim iowrite = snapshots.Select(Function(a) stdNum.Round(a.iostat.GetTotalBytesWrite, 1)).ToArray
            Dim maxMemory As Double = meminfo.MemTotal / 1024 / 1024
            Dim maxSwap As Double = meminfo.SwapTotal / 1024 / 1024

            Return New SynchronizedLines With {
                .xData = timeline,
                .datasets = {
                    New LineDataSet With {.data = cpu, .name = "CPU", .type = "area", .unit = "%", .valueDecimals = 1},
                    New LineDataSet With {.data = memory, .name = $"Memory ({stdNum.Round(maxMemory, 1)} GB)", .type = "area", .unit = "GB", .valueDecimals = 1, .max = maxMemory},
                    New LineDataSet With {.data = swap, .name = $"Swap ({stdNum.Round(maxSwap, 1)} GB)", .type = "area", .unit = "GB", .valueDecimals = 1, .max = maxSwap},
                    New LineDataSet With {.data = ioread, .name = "I/O read per sec", .type = "line", .unit = "KB/s", .valueDecimals = 1},
                    New LineDataSet With {.data = iowrite, .name = "I/O write per sec", .type = "line", .unit = "KB/s", .valueDecimals = 1}
                }
            }
        End Function

        <Extension>
        Public Function system_load(snapshots As Snapshot()) As Dictionary(Of String, Object)
            Return New Dictionary(Of String, Object) From {
                {"name", "system load"},
                {"data", snapshots.Select(Function(a) a.uptime.load1).ToArray}
            }
        End Function
    End Module
End Namespace