namespace report {

    export class index extends Bootstrap {

        public get appName(): string {
            return "index";
        };

        private overviews: apps.overviews;
        private system_load: apps.system_load;

        protected init(): void {
            this.overviews = new apps.overviews((<any>window)[<any>$ts("@data:sysLoad")](), <any>$ts("@canvas:overviews"));
            this.system_load = new apps.system_load(
                (<any>window)[<any>$ts("@data:systemLoad")](),
                (<any>window)[<any>$ts("@data:ps")](),
                (<any>$ts("@canvas:system_load"))
            );
        }
    }

    export function orderFrames(ps: models.jsFrame<models.ps[]>[]): models.jsFrame<models.ps[]>[] {
        let order: models.jsFrame<models.ps[]>[] = [];

        for (let i: number = 0; i < ps.length; i++) {
            order[i] = <models.jsFrame<models.ps[]>>{
                timeframe: ps[i].timeframe,
                data: $from(ps[i].data).OrderByDescending(p => p.CPU).ToArray(false)
            }
        }

        return order;
    }
}