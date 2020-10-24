Imports System.IO.Compression
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.Utility
Imports Microsoft.VisualBasic.ApplicationServices.Zip
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class Profiler : Implements ITaskDriver

    Dim cancel As Boolean = False
    Dim save As String
    Dim seconds As Integer
    Dim tmp As String = App.GetAppSysTempFile("__", sessionID:=App.PID, prefix:="profiler_tools")
    Dim i As i32 = 1

    Sub New(save As String, Optional seconds As Integer = 15, Optional title$ = "benchmark")
        Me.save = save
        Me.cancel = False
        Me.seconds = seconds

        Call New ProfilerReport With {
            .cpuinfo = proc.cpuinfo.Parse(Interaction.cat("/proc/cpuinfo", verbose:=False)).ToArray,
            .meminfo = proc.meminfo.Parse(Interaction.cat("/proc/meminfo", verbose:=False)),
            .release = Interaction.release,
            .version = Interaction.version,
            .osinfo = etc.os_release.Parse(Interaction.cat("/etc/os-release", verbose:=False)),
            .time = Now,
            .sampling_intervals = seconds,
            .note = App.Command,
            .title = title,
            .dmidecode = Commands.dmidecode.Parse(Interaction.Shell("dmidecode", "", verbose:=False))
        }.GetJson _
         .SaveTo($"{tmp}/index.json")
    End Sub

    Public Function Run() As Integer Implements ITaskDriver.Run
        Dim cancel As New UserTaskCancelAction(Sub() Me.cancel = True)

        Do While Not Me.cancel
            Call sampling()
            Call Thread.Sleep(seconds * 1000)
        Loop

        Return flush()
    End Function

    Private Function flush() As Integer
        Call Zip.DirectoryArchive(
            directory:=tmp,
            saveZip:=save,
            action:=ArchiveAction.Replace,
            fileOverwrite:=Overwrite.Always,
            compression:=CompressionLevel.Fastest,
            flatDirectory:=True
        )

        Return 0
    End Function

    Private Sub sampling()
        Dim snapshot As New Snapshot With {
            .free = Commands.free.Parse(Interaction.Shell("free", "", verbose:=False)),
            .iostat = Commands.iostat.Parse(Interaction.Shell("iostat", "", verbose:=False)),
            .mpstat = Commands.mpstat.Parse(Interaction.Shell("mpstat", "-P ALL", verbose:=False)).ToArray,
            .ps = Commands.ps.Parse(Interaction.Shell("ps", "-aux", verbose:=False)).ToArray,
            .uptime = Commands.uptime.Parse(Interaction.Shell("uptime", "", verbose:=False)),
            .timestamp = App.UnixTimeStamp
        }

        Call Console.WriteLine(snapshot.uptime.ToString)
        Call snapshot.GetJson.SaveTo($"{tmp}/profiles/snapshots_{++i}.json")
    End Sub
End Class
