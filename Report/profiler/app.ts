/// <reference path="../dev/linq.d.ts" />
/// <reference path="../dev/highcharts.d.ts" />

namespace app {

    export function start() {
        Router.AddAppHandler(new report.index());

        Router.RunApp();
    }
}

$ts.mode = Modes.debug;
$ts(app.start);