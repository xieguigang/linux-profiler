Imports System.Runtime.CompilerServices

Public Class Interaction

    ' 20201023
    '
    ' ./linux_run.R --ignore-missing-startup-packages

    ' Loading required package: base
    ' Loading required package: utils
    ' Loading required package: grDevices
    ' Loading required package: stats

    ' Error in <globalEnvironment> -> InitializeEnvironment -> str -> str -> "iostat" -> iostat
    ' 1. TargetInvocationException: Exception has been thrown by the target of an invocation.
    ' 2. Win32Exception: ApplicationName='iostat', CommandLine='', CurrentDirectory='', Native error= Cannot find the specified file
    ' 3. stackFrames: 
    ' at System.Reflection.MonoMethod.Invoke (System.Object obj, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, System.Object[] parameters, System.Globalization.CultureInfo culture) [0x00083] in <6b8bdf3aa3e64e7f91da81ece11b0637>:0 
    ' at System.Reflection.MethodBase.Invoke (System.Object obj, System.Object[] parameters) [0x00000] in <6b8bdf3aa3e64e7f91da81ece11b0637>:0 
    ' at SMRUCC.Rsharp.Runtime.Interop.RMethodInfo.Invoke (System.Object[] parameters, SMRUCC.Rsharp.Runtime.Environment env) [0x000b3] in <b355be0c7d6e431380a05a956aec0fbf>:0 

    ' R# source: Call "str"(Call "iostat"())

    ' linux.R#_interop::.iostat at Profiler.dll:line <unknown>
    ' SMRUCC/R#.call_function."iostat" at linux_run.R:line 3
    ' base.R#_interop::.str at REnv.dll:line <unknown>
    ' SMRUCC/R#.call_function.str at linux_run.R:line n/a
    ' SMRUCC/R#.n/a.InitializeEnvironment at linux_run.R:line 0
    ' SMRUCC/R#.global.<globalEnvironment> at <globalEnvironment>:line n/a

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function Shell(command As String, args As String, verbose As Boolean) As String
        Dim cmdl As String

        If args.StringEmpty Then
            cmdl = command
        Else
            cmdl = $"{command} {args}"
        End If

        If verbose Then
            Call Console.WriteLine("run commandline:")
            Call Console.WriteLine($"/bin/bash -c ""{cmdl}""")
        End If

        Dim stdout As String = CommandLine.Call("/bin/bash", $"-c ""{cmdl}""")

        If verbose Then
            Call Console.WriteLine("std_output:")
            Call Console.WriteLine(stdout)
        End If

        Return stdout
    End Function

    Public Shared Function cat(file As String, verbose As Boolean) As String
        Return Shell("cat", args:=file, verbose)
    End Function
End Class
