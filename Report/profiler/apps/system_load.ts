namespace apps {

    export class system_load {

        private chart: Highcharts.Chart;
        private div: HTMLElement;
        private psFrames: models.jsFrame<models.ps[]>[];

        public constructor(
            data: {
                name: string,
                x: number[],
                data: number[]
            },
            ps: models.jsFrame<models.ps[]>[],
            private id: string = "#container") {

            let vm = this;

            this.psFrames = report.orderFrames(ps);
            this.chart = Highcharts.chart(this.div = <any>$ts(id), <any>system_load.createPlotOptions(data));

            console.log(this.psFrames);

            /**
             * In order to synchronize tooltips and crosshairs, override the
             * built-in events with handlers defined on the parent element.
            */
            ['mousemove', 'touchmove', 'touchstart'].forEach(function (eventType) {
                vm.div.addEventListener(eventType, e => vm.mouseEvent(e));
            });
        }

        private lastIndex: number;

        /**
         * update piechart at here
        */
        private mouseEvent(e) {
            // Find coordinates within the chart
            let event = this.chart.pointer.normalize(e);
            // Get the hovered point
            let point = (<any>this.chart.series[0]).searchPoint(event, true);

            if ((!isNullOrUndefined(point)) && (point.index != this.lastIndex)) {
                let ps: models.ps[] = this.findPsFrame(point.x);

                point.highlight(e);

                // update piechart at here
                // console.log(e);
                // console.log(point);

                this.updatePie(ps);
                this.updatePsFrame(ps);
                this.lastIndex = point.index;
            }
        }


        private updatePie(ps: models.ps[]) {
            let pi = $from(ps).Where(p => p.CPU > 0).Select(p => <{ name: string, y: number }>{ name: p.COMMAND, y: p.CPU }).ToArray();
            let total = $from(pi).Sum(p => p.y);

            $ts("#cpu-pie").clear();

            Highcharts.chart('cpu-pie', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie',
                    animation: false
                },
                title: {
                    text: `CPU usage percentage (${Strings.round(total, 1)} %)`
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
                series: <any>[{
                    name: 'Process',
                    colorByPoint: true,
                    data: pi
                }]
            });
        }

        private findPsFrame(time: number): models.ps[] {
            let mind = 999999;
            let minFrame: models.ps[] = [];

            for (let frame of this.psFrames) {
                let delta = Math.abs(frame.timeframe - time);

                if (delta <= 1) {
                    return frame.data;
                } else if (delta < mind) {
                    mind = delta;
                    minFrame = frame.data;
                }
            }

            return minFrame;
        }

        private updatePsFrame(ps: models.ps[]) {
            $ts("#ps").clear();
            $ts.appendTable(ps, "#ps", null, { class: "table" });
        }

        private static createPlotOptions(dataset: { name: string, data: number[], x: number[] }) {
            let x = dataset.x;

            // Add X values
            dataset.data = Highcharts.map(dataset.data, function (val, j) {
                return [x[j], val];
            });

            console.log(dataset);

            return <Highcharts.Options>{
                chart: {
                    type: 'area'
                },
                title: {
                    text: 'System Load'
                },
                subtitle: {
                    text: 'Show system load during the profiler sampling progress.'
                },
                xAxis: <any>{
                    allowDecimals: false,
                    labels: {
                        formatter: function () {
                            return this.value; // clean, unformatted number for year
                        }
                    }
                },
                yAxis: <any>{
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
                series: <any>[dataset]
            }
        }
    }
}