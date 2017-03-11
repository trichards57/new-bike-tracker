class DeleteFormController {
    constructor($modalInstance: angular.ui.bootstrap.IModalServiceInstance, name: string) {
        this.$modalInstance = $modalInstance;
        this.name = name;
    }

    static $inject = ["$uibModalInstance", "name"];

    private $modalInstance: angular.ui.bootstrap.IModalServiceInstance;
    name: string;

    ok() {
        this.$modalInstance.close();
    }

    cancel() {
        this.$modalInstance.dismiss("cancel");
    }
}

angular.module("app").controller("DeleteFormCtrl", DeleteFormController);