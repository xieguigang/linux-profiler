/// <reference path="../../linq.d.ts" />
declare namespace app {
    function start(): void;
}
declare namespace report {
    class index extends Bootstrap {
        readonly appName: string;
        protected init(): void;
    }
}
