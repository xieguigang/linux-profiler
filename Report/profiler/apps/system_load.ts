namespace apps {

    export class system_load {

        private chart: Highcharts.Chart;
        private div: HTMLElement;
        private psFrames: models.jsFrame<models.ps[]>[];
        private pidIndex: {} = {};

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
            this.pidIndex = this.createPIDindex();
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

        private createPIDindex(): any {
            let index: {} = this.pidIndex;
            let pidtemp: { proc: models.ps, time: number }[] = [];

            for (let frame of this.psFrames) {
                for (let process of frame.data) {
                    pidtemp.push({
                        proc: process,
                        time: frame.timeframe
                    });
                }
            }

            let pid_groups = $from(pidtemp)
                .GroupBy(p => p.proc.PID)
                .Select(p => $from(p).OrderBy(p => p.time).ToArray(false))
                .ToArray(false);

            for (let group of pid_groups) {
                index[group[0].proc.PID.toString()] = group;
            }

            return index;
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

                if (isNullOrUndefined(ps) || ps.length == 0) {
                    return;
                }

                point.highlight(e);

                // update piechart at here
                // console.log(e);
                // console.log(point);

                this.updatePie(ps);
                this.updatePsFrame(ps);
                this.lastIndex = point.index;
            }
        }

        private static nameLabel(command: models.ps): string {
            let label: string = `[#${command.PID}] ${(<any>command).raw}`;

            if (label.length < 64) {
                return label;
            } else {
                return label.substr(0, 63) + "~";
            }
        }

        private updatePie(ps: models.ps[]) {
            let vm = this;
            let pi = $from(ps)
                .Where(p => p.CPU > 0)
                .Select(function (p) {
                    return {
                        name: system_load.nameLabel(p),
                        y: p.CPU,
                        id: p.PID,
                        events: {
                            click: function () {
                                vm.showByPID(p.PID.toString());
                            }
                        }
                    }
                })
                .ToArray();
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
            let vm = this;

            $ts("#ps").clear();
            $ts.appendTable(ps, "#ps", ["USER", "PID", "CPU", "MEM", "TTY", "COMMAND"], { class: "table" });
            // add click event handles
            $ts.select(`.${report.click_process}`).onClick((sender, evt) => vm.showByPID(sender.getAttribute("pid")));
        }

        private showByPID(pid: string) {
            let line: { proc: models.ps, time: number }[] = this.pidIndex[pid];
            let proc: models.ps = line[0].proc;
            let CPU = $from(line).Select(p => p.proc.CPU).ToArray(false);
            let memory = $from(line).Select(p => p.proc.MEM).ToArray(false);
            let timeline: number[] = $from(line).Select(p => p.time).ToArray(false);

            $ts("#summary").display(`<p>User: ${proc.USER}</p><p>TTY: ${proc.TTY}</p><p>PID: ${pid}</p><p>COMMAND: ${proc.COMMAND}</p>`);
            $ts("#ps_view").clear();

            let plot = new apps.overviews(<models.synchronizePlots>{
                xData: timeline,
                datasets: [
                    <models.synchronizePartition>{ name: "CPU Usage", valueDecimals: 1, type: "area", unit: "%", data: CPU },
                    <models.synchronizePartition>{ name: "Memory Usage", valueDecimals: 1, type: "area", unit: "%", data: memory }
                ]
            }, "#ps_view");
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