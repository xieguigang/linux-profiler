namespace apps {

    export class system_load {

        private chart: Highcharts.Chart;

        public constructor(data: { name: string, data: number[] }, id: string = "container") {
            this.chart = Highcharts.chart(<any>id, <any>system_load.createPlotOptions(data));
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
                series: <any>[data]
            }
        }
    }
}