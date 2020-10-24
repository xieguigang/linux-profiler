namespace report {

    export class index extends Bootstrap {

        public get appName(): string {
            return "index";
        };

        private overviews: apps.overviews;

        protected init(): void {
            let sysLoad: any = (<any>window)[<any>$ts("@data:sysLoad")]();

            this.overviews = new apps.overviews(sysLoad, <any>$ts("@canvas:overviews"));
        }

    }
}