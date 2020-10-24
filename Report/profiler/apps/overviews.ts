namespace apps {

    export class overviews {

        private overview: HTMLElement;

        public constructor(activity: { datasets: any[], xData: number[] }, private id: string = 'overviews') {
            let x = activity.xData;
            let vm = this;

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
                vm.overview.addEventListener(eventType, e => vm.mouseEvent(e));
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
            (<any>Highcharts.Point.prototype).highlight = function (event) {
                event = this.series.chart.pointer.normalize(event);

                this.onMouseOver(); // Show the hover marker
                this.series.chart.tooltip.refresh(this); // Show the tooltip
                this.series.chart.xAxis[0].drawCrosshair(event, this); // Show the crosshair
            };

            // Get the data. The contents of the data file can be viewed at          
            activity.datasets.forEach((dataset, i) => this.loadLineData(x, dataset, i));
        }

        private mouseEvent(e) {
            let chart,
                point,
                i,
                event;

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
        }

        /**
         * Synchronize zooming through the setExtremes event handler.
        */
        private syncExtremes(e, thisChart: any) {
            if (e.trigger !== 'syncExtremes') {
                // Prevent feedback loop
                Highcharts.each(Highcharts.charts, function (chart) {
                    if (chart !== thisChart) {
                        if (chart.xAxis[0].setExtremes) { // It is null while updating
                            chart.xAxis[0].setExtremes(
                                e.min,
                                e.max,
                                undefined,
                                false,
                                { trigger: 'syncExtremes' }
                            );
                        }
                    }
                });
            }
        }

        private loadLineData(x: number[], dataset: any, i: number) {
            let chartDiv = $ts('<div>', { class: "chart" });
            let chart: Highcharts.Chart;

            // Add X values
            dataset.data = Highcharts.map(dataset.data, function (val, j) {
                return [x[j], val];
            });

            this.overview.appendChild(chartDiv);

            chart = Highcharts.chart(chartDiv, {
                chart: {
                    marginLeft: 100, // Keep all charts left aligned
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
                        setExtremes: (e) => this.syncExtremes(e, chart)
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
        }
    }
}