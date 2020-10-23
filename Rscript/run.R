imports "report" from "Profiler";

new profiler(
	save = `${!script$dir}/test.zip`,
	seconds = 1
) :> start.session
;