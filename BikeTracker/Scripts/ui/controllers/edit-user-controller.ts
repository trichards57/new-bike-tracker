class EditUserController extends BaseController {
    constructor($scope: angular.IScope, $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance,
        $window: angular.IWindowService, createMode: boolean,
        email: string, role: string) {
        if (createMode) {
            super($scope, null, $window, "New User - SJA Tracker");
            this.email = "";
            this.role = "Normal";
        } else {
            super($scope, null, $window, "Edit User - SJA Tracker");
            this.email = email;
            this.role = role;
        }

        this.$uibModalInstance = $uibModalInstance;
    }

    static $inject = ["$scope", "$uibModalInstance", "$window", "createMode", "email", "role"];

    private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance;
    email: string;
    role: string;

    ok() {
        this.$uibModalInstance.close({
            email: this.email,
            role: this.role
        });
    }

    cancel() {
        this.$uibModalInstance.dismiss("cancel");
    }
}

angular.module("app").controller("EditUserCtrl", EditUserController);