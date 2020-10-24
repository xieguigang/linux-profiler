Imports System.Runtime.CompilerServices
Imports Linux.Commands
Imports Linux.proc
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.ValueTypes
Imports SMRUCC.WebCloud.JavaScript.highcharts
Imports stdNum = System.Math

Namespace Report

    Public Module Chartting

        <Extension>
        Private Function IoSpeed(time2 As Snapshot, time1 As Snapshot, isRead As Boolean) As Double
            Dim dt As Double = (DateTimeHelper.FromUnixTimeStamp(time2.timestamp) - DateTimeHelper.FromUnixTimeStamp(time1.timestamp)).TotalSeconds
            Dim data2, data1 As Double

            If isRead Then
                data2 = Aggregate dev As iodev In time2.iostat.devices Into Sum(dev.kB_read)
                data1 = Aggregate dev As iodev In time1.iostat.devices Into Sum(dev.kB_read)
            Else
                data2 = Aggregate dev As iodev In time2.iostat.devices Into Sum(dev.kB_wrtn)
                data1 = Aggregate dev As iodev In time1.iostat.devices Into Sum(dev.kB_wrtn)
            End If

            Return (data2 - data1) / dt
        End Function

        <Extension>
        Private Function IoSpeed(snapshots As Snapshot(), isRead As Boolean) As Double()
            Dim last As Snapshot = snapshots(Scan0)
            Dim data As Double() = New Double(snapshots.Length - 1) {}

            For i As Integer = 0 To snapshots.Length - 1
                data(i) = IoSpeed(snapshots(i), last, isRead) / 1024
                last = snapshots(i)
            Next

            Return data
        End Function

        <Extension>
        Public Function SystemLoadOverloads(snapshots As Snapshot(), meminfo As meminfo) As SynchronizedLines
            Dim base_time As Date = DateTimeHelper.FromUnixTimeStamp(snapshots(Scan0).timestamp)
            Dim timeline As Double() = snapshots.Select(Function(a) (DateTimeHelper.FromUnixTimeStamp(a.timestamp) - base_time).TotalSeconds).ToArray
            Dim cpu = snapshots.Select(Function(a) a.ps.Sum(Function(p) p.CPU)).ToArray
            Dim memory = snapshots.Select(Function(a) a.free.Mem.used / 1024 / 1024).ToArray
            Dim swap = snapshots.Select(Function(a) a.free.Swap.used / 1024 / 1024).ToArray
            Dim ioread = snapshots.IoSpeed(isRead:=True).ToArray
            Dim iowrite = snapshots.IoSpeed(isRead:=False).ToArray
            Dim maxMemory As Double = meminfo.MemTotal / 1024 / 1024
            Dim maxSwap As Double = meminfo.SwapTotal / 1024 / 1024

            Return New SynchronizedLines With {
                .xData = timeline,
                .datasets = {
                    New LineDataSet With {.data = cpu, .name = "CPU", .type = "area", .unit = "%", .valueDecimals = 1},
                    New LineDataSet With {.data = memory, .name = $"Memory ({stdNum.Round(maxMemory, 1)} GB)", .type = "area", .unit = "GB", .valueDecimals = 1, .max = maxMemory},
                    New LineDataSet With {.data = swap, .name = $"Swap ({stdNum.Round(maxSwap, 1)} GB)", .type = "area", .unit = "GB", .valueDecimals = 1, .max = maxSwap},
                    New LineDataSet With {.data = ioread, .name = "I/O read per sec", .type = "line", .unit = "MB/s", .valueDecimals = 1},
                    New LineDataSet With {.data = iowrite, .name = "I/O write per sec", .type = "line", .unit = "MB/s", .valueDecimals = 1}
                }
            }
        End Function

        <Extension>
        Public Function system_load(snapshots As Snapshot()) As Dictionary(Of String, Object)
            Dim base_time As Date = DateTimeHelper.FromUnixTimeStamp(snapshots(Scan0).timestamp)

            Return New Dictionary(Of String, Object) From {
                {"name", "system load"},
                {"data", snapshots.Select(Function(a) a.uptime.load1).ToArray},
                {"x", snapshots.Select(Function(a) (DateTimeHelper.FromUnixTimeStamp(a.timestamp) - base_time).TotalSeconds).ToArray}
            }
        End Function
    End Module
End Namespace