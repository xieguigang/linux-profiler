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
declare namespace apps {
    class system_load {
        private chart;
        private div;
        constructor(data: {
            name: string;
            data: number[];
        }, id?: string);
        private lastIndex;
        /**
         * update piechart at here
        */
        private mouseEvent;
        private updatePie;
        private static createPlotOptions;
    }
}
declare namespace report {
    class index extends Bootstrap {
        readonly appName: string;
        private overviews;
        private system_load;
        protected init(): void;
    }
}
