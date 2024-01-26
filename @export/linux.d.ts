// export R# package module type define for javascript/typescript language
//
//    imports "linux" from "LinuxProfiler";
//
// ref=Linux.Rscript@LinuxProfiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace linux {
   /**
     * @param file default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function cpuinfo(file?: string, env?: object): object;
   /**
     * @param file default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function dmidecode(file?: string, env?: object): object;
   /**
     * @param file default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function free(file?: string, env?: object): object;
   /**
     * @param file default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function iostat(file?: string, env?: object): object;
   /**
     * @param file default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function meminfo(file?: string, env?: object): object;
   /**
     * @param file default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function mpstat(file?: string, env?: object): object;
   /**
     * @param file default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function os_release(file?: string, env?: object): object;
   /**
     * @param file default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function ps(file?: string, env?: object): object;
   /**
     * @param file default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function uptime(file?: string, env?: object): object;
}
