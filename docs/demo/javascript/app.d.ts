/// <reference path="../../dev/linq.d.ts" />
/// <reference path="../../dev/highcharts.d.ts" />
declare namespace app {
    function start(): void;
}
declare namespace apps {
    class overviews {
        private id;
        private overview;
        constructor(activity: {
            datasets: any[];
            xData: number[];
        }, id?: string);
        private mouseEvent;
        /**
         * Synchronize zooming through the setExtremes event handler.
        */
        private syncExtremes;
        private loadLineData;
    }
}
declare namespace report {
    class index extends Bootstrap {
        readonly appName: string;
        private overviews;
        protected init(): void;
    }
}
