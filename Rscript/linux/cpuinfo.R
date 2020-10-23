imports "linux" from "Profiler";

let summary = "D:\linux-profiler\Rscript\proc\cpuinfo.txt"
:> readText
:> cpuinfo
:> as.data.frame
:> print
;