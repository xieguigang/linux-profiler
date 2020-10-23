Imports System.Runtime.CompilerServices
Imports SMRUCC.WebCloud.JavaScript.highcharts

Namespace Report

    Public Module Chartting

        <Extension>
        Public Function SystemLoadOverloads(snapshots As Snapshot()) As SynchronizedLines
            Dim cpu = snapshots.Select(Function(a) 100 - a.FindAllCPUUsage.idle).ToArray
            Dim memory = snapshots.Select(Function(a) a.free.Mem.used).ToArray
            Dim io = snapshots.Select(Function(a) a.iostat.GetTotalBytesRead).ToArray
            Dim base_time = snapshots(Scan0).uptime.uptime.TotalSeconds

            Return New SynchronizedLines With {
                .xData = snapshots.Select(Function(a) a.uptime.uptime.TotalSeconds - base_time).ToArray,
                .datasets = {
                    New LineDataSet With {.data = cpu, .name = "CPU", .type = "line", .unit = "%", .valueDecimals = 1},
                    New LineDataSet With {.data = memory, .name = "Memory", .type = "line", .unit = "KB", .valueDecimals = 1},
                    New LineDataSet With {.data = io, .name = "I/O", .type = "line", .unit = "KB/s", .valueDecimals = 1}
                }
            }
        End Function
    End Module
End Namespace