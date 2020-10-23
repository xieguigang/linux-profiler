imports "linux" from "Profiler";

let summary = "D:\linux-profiler\Rscript\commands\mpstat.txt"
:> readText
:> mpstat
:> as.data.frame
:> print
;