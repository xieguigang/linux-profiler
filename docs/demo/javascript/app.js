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
            if (id === void 0) { id = '#overviews'; }
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
                    marginLeft: 100,
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
                    },
                    max: dataset.max || null
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
var apps;
(function (apps) {
    var system_load = /** @class */ (function () {
        function system_load(data, ps, id) {
            if (id === void 0) { id = "#container"; }
            this.id = id;
            var vm = this;
            this.psFrames = report.orderFrames(ps);
            this.chart = Highcharts.chart(this.div = $ts(id), system_load.createPlotOptions(data));
            console.log(this.psFrames);
            /**
             * In order to synchronize tooltips and crosshairs, override the
             * built-in events with handlers defined on the parent element.
            */
            ['mousemove', 'touchmove', 'touchstart'].forEach(function (eventType) {
                vm.div.addEventListener(eventType, function (e) { return vm.mouseEvent(e); });
            });
        }
        /**
         * update piechart at here
        */
        system_load.prototype.mouseEvent = function (e) {
            // Find coordinates within the chart
            var event = this.chart.pointer.normalize(e);
            // Get the hovered point
            var point = this.chart.series[0].searchPoint(event, true);
            if ((!isNullOrUndefined(point)) && (point.index != this.lastIndex)) {
                var ps = this.findPsFrame(point.x);
                point.highlight(e);
                // update piechart at here
                // console.log(e);
                // console.log(point);
                this.updatePie(ps);
                this.updatePsFrame(ps);
                this.lastIndex = point.index;
            }
        };
        system_load.prototype.updatePie = function (ps) {
            var pi = $from(ps).Where(function (p) { return p.CPU > 0; }).Select(function (p) { return ({ name: p.COMMAND, y: p.CPU }); }).ToArray();
            $ts("#cpu-pie").clear();
            Highcharts.chart('cpu-pie', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'CPU usage percentage'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                },
                series: [{
                        name: 'Process',
                        colorByPoint: true,
                        data: pi
                    }]
            });
        };
        system_load.prototype.findPsFrame = function (time) {
            var mind = 999999;
            var minFrame = [];
            for (var _i = 0, _a = this.psFrames; _i < _a.length; _i++) {
                var frame = _a[_i];
                var delta = Math.abs(frame.timeframe - time);
                if (delta <= 1) {
                    return frame.data;
                }
                else if (delta < mind) {
                    mind = delta;
                    minFrame = frame.data;
                }
            }
            return minFrame;
        };
        system_load.prototype.updatePsFrame = function (ps) {
            $ts("#ps").clear();
            $ts.appendTable(ps, "#ps", null, { class: "table" });
        };
        system_load.createPlotOptions = function (dataset) {
            var x = dataset.x;
            // Add X values
            dataset.data = Highcharts.map(dataset.data, function (val, j) {
                return [x[j], val];
            });
            console.log(dataset);
            return {
                chart: {
                    type: 'area'
                },
                title: {
                    text: 'System Load'
                },
                subtitle: {
                    text: 'Show system load during the profiler sampling progress.'
                },
                xAxis: {
                    allowDecimals: false,
                    labels: {
                        formatter: function () {
                            return this.value; // clean, unformatted number for year
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: 'system load'
                    },
                    labels: {
                        formatter: function () {
                            return this.value;
                        }
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.y:,.0f}</b> at time point {point.x}'
                },
                plotOptions: {
                    area: {
                        pointStart: 0,
                        marker: {
                            enabled: false,
                            symbol: 'circle',
                            radius: 2,
                            states: {
                                hover: {
                                    enabled: true
                                }
                            }
                        }
                    }
                },
                series: [dataset]
            };
        };
        return system_load;
    }());
    apps.system_load = system_load;
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
            this.overviews = new apps.overviews(window[$ts("@data:sysLoad")](), $ts("@canvas:overviews"));
            this.system_load = new apps.system_load(window[$ts("@data:systemLoad")](), window[$ts("@data:ps")](), $ts("@canvas:system_load"));
        };
        return index;
    }(Bootstrap));
    report.index = index;
    function orderFrames(ps) {
        var order = [];
        var cmdl;
        for (var i = 0; i < ps.length; i++) {
            var snapshots = $from(ps[i].data).OrderByDescending(function (p) { return p.CPU; }).ToArray(false);
            for (var j = 0; j < snapshots.length; j++) {
                cmdl = snapshots[j].COMMAND;
                delete snapshots[j].COMMAND;
                delete snapshots[j].RSS;
                delete snapshots[j].STAT;
                delete snapshots[j].START;
                delete snapshots[j].TIME;
                delete snapshots[j].VSZ;
                snapshots[j].COMMAND = "<span style=\"font-size: 0.8em;\"><strong>" + cmdl + "</strong></span>";
            }
            order[i] = {
                timeframe: ps[i].timeframe,
                data: snapshots
            };
        }
        return order;
    }
    report.orderFrames = orderFrames;
})(report || (report = {}));
//# sourceMappingURL=app.js.map