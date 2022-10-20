require(LinuxProfiler);

#' Run benchmark test

imports "report" from "LinuxProfiler";

[@info "A zip file path for save the profiler session sampling result data."]
const report_sample_save as string = ?"--save" || stop("A zip file path must be provided to specific the session data save location!");
[@info "the report title."]
const title as string = ?"--title" || "benchmark";
[@info "the time interval for run profiler sampling, time unit in seconds."]
const interval as integer = ?"--interval" || 15;

report_sample_save
|> profiler(
    seconds = interval, 
    title = title
)
|> start.session()
;