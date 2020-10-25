/// <reference path="../../dev/linq.d.ts" />
/// <reference path="../../dev/highcharts.d.ts" />
declare namespace app {
    function start(): void;
}
declare namespace apps {
    class overviews {
        private id;
        private overview;
        constructor(activity: models.synchronizePlots, id?: string);
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
        private pidIndex;
        constructor(data: {
            name: string;
            x: number[];
            data: number[];
        }, ps: models.jsFrame<models.ps[]>[], id?: string);
        private createPIDindex;
        private lastIndex;
        /**
         * update piechart at here
        */
        private mouseEvent;
        private static nameLabel;
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
declare namespace models {
    interface synchronizePlots {
        datasets: synchronizePartition[];
        xData: number[];
    }
    interface synchronizePartition {
        data: number[] | number[][];
        name: string;
        max?: number;
        valueDecimals: number;
        type: string;
        unit: string;
    }
}
declare namespace report {
    class index extends Bootstrap {
        get appName(): string;
        private overviews;
        private system_load;
        protected init(): void;
    }
    const click_process: string;
    function orderFrames(ps: models.jsFrame<models.ps[]>[]): models.jsFrame<models.ps[]>[];
}
