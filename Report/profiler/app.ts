/// <reference path="../linq.d.ts" />

namespace app {

    export function start() {
        Router.AddAppHandler(new report.index());

        Router.RunApp();
    }
}

$ts.mode = Modes.debug;
$ts(app.start);