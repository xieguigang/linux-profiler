// export R# package module type define for javascript/typescript language
//
//    imports "report" from "LinuxProfiler";
//
// ref=Linux.Reporter@LinuxProfiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace report {
   /**
   */
   function create_report(snapshotsZip: string, out: string, template: string): any;
   /**
     * @param seconds default value Is ``15``.
     * @param title default value Is ``'benchmark'``.
   */
   function profiler(save: string, seconds?: object, title?: string): object;
   module start {
      /**
      */
      function session(profiler: object): object;
   }
}
