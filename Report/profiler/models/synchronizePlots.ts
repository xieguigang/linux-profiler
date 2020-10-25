namespace models {

    export interface synchronizePlots {
        datasets: synchronizePartition[];
        xData: number[];
    }

    export interface synchronizePartition {
        data: number[] | number[][];
        name: string;
        max?: number;
        valueDecimals: number;
        type: string;
        unit: string;
    }
}