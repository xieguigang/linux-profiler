require(LinuxProfiler);

#' Run benchmark test

imports "report" from "LinuxProfiler";

const profiler_sample as string = ?"--samples" || stop("The profiler sampleing data zip file must be provided!");
const outputdir as string = ?"--outputdir" || `${dirname(profiler_sample)}/${basename(profiler_sample)}_report/`;

create_report(
	snapshotsZip = profiler_sample, 
	out = outputdir, 
	template = `${system.file("data", package = "LinuxProfiler")}/html/`
);