imports "linux" from "Profiler";

let summary = "D:\linux-profiler\Rscript\commands\ps.txt"
:> readText
:> ps
:> as.data.frame
;

print(summary);