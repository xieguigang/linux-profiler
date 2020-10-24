namespace apps {

    export class system_load {

        private chart: Highcharts.Chart;
        private div: HTMLElement;

        public constructor(data: { name: string, data: number[] }, id: string = "#container") {
            let vm = this;

            this.chart = Highcharts.chart(this.div = <any>$ts(id), <any>system_load.createPlotOptions(data));

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
                point.highlight(e);

                // update piechart at here
                console.log(e);
                console.log(point);

                this.updatePie(point, e);
                this.lastIndex = point.index;
            }
        }


        private updatePie(point, e) {

        }

        private static createPlotOptions(data: { name: string, data: number[] }) {
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
                series: <any>[data]
            }
        }
    }
}