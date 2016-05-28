module AzureSampleWebApplication {
    'use strict';

    class MyClass {
        private app: ng.IModule;

        constructor() {
            this.app = angular.module("MVC5Application",[]);

            this.app.run(() => {
                console.log('Running...1');
            });

            this.app.run(function () {
                console.log('Running...2');
            });

            this.app.run([function () {
                console.log('Running...3');

            }]);

        }
    }

    (() => { var main = new MyClass();})();
}