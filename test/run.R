imports "report" from "Profiler";

profiler(
	save = `${!script$dir}/test.zip`,
	seconds = 1
) :> start.session
;