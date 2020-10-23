var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../linq.d.ts" />
var app;
(function (app) {
    function start() {
        Router.AddAppHandler(new report.index());
        Router.RunApp();
    }
    app.start = start;
})(app || (app = {}));
$ts.mode = Modes.debug;
$ts(app.start);
var report;
(function (report) {
    var index = /** @class */ (function (_super) {
        __extends(index, _super);
        function index() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Object.defineProperty(index.prototype, "appName", {
            get: function () {
                return "index";
            },
            enumerable: true,
            configurable: true
        });
        ;
        index.prototype.init = function () {
            throw new Error("Method not implemented.");
        };
        return index;
    }(Bootstrap));
    report.index = index;
})(report || (report = {}));
//# sourceMappingURL=app.js.map