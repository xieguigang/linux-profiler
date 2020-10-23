imports "linux" from "Profiler";

options(verbose = FALSE);

iostat() :> str;
free() :> as.list :> str;
uptime() :> as.list :> str;
meminfo() :> as.list :> str;

cpuinfo() :> as.data.frame :> print;
ps() :> as.data.frame :> print;
mpstat() :> as.data.frame :> print;