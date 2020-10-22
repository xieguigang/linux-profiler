imports "linux" from "Profiler";

let summary = "D:\linux-profiler\Rscript\proc\meminfo.txt"
:> readText
:> meminfo
:> as.list
;

print("Active file:");
print(summary[["Active(file)"]]);

str(summary);