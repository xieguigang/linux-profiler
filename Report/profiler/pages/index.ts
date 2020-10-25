namespace report {

    export class index extends Bootstrap {

        public get appName(): string {
            return "index";
        };

        private overviews: apps.overviews;
        private system_load: apps.system_load;

        protected init(): void {
            Highcharts.setOptions({
                plotOptions: {
                    series: {
                        animation: false
                    }
                }
            });

            this.overviews = new apps.overviews((<any>window)[<any>$ts("@data:sysLoad")](), <any>$ts("@canvas:overviews"));
            this.system_load = new apps.system_load(
                (<any>window)[<any>$ts("@data:systemLoad")](),
                (<any>window)[<any>$ts("@data:ps")](),
                (<any>$ts("@canvas:system_load"))
            );
        }
    }

    export const click_process: string = "click_process";

    export function orderFrames(ps: models.jsFrame<models.ps[]>[]): models.jsFrame<models.ps[]>[] {
        let order: models.jsFrame<models.ps[]>[] = [];
        let cmdl: string;

        for (let i: number = 0; i < ps.length; i++) {
            let snapshots = $from(ps[i].data).OrderByDescending(p => p.CPU).ToArray(false);

            for (let j: number = 0; j < snapshots.length; j++) {
                cmdl = snapshots[j].COMMAND;

                delete snapshots[j].COMMAND;
                delete snapshots[j].RSS;
                delete snapshots[j].STAT;
                delete snapshots[j].START;
                delete snapshots[j].TIME;
                delete snapshots[j].VSZ;

                (<any>snapshots[j]).COMMAND = `<span style="font-size: 0.8em;"><strong><a class="${click_process}" pid="${snapshots[j].PID}" href="javascript:void(0);">${cmdl}</a></strong></span>`;
                (<any>snapshots[j]).raw = cmdl;
            }

            order[i] = <models.jsFrame<models.ps[]>>{
                timeframe: ps[i].timeframe,
                data: snapshots
            }
        }

        return order;
    }
}