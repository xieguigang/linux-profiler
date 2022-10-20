#If netcore5 = 0 Then
Imports System.Data.Linq.Mapping
#End If
Imports System.Runtime.CompilerServices
Imports Linux.Commands
Imports Linux.etc
Imports Linux.proc
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.Scripting
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports Rdataframe = SMRUCC.Rsharp.Runtime.Internal.Object.dataframe

''' <summary>
''' package module for get performance data from linux system. you should install ``sysstat``
''' toolkit at first via:
''' 
''' ```bash
''' yum install sysstat -y
''' ```
''' </summary>
<Package("linux", Category:=APICategories.UtilityTools, Publisher:="xie.guigang@gcmodeller.org")>
Module Rscript

    Sub New()
        Internal.Object.Converts.makeDataframe.addHandler(GetType(ps()), AddressOf Rscript.ps)
        Internal.Object.Converts.makeDataframe.addHandler(GetType(mpstat()), AddressOf Rscript.mpstat)
        Internal.Object.Converts.makeDataframe.addHandler(GetType(cpuinfo()), AddressOf Rscript.cpuinfo)
    End Sub

#Region "data frame"

    Private Function ps(list As ps(), args As list, env As Environment) As Rdataframe
        Dim table As New Dictionary(Of String, Array)

        table("USER") = list.Select(Function(p) p.USER).ToArray
        table("PID") = list.Select(Function(p) p.PID).ToArray
        table("%CPU") = list.Select(Function(p) p.CPU).ToArray
        table("%MEM") = list.Select(Function(p) p.MEM).ToArray
        table("VSZ") = list.Select(Function(p) p.VSZ).ToArray
        table("RSS") = list.Select(Function(p) p.RSS).ToArray
        table("TTY") = list.Select(Function(p) p.TTY).ToArray
        table("STAT") = list.Select(Function(p) p.STAT).ToArray
        table("START") = list.Select(Function(p) p.START).ToArray
        table("TIME") = list.Select(Function(p) p.TIME).ToArray
        table("COMMAND") = list.Select(Function(p) p.COMMAND).ToArray

        Return New Rdataframe With {
            .columns = table,
            .rownames = list _
                .Select(Function(p) p.PID.ToHexString.ToUpper) _
                .ToArray
        }
    End Function

    Private Function mpstat(all As mpstat(), args As list, env As Environment) As Rdataframe
        Dim table As New Dictionary(Of String, Array)

        table("time") = all.Select(Function(c) c.time).ToArray
        table("CPU") = all.Select(Function(c) c.CPU).ToArray
        table("%usr") = all.Select(Function(c) c.usr).ToArray
        table("%nice") = all.Select(Function(c) c.nice).ToArray
        table("%sys") = all.Select(Function(c) c.sys).ToArray
        table("%iowait") = all.Select(Function(c) c.iowait).ToArray
        table("%irq") = all.Select(Function(c) c.irq).ToArray
        table("%soft") = all.Select(Function(c) c.soft).ToArray
        table("%steal") = all.Select(Function(c) c.steal).ToArray
        table("%guest") = all.Select(Function(c) c.guest).ToArray
        table("%gnice") = all.Select(Function(c) c.gnice).ToArray
        table("%idle") = all.Select(Function(c) c.idle).ToArray

        Return New Rdataframe With {
            .columns = table,
            .rownames = all _
                .Select(Function(a) "#" & a.CPU) _
                .ToArray
        }
    End Function

    Private Function cpuinfo(list As cpuinfo(), args As list, env As Environment) As Rdataframe
        Dim table As New Dictionary(Of String, Array)
        Dim reader = Schema(Of ColumnAttribute).GetSchema(GetType(cpuinfo), Function(c) c.Name, explict:=True)

        For Each column In reader.Fields
            table(column.Identity) = list _
                .Select(Function(a)
                            Return column.GetValue(a)
                        End Function) _
                .DirectCast(type:=column.Type)
        Next

        Return New Rdataframe With {
            .columns = table,
            .rownames = list _
                .Select(Function(cpu) $"#{cpu.processor}") _
                .ToArray
        }
    End Function

#End Region

    ''' <summary>
    ''' Dmidecode reports information about your system's hardware 
    ''' as described in your system BIOS according to the SMBIOS/DMI 
    ''' standard (see a sample output). This information typically 
    ''' includes system manufacturer, model name, serial number, 
    ''' BIOS version, asset tag as well as a lot of other details of 
    ''' varying level of interest and reliability depending on the 
    ''' manufacturer. This will often include usage status for the 
    ''' CPU sockets, expansion slots (e.g. AGP, PCI, ISA) and memory
    ''' module slots, and the list of I/O ports (e.g. serial, parallel,
    ''' USB).
    '''
    ''' DMI data can be used to enable or disable specific portions 
    ''' of kernel code depending on the specific hardware. Thus, one
    ''' use of dmidecode is for kernel developers to detect system 
    ''' "signatures" and add them to the kernel source code when 
    ''' needed.
    '''
    ''' Beware that DMI data have proven to be too unreliable to be 
    ''' blindly trusted. Dmidecode does not scan your hardware, it 
    ''' only reports what the BIOS told it to.
    '''
    ''' Dmidecode was first written by Alan Cox, then was further 
    ''' developed and is currently maintained again by Jean Delvare,
    ''' after a 5-year interim by Anton Arapov. It is released under 
    ''' the General Public License (GPL). For more details, you 
    ''' should have a look at the AUTHORS and LICENSE files that 
    ''' come with the source code.
    '''
    ''' Three additional tools come with dmidecode:
    '''
    ''' biosdecode prints all BIOS related information it can find
    ''' (see a sample output);
    ''' ownership retrieves the "ownership tag" that can be set on
    ''' Compaq computers;
    ''' vpddecode prints the "vital product data" information that 
    ''' can be found in almost all IBM computers (see a sample 
    ''' output).
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("dmidecode")>
    Public Function dmidecode(Optional file As String = Nothing, Optional env As Environment = Nothing) As dmidecode
        If file.StringEmpty Then
            Return dmidecode.Parse(Interaction.Shell("dmidecode", "", verbose:=env.globalEnvironment.options.verbose))
        Else
            Return dmidecode.Parse(stdout:=file.SolveStream)
        End If
    End Function

    ''' <summary>
    ''' # os-release - Operating system identification
    ''' 
    ''' The /etc/os-release and /usr/lib/os-release files contain operating system identification data.
    '''
    ''' The basic file format of os-release is a newline-separated list of environment-like shell-compatible variable
    ''' assignments. It is possible to source the configuration from shell scripts, however, beyond mere variable
    ''' assignments, no shell features are supported (this means variable expansion is explicitly not supported),
    ''' allowing applications to read the file without implementing a shell compatible execution engine. Variable
    ''' assignment values must be enclosed in double or single quotes if they include spaces, semicolons or other
    ''' special characters outside of A-Z, a-z, 0-9. Shell special characters ("$", quotes, backslash, backtick) must
    ''' be escaped with backslashes, following shell style. All strings should be in UTF-8 format, and non-printable
    ''' characters should not be used. It is not supported to concatenate multiple individually quoted strings. Lines
    ''' beginning with "#" shall be ignored as comments.
    '''
    ''' The file /etc/os-release takes precedence over /usr/lib/os-release. Applications should check for the former,
    ''' and exclusively use its data if it exists, and only fall back to /usr/lib/os-release if it is missing.
    ''' Applications should not read data from both files at the same time.  /usr/lib/os-release is the recommended
    ''' place to store OS release information as part of vendor trees.  /etc/os-release should be a relative symlink
    ''' to /usr/lib/os-release, to provide compatibility with applications only looking at /etc. A relative symlink
    ''' instead of an absolute symlink is necessary to avoid breaking the link in a chroot or initrd environment such
    ''' as dracut.
    '''
    ''' os-release contains data that is defined by the operating system vendor and should generally not be changed by
    ''' the administrator.
    '''
    ''' As this file only encodes names and identifiers it should not be localized.
    '''
    ''' The /etc/os-release and /usr/lib/os-release files might be symlinks to other files, but it is important that
    ''' the file is available from earliest boot on, and hence must be located on the root file system.
    '''
    ''' For a longer rationale for os-release please refer to the Announcement of /etc/os-release.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("os_release")>
    Public Function os_release(Optional file As String = Nothing, Optional env As Environment = Nothing) As os_release
        If file.StringEmpty Then
            Return os_release.Parse(Interaction.cat("/etc/os-release", verbose:=env.globalEnvironment.options.verbose))
        Else
            Return os_release.Parse(stdout:=file.SolveStream)
        End If
    End Function

    ''' <summary>
    ''' On Unix-like operating systems, the uptime command tells 
    ''' you how long the system has been running.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    <ExportAPI("uptime")>
    Public Function uptime(Optional file As String = Nothing, Optional env As Environment = Nothing) As uptime
        If file.StringEmpty Then
            Return uptime.Parse(Interaction.Shell("uptime", "", verbose:=env.globalEnvironment.options.verbose))
        Else
            Return uptime.Parse(stdout:=file.SolveStream)
        End If
    End Function

    ''' <summary>
    ''' This ``/proc/meminfo`` file contains information about 
    ''' the system's memory usage.
    ''' </summary>
    ''' <param name="file">use for debug</param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <ExportAPI("meminfo")>
    Public Function meminfo(Optional file As String = Nothing, Optional env As Environment = Nothing) As meminfo
        If file.StringEmpty Then
            Return meminfo.Parse(Interaction.cat("/proc/meminfo", verbose:=env.globalEnvironment.options.verbose))
        Else
            Return meminfo.Parse(stdout:=file.SolveStream)
        End If
    End Function

    ''' <summary>
    ''' ``/proc/cpuinfo`` contains information about the processor, 
    ''' the Linux system is running on.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <ExportAPI("cpuinfo")>
    Public Function cpuinfo(Optional file As String = Nothing, Optional env As Environment = Nothing) As cpuinfo()
        If file.StringEmpty Then
            Return proc.cpuinfo.Parse(Interaction.cat("/proc/cpuinfo", verbose:=env.globalEnvironment.options.verbose)).ToArray
        Else
            Return proc.cpuinfo.Parse(stdout:=file.SolveStream).ToArray
        End If
    End Function

    ''' <summary>
    ''' Get a snapshot of the processes running in your Linux computer with 
    ''' the ``ps`` command. Locate processes by name, user, or even terminal 
    ''' with as much or as little detail as you need.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    <ExportAPI("ps")>
    Public Function ps(Optional file As String = Nothing, Optional env As Environment = Nothing) As ps()
        If file.StringEmpty Then
            Return Commands.ps.Parse(Interaction.Shell("ps", "-aux", verbose:=env.globalEnvironment.options.verbose)).ToArray
        Else
            Return Commands.ps.Parse(stdout:=file.SolveStream).ToArray
        End If
    End Function

    ''' <summary>
    ''' Report Central Processing Unit (CPU) statistics and input/output 
    ''' statistics for devices, partitions and network filesystems (NFS).
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    <ExportAPI("iostat")>
    Public Function iostat(Optional file As String = Nothing, Optional env As Environment = Nothing) As iostat
        If file.StringEmpty Then
            Return iostat.Parse(Interaction.Shell("iostat", "", verbose:=env.globalEnvironment.options.verbose))
        Else
            Return iostat.Parse(stdout:=file.SolveStream)
        End If
    End Function

    ''' <summary>
    ''' On Unix-like operating systems, the free command displays the total 
    ''' amount of free and used physical and swap memory, and the buffers 
    ''' used by the kernel.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    <ExportAPI("free")>
    Public Function free(Optional file As String = Nothing, Optional env As Environment = Nothing) As free
        If file.StringEmpty Then
            Return free.Parse(Interaction.Shell("free", "", verbose:=env.globalEnvironment.options.verbose))
        Else
            Return free.Parse(stdout:=file.SolveStream)
        End If
    End Function

    ''' <summary>
    ''' The mpstat command displays output activities for each available 
    ''' processor, processor 0 being the first one. Global average activities 
    ''' among all processors are also reported. The mpstat command can be 
    ''' used both on SMP and UP machines, but in the latter, only global 
    ''' average activities will be printed.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    <ExportAPI("mpstat")>
    Public Function mpstat(Optional file As String = Nothing, Optional env As Environment = Nothing) As mpstat()
        If file.StringEmpty Then
            Return Commands.mpstat.Parse(Interaction.Shell("mpstat", "-P ALL", verbose:=env.globalEnvironment.options.verbose)).ToArray
        Else
            Return Commands.mpstat.Parse(stdout:=file.SolveStream).ToArray
        End If
    End Function
End Module
