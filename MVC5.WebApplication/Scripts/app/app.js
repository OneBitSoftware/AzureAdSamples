var AzureSampleWebApplication;
(function (AzureSampleWebApplication) {
    'use strict';
    var MyClass = (function () {
        function MyClass() {
            this.app = angular.module("MVC5Application", []);
            this.app.run(function () {
                console.log('Running...1');
            });
            this.app.run(function () {
                console.log('Running...2');
            });
            this.app.run([function () {
                    console.log('Running...3');
                }]);
        }
        return MyClass;
    }());
    (function () { var main = new MyClass(); })();
})(AzureSampleWebApplication || (AzureSampleWebApplication = {}));
