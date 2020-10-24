var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../dev/linq.d.ts" />
/// <reference path="../dev/highcharts.d.ts" />
var app;
(function (app) {
    function start() {
        Router.AddAppHandler(new report.index());
        Router.RunApp();
    }
    app.start = start;
})(app || (app = {}));
$ts.mode = Modes.debug;
$ts(app.start);
var apps;
(function (apps) {
    var overviews = /** @class */ (function () {
        function overviews(activity, id) {
            if (id === void 0) { id = 'overviews'; }
            var _this = this;
            this.id = id;
            var x = activity.xData;
            var vm = this;
            this.overview = $ts(id);
            /*
            The purpose of this demo is to demonstrate how multiple charts on the same page
            can be linked through DOM and Highcharts events and API methods. It takes a
            standard Highcharts config with a small variation for each data set, and a
            mouse/touch event handler to bind the charts together.
            */
            /**
             * In order to synchronize tooltips and crosshairs, override the
             * built-in events with handlers defined on the parent element.
             */
            ['mousemove', 'touchmove', 'touchstart'].forEach(function (eventType) {
                vm.overview.addEventListener(eventType, function (e) { return vm.mouseEvent(e); });
            });
            /**
             * Override the reset function, we don't need to hide the tooltips and
             * crosshairs.
             */
            Highcharts.Pointer.prototype.reset = function () {
                return undefined;
            };
            /**
             * Highlight a point by showing tooltip, setting hover state and draw crosshair
             */
            Highcharts.Point.prototype.highlight = function (event) {
                event = this.series.chart.pointer.normalize(event);
                this.onMouseOver(); // Show the hover marker
                this.series.chart.tooltip.refresh(this); // Show the tooltip
                this.series.chart.xAxis[0].drawCrosshair(event, this); // Show the crosshair
            };
            // Get the data. The contents of the data file can be viewed at          
            activity.datasets.forEach(function (dataset, i) { return _this.loadLineData(x, dataset, i); });
        }
        overviews.prototype.mouseEvent = function (e) {
            var chart, point, i, event;
            for (i = 0; i < Highcharts.charts.length; i = i + 1) {
                chart = Highcharts.charts[i];
                // Find coordinates within the chart
                event = chart.pointer.normalize(e);
                // Get the hovered point
                point = chart.series[0].searchPoint(event, true);
                if (point) {
                    point.highlight(e);
                }
            }
        };
        /**
         * Synchronize zooming through the setExtremes event handler.
        */
        overviews.prototype.syncExtremes = function (e, thisChart) {
            if (e.trigger !== 'syncExtremes') {
                // Prevent feedback loop
                Highcharts.each(Highcharts.charts, function (chart) {
                    if (chart !== thisChart) {
                        if (chart.xAxis[0].setExtremes) { // It is null while updating
                            chart.xAxis[0].setExtremes(e.min, e.max, undefined, false, { trigger: 'syncExtremes' });
                        }
                    }
                });
            }
        };
        overviews.prototype.loadLineData = function (x, dataset, i) {
            var _this = this;
            var chartDiv = $ts('<div>', { class: "chart" });
            var chart;
            // Add X values
            dataset.data = Highcharts.map(dataset.data, function (val, j) {
                return [x[j], val];
            });
            this.overview.appendChild(chartDiv);
            chart = Highcharts.chart(chartDiv, {
                chart: {
                    marginLeft: 40,
                    spacingTop: 20,
                    spacingBottom: 20
                },
                title: {
                    text: dataset.name,
                    align: 'left',
                    margin: 0,
                    x: 30
                },
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                xAxis: {
                    crosshair: true,
                    events: {
                        setExtremes: function (e) { return _this.syncExtremes(e, chart); }
                    },
                    labels: {
                        format: '{value}s'
                    }
                },
                yAxis: {
                    title: {
                        text: null
                    }
                },
                tooltip: {
                    positioner: function () {
                        return {
                            // right aligned
                            x: this.chart.chartWidth - this.label.width,
                            y: 10 // align to title
                        };
                    },
                    borderWidth: 0,
                    backgroundColor: 'none',
                    pointFormat: '{point.y}',
                    headerFormat: '',
                    shadow: false,
                    style: {
                        fontSize: '18px'
                    },
                    valueDecimals: dataset.valueDecimals
                },
                series: [{
                        data: dataset.data,
                        name: dataset.name,
                        type: dataset.type,
                        color: Highcharts.getOptions().colors[i],
                        fillOpacity: 0.3,
                        tooltip: {
                            valueSuffix: ' ' + dataset.unit
                        }
                    }]
            });
        };
        return overviews;
    }());
    apps.overviews = overviews;
})(apps || (apps = {}));
var report;
(function (report) {
    var index = /** @class */ (function (_super) {
        __extends(index, _super);
        function index() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(index.prototype, "appName", {
            get: function () {
                return "index";
            },
            enumerable: true,
            configurable: true
        });
        ;
        index.prototype.init = function () {
            var sysLoad = window[$ts("@data:sysLoad")]();
            this.overviews = new apps.overviews(sysLoad, $ts("@canvas:overviews"));
        };
        return index;
    }(Bootstrap));
    report.index = index;
})(report || (report = {}));
//# sourceMappingURL=app.js.map