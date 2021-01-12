Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
#if netcore5=0 then
' 有关程序集的一般信息由以下
' 控制。更改这些特性值可修改
' 与程序集关联的信息。

'查看程序集特性的值

<Assembly: AssemblyTitle("performance diagnose tool of LINUX system")>
<Assembly: AssemblyDescription("performance diagnose tool of LINUX system")>
<Assembly: AssemblyCompany("xieguigang")>
<Assembly: AssemblyProduct("Profiler")>
<Assembly: AssemblyCopyright("Copyright © I@xieguigang.me 2020")>
<Assembly: AssemblyTrademark("Profiler")>

<Assembly: ComVisible(False)>

'如果此项目向 COM 公开，则下列 GUID 用于 typelib 的 ID
<Assembly: Guid("4c8617d4-b78d-4d51-80a2-d0360cdadf26")>

' 程序集的版本信息由下列四个值组成: 
'
'      主版本
'      次版本
'      生成号
'      修订号
'
'可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值
'通过使用 "*"，如下所示:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.563.*")>
<Assembly: AssemblyFileVersion("1.1.*")>
#end if