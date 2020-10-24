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
        private id;
        private chart;
        private div;
        private psFrames;
        constructor(data: {
            name: string;
            x: number[];
            data: number[];
        }, ps: models.jsFrame<models.ps[]>[], id?: string);
        private lastIndex;
        /**
         * update piechart at here
        */
        private mouseEvent;
        private updatePie;
        private findPsFrame;
        private updatePsFrame;
        private static createPlotOptions;
    }
}
declare namespace models {
    interface jsFrame<T> {
        timeframe: number;
        data: T;
    }
    interface ps {
        USER: string;
        PID: number;
        CPU: number;
        MEM: number;
        VSZ: string;
        RSS: string;
        TTY: string;
        STAT: string;
        START: string;
        TIME: string;
        COMMAND: string;
    }
}
declare namespace report {
    class index extends Bootstrap {
        readonly appName: string;
        private overviews;
        private system_load;
        protected init(): void;
    }
    function orderFrames(ps: models.jsFrame<models.ps[]>[]): models.jsFrame<models.ps[]>[];
}
