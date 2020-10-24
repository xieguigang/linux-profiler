imports "linux" from "Profiler";

let summary = "E:\linux-profiler\Rscript\commands\ps.txt"
:> readText
:> ps
:> as.data.frame
;

print(summary);