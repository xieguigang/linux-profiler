imports "linux" from "Profiler";

iostat :> str;
free :> str;
uptime :> str;
meminfo :> as.list :> str;

cpuinfo :> as.data.frame :> print;
ps :> as.data.frame :> print;
mpstat :> as.data.frame :> print;