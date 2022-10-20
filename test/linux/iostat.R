imports "linux" from "Profiler";

let summary = "D:\linux-profiler\Rscript\commands\iostat.txt"
:> readText
:> iostat
:> as.list
:> str
;