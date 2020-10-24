namespace models {

    export interface jsFrame<T> {
        timeframe: number;
        data: T;
    }

    export interface ps {
        USER: string;
        PID: number;
        CPU: number;
        MEM: number;
        VSZ: string
        RSS: string
        TTY: string
        STAT: string
        START: string
        TIME: string
        COMMAND: string
    }
}