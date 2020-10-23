imports "report" from "Profiler";

let session = new profiler(
	save = `${!script$dir}/test.zip`,
	seconds = 1
)
;

session :> start.session;